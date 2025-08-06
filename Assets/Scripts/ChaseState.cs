using UnityEngine;

public class ChaseState : AIState
{

    // When the player gets close the AI depending on personailty will chase the player down
    public override void EnterState() { }

    public override void UpdateState()
    {
        float distance = controller.DistanceToPlayer();
        Vector3 direction = (controller.player.position - controller.transform.position).normalized;
        float angle = Vector3.SignedAngle(controller.transform.up, direction, Vector3.forward);

        if (Mathf.Abs(angle) > 5f)
        {
            if (angle > 0) controller.Mover.TurnLeft();
            else controller.Mover.TurnRight();
        }

        if (distance > controller.stopShootDistance && Mathf.Abs(angle) < 45f)
        {
            controller.Mover.MoveForward();
        }

        if (distance <= controller.shootRange && Mathf.Abs(angle) < 10f)
        {
            controller.Shooter.Fire();
        }

        if (controller.CurrentHealth() <= controller.fleeHealthThreshold &&
            controller.personality != AIPersonalityType.Aggressor)
        {
            controller.SwitchState(controller.GetComponent<FleeState>());
            return;
        }

        if (distance > controller.visionRange)
        {
            controller.SwitchState(controller.GetComponent<IdleState>());
        }
    }

    public override void ExitState() { }
}







