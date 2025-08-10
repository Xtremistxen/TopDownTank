using UnityEngine;

public class Powerup_Speed : PowerupBase
{
    public float moveMultiplier = 1.5f;
    public float rotateMultiplier = 1.2f;

    public override void OnApply(PickupManager target)
    {
        if (!target || !target.mover) return;
        target.mover.moveSpeed *= moveMultiplier;
        target.mover.turnSpeed *= rotateMultiplier;
    }

    public override void OnExpire(PickupManager target)
    {
        if (!target || !target.mover) return;
        // Revert by dividing
        target.mover.moveSpeed /= moveMultiplier;
        target.mover.turnSpeed /= rotateMultiplier;
    }
}

