using UnityEngine;

public class Controller : MonoBehaviour
{
    protected virtual void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterController(this);
        else
            Debug.LogWarning($"{name}: GameManager.Instance is null; could not register Controller.");
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterController(this);
    }

    public virtual void ProcessInput() { }
}


