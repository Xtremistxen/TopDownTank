using UnityEngine;

public class Powerup_Health : PowerupBase
{
    public float healAmount = 25f;

    public override void OnApply(PickupManager target)
    {
        if (target && target.health)
        {
            target.health.currentHealth = Mathf.Min(
                target.health.currentHealth + healAmount,
                target.health.maxHealth
            );
        }

        
    }
    
}

