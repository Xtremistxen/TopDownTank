using UnityEngine;

public class IdleState : AIState
{

    //Most tanks will be in the idle such as sniper or if not patrolling
    // Depending if the player is within range the AI will either chase or flee
    private float idleTimer = 0f;
    private float maxIdleTime = 3f;

    public override void EnterState()
    {
        idleTimer = 0f;
    }

    public override void UpdateState()
    {
        float distanceToPlayer = controller.DistanceToPlayer();

        if (controller.personality == AIPersonalityType.Coward && distanceToPlayer < controller.visionRange)
        {
            controller.SwitchState(controller.GetComponent<FleeState>());
            return;
        }

        if (controller.personality == AIPersonalityType.Aggressor && distanceToPlayer < controller.visionRange)
        {
            controller.SwitchState(controller.GetComponent<ChaseState>());
            return;
        }

        if (controller.personality == AIPersonalityType.Balanced && distanceToPlayer < controller.visionRange)
        {
            if (controller.CurrentHealth() > controller.fleeHealthThreshold)
                controller.SwitchState(controller.GetComponent<ChaseState>());
            else
                controller.SwitchState(controller.GetComponent<FleeState>());

            return;
        }

        idleTimer += Time.deltaTime;
        if (idleTimer >= maxIdleTime)
        {
            controller.SwitchState(controller.GetComponent<PatrolState>());
        }
    }

    public override void ExitState() { }
}

