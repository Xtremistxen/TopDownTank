using System.Diagnostics;
using UnityEngine;
// This is the base class for all of the controllers players and AI
// and is responsible for controlling pawns
// It does this by automaticlly register with the gamemanager
public class Controller : MonoBehaviour
{
    protected virtual void Start()// Registers with the gamemanager
    {
        GameManager.Instance.RegisterController(this);
    }

    protected virtual void OnDestroy() // unregisters with the gamemanager
    {
        GameManager.Instance.UnregisterController(this);
    }

    public virtual void ProcessInput()
    {
        
    }
}
