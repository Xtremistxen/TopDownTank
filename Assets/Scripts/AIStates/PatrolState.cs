using UnityEngine;

public class PatrolState : AIState
{
    // AI tank given patrol points will go to each point and loop each point.
    private int currentIndex = 0;
    private float waypointThreshold = 0.5f;
    private float waitTimer = 0f;
    private float waitAtWaypoint = 1f;
    private bool waiting = false;

    public override void EnterState()
    {
        currentIndex = 0;
        waiting = false;
        waitTimer = 0f;
    }

    public override void UpdateState()
    {
        float distanceToPlayer = controller.DistanceToPlayer();

        if (controller.personality == AIPersonalityType.Coward && distanceToPlayer < controller.visionRange)
        {
            controller.SwitchState(controller.GetComponent<FleeState>());
            return;
        }
        
        if (controller.personality == AIPersonalityType.Sniper)
        {
            if (distanceToPlayer <= controller.shootRange)
            {
                controller.Shooter.Fire();
            }

            if (distanceToPlayer < 4f)
            {
                controller.SwitchState(controller.GetComponent<FleeState>());
                return;
            }
        }

        if ((controller.personality == AIPersonalityType.Balanced || controller.personality == AIPersonalityType.Aggressor)
            && distanceToPlayer < controller.visionRange)
        {
            controller.SwitchState(controller.GetComponent<ChaseState>());
            return;
        }

        if (controller.CurrentHealth() <= controller.fleeHealthThreshold)
        {
            controller.SwitchState(controller.GetComponent<FleeState>());
            return;
        }

        if (controller.patrolPoints == null || controller.patrolPoints.Length == 0)
        {
            controller.SwitchState(controller.GetComponent<IdleState>());
            return;
        }

        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtWaypoint)
            {
                waiting = false;
                waitTimer = 0f;
                currentIndex++;

                if (currentIndex >= controller.patrolPoints.Length)
                {
                    if (controller.loopPatrol)
                        currentIndex = 0;
                    else
                    {
                        controller.SwitchState(controller.GetComponent<IdleState>());
                        return;
                    }
                }
            }

            return;
        }

        Transform target = controller.patrolPoints[currentIndex];
        Vector2 currentPos = new Vector2(controller.transform.position.x, controller.transform.position.y);
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        Vector2 toWaypoint = (targetPos - currentPos).normalized;
        Vector2 forward2D = new Vector2(controller.transform.up.x, controller.transform.up.y);

        float angle = Vector2.SignedAngle(forward2D, toWaypoint);

        if (Mathf.Abs(angle) > 5f)
        {
            if (angle > 0f)
                controller.Mover.TurnLeft();
            else
                controller.Mover.TurnRight();
        }
        else
        {
            controller.Mover.MoveForward();
        }

        if (Vector2.Distance(currentPos, targetPos) <= waypointThreshold)
        {
            waiting = true;
            waitTimer = 0f;
        }
    }

    public override void ExitState() { }
}



