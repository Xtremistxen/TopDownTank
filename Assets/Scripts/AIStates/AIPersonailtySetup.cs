using UnityEngine;

public enum AIPersonalityType { Coward, Aggressor, Sniper, Balanced }// The types of personality you can choose from

[RequireComponent(typeof(AITankController))]
public class AIPersonalitySetup : MonoBehaviour
{
    public AIPersonalityType personality = AIPersonalityType.Balanced;

    void Start()
    {   // Each personality will have their own default settings
        AITankController controller = GetComponent<AITankController>();
        controller.personality = personality; // Ensure it’s synced

        switch (personality)
        {
            case AIPersonalityType.Coward: // will never engage the player and will run way instead
                controller.visionRange = 5f;
                controller.fleeHealthThreshold = 100f;
                controller.stopShootDistance = 0f;
                controller.shootRange = 0f;
                controller.SwitchState(controller.GetComponent<IdleState>());
                break;

            case AIPersonalityType.Aggressor: // will chase down the player aggresively 
                controller.visionRange = 10f;
                controller.fleeHealthThreshold = 0f;
                controller.stopShootDistance = 3f;
                controller.shootRange = 5f;
                controller.SwitchState(controller.GetComponent<ChaseState>());
                break;

            case AIPersonalityType.Sniper: // shoots from a far advantage
                controller.visionRange = 20f;
                controller.fleeHealthThreshold = 25f;
                controller.stopShootDistance = 10f;
                controller.shootRange = 15f;
                controller.SwitchState(controller.GetComponent<PatrolState>());
                break;

            case AIPersonalityType.Balanced: // typical balanced tank
                controller.visionRange = 6f;
                controller.fleeHealthThreshold = 25f;
                controller.stopShootDistance = 4f;
                controller.shootRange = 6f;
                controller.SwitchState(controller.GetComponent<PatrolState>());
                break;
        }
    }
}

