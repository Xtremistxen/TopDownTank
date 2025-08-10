using UnityEngine;

public class FleeState : AIState
{

    // This AI state will check if the AI is coward or reaches threshold for Health
    public override void EnterState() { }

    public override void UpdateState()
    {   // Will detect the player and begin to flee in a direction away from the player and stop if far away enough
        Vector3 dir = (controller.transform.position - controller.player.position).normalized;
        float angle = Vector3.SignedAngle(controller.transform.up, dir, Vector3.forward);

        if (Mathf.Abs(angle) > 5f)
        {
            if (angle > 0f) controller.Mover.TurnLeft();
            else            controller.Mover.TurnRight();
        }

        if (Mathf.Abs(angle) < 45f)
        {
            controller.Mover.MoveForward();
        }
        
        if (controller.CurrentHealth() > controller.fleeHealthThreshold &&
            controller.personality != AIPersonalityType.Coward)
        {
            controller.SwitchState(controller.GetComponent<IdleState>());
        }
    }

    public override void ExitState() { }
}



