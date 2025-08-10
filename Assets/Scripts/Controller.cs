using UnityEngine;

// Base class for Player and AI controllers. Registers/unregisters with GameManager.
public class Controller : MonoBehaviour
{
    protected virtual void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterController(this);
        else
            Debug.LogWarning($"{name}: GameManager.Instance is null; could not register Controller. " +
                             "Ensure a GameManager is in the scene.");
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterController(this);
    }

    // For player input or AI logic to override if needed
    public virtual void ProcessInput() { }
}

