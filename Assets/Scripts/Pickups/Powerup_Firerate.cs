using UnityEngine;

public class Powerup_FireRate : PowerupBase
{
    public float rateMultiplier = 1.5f; // 1.5x faster (higher fireRate)

    public override void OnApply(PickupManager target)
    {
        if (!target || !target.shooter) return;
        target.shooter.fireRate *= rateMultiplier;
    }

    public override void OnExpire(PickupManager target)
    {
        if (!target || !target.shooter) return;
        target.shooter.fireRate /= rateMultiplier;
    }
}

