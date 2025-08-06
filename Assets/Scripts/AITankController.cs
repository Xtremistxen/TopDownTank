using UnityEngine;

public class AITankController : MonoBehaviour
{
    public AIPersonalityType personality; // Choose what ever personality type
    public AIState currentState; // checks what state the AI is in Idle, flee, chase, patroll
    public Transform player; // detects the player
    public float visionRange; // Checks if player is within vision
    public float fleeHealthThreshold; // checks health and flees at X%
    public float stopShootDistance; // stop to shoot the player
    public float shootRange; // Range at which the projectile goes

    private TankMover mover; // Moves the tanks
    private TankShooter shooter; // allows the tanks to shoot
    private Health health; // gathers the tank health

    public Transform[] patrolPoints; // Patrols the points
    public bool loopPatrol = true; // can be true or false if patrolling 

    public TankMover Mover => mover;
    public TankShooter Shooter => shooter;
    public float viewAngle = 90f; // Field of View in degrees


    void Start() // Allows tanks to get the components needed
    {
        health = GetComponent<Health>();
        mover = GetComponent<TankMover>();
        shooter = GetComponent<TankShooter>();
        player = GameObject.FindWithTag("Player")?.transform;

        SwitchState(GetComponent<PatrolState>());
    }

    void Update()
    {
        currentState?.UpdateState();
    }


    public bool CanSeePlayer()
{
    if (player == null) return false;

    Vector3 directionToPlayer = (player.position - transform.position).normalized;
    float angleToPlayer = Vector3.Angle(transform.up, directionToPlayer);

    // Check angle
    if (angleToPlayer > viewAngle / 2f || DistanceToPlayer() > visionRange)
        return false;

    // Line of sight raycast
    RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, visionRange);
    if (hit.collider != null)
    {
        // Did we hit the player?
        if (hit.collider.CompareTag("Player"))
        {
            return true;
        }
    }

    return false;
}



    public void SwitchState(AIState newState) // Checks the states at which the AI enters/exits
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.Init(this);
        currentState.EnterState();
    }

    public float DistanceToPlayer()
    {
        return player != null ? Vector3.Distance(transform.position, player.position) : float.MaxValue;
    }

    public float CurrentHealth()
    {
        return health != null ? health.currentHealth : 100f;
    }

    void OnDrawGizmosSelected()
{
    Gizmos.color = Color.yellow;

    Vector3 forward = transform.up;
    Quaternion leftRayRotation = Quaternion.Euler(0, 0, -viewAngle / 2);
    Quaternion rightRayRotation = Quaternion.Euler(0, 0, viewAngle / 2);

    Vector3 leftRay = leftRayRotation * forward;
    Vector3 rightRay = rightRayRotation * forward;

    Gizmos.DrawRay(transform.position, leftRay * visionRange);
    Gizmos.DrawRay(transform.position, rightRay * visionRange);
}
     
}











