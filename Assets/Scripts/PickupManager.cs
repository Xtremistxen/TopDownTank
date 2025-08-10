using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMover))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(Health))]
public class PickupManager : MonoBehaviour
{
    [Header("Cached Components")]
    public TankMover mover;
    public TankShooter shooter;
    public Health health;

    // Track active timed powerups
    private readonly List<PowerupInstance> activePowerups = new();

    void Awake()
    {
        if (!mover) mover = GetComponent<TankMover>();
        if (!shooter) shooter = GetComponent<TankShooter>();
        if (!health) health = GetComponent<Health>();
    }

    public void ApplyPowerup(PowerupBase powerup)
    {
        if (powerup == null) return;

        // Clone a runtime instance so each pickup has its own timer
        PowerupBase runtime = Instantiate(powerup);
        float duration = runtime.duration;

        // Apply immediately
        runtime.OnApply(this);

        if (duration > 0f)
        {
            // Track and start expiry
            var inst = new PowerupInstance { powerup = runtime };
            activePowerups.Add(inst);
            StartCoroutine(ExpireAfter(duration, inst));
        }
        else
        {
            // Permanent effect — nothing else to do
        }
    }

    private IEnumerator ExpireAfter(float seconds, PowerupInstance inst)
    {
        yield return new WaitForSeconds(seconds);

        if (inst != null && inst.powerup != null)
        {
            inst.powerup.OnExpire(this);
            activePowerups.Remove(inst);
            Destroy(inst.powerup);
        }
    }

    private class PowerupInstance
    {
        public PowerupBase powerup;
    }
}
