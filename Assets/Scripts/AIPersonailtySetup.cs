using UnityEngine;

public enum AIPersonalityType { Coward, Aggressor, Sniper, Balanced }

[RequireComponent(typeof(AITankController))]
public class AIPersonalitySetup : MonoBehaviour
{
    public AIPersonalityType personality = AIPersonalityType.Balanced;

    void Start()
    {
        AITankController controller = GetComponent<AITankController>();
        controller.personality = personality; // Ensure it’s synced

        switch (personality)
        {
            case AIPersonalityType.Coward:
                controller.visionRange = 5f;
                controller.fleeHealthThreshold = 100f;
                controller.stopShootDistance = 0f;
                controller.shootRange = 0f;
                controller.SwitchState(controller.GetComponent<IdleState>());
                break;

            case AIPersonalityType.Aggressor:
                controller.visionRange = 10f;
                controller.fleeHealthThreshold = 0f;
                controller.stopShootDistance = 3f;
                controller.shootRange = 5f;
                controller.SwitchState(controller.GetComponent<ChaseState>());
                break;

            case AIPersonalityType.Sniper:
                controller.visionRange = 20f;
                controller.fleeHealthThreshold = 25f;
                controller.stopShootDistance = 10f;
                controller.shootRange = 15f;
                controller.SwitchState(controller.GetComponent<PatrolState>());
                break;

            case AIPersonalityType.Balanced:
                controller.visionRange = 6f;
                controller.fleeHealthThreshold = 25f;
                controller.stopShootDistance = 4f;
                controller.shootRange = 6f;
                controller.SwitchState(controller.GetComponent<PatrolState>());
                break;
        }
    }
}

