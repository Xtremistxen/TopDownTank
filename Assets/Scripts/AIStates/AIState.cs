using UnityEngine;
public abstract class AIState : MonoBehaviour
{
    // FSM will cheack the state of our AI such as Idle, Chase, Flee and Guard.
    protected AITankController controller;

    public void Init(AITankController ctrl)

    {
        controller = ctrl;
    }

    public abstract void EnterState(); // first state is Idle and this will check if AI went to another state
    public abstract void ExitState(); // When the AI leave a state to another
    public abstract void UpdateState(); // updates the current state
}

