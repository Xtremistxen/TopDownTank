using UnityEngine;
public abstract class AIState : MonoBehaviour
{
    // FSM will cheack the state of our AI such as Idle, Chase, Flee and Guard.
    protected AITankController controller;

    public void Init(AITankController ctrl)

    {
        controller = ctrl;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}

