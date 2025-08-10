using UnityEngine;

public abstract class PowerupBase : MonoBehaviour
{
    [Tooltip("If > 0, powerup expires after this many seconds; if 0 or less, it's permanent.")]
    public float duration = 0f;

    public abstract void OnApply(PickupManager target);
    public virtual void OnExpire(PickupManager target) { }
}

