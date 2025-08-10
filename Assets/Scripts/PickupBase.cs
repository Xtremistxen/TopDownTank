using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PickupBase : MonoBehaviour
{
    [Tooltip("Optional: Assign a powerup to grant on pickup")]
    public PowerupBase powerupToGrant;

    [Tooltip("If true, this pickup destroys itself immediately after being collected (Spawner will handle respawn).")]
    public bool destroyOnPickup = true;

    protected virtual void Awake()
    {
        // Ensure trigger collider
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var picker = other.GetComponent<PickupManager>();
        if (picker == null) return;

        OnPicked(picker);

        // Try to inform a spawner in parents
        var spawner = GetComponentInParent<PickupSpawner>();
        if (spawner != null) spawner.HandleCollected();

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    protected virtual void OnPicked(PickupManager picker)
    {
        if (powerupToGrant != null)
        {
            picker.ApplyPowerup(powerupToGrant);
        }
    }
}

