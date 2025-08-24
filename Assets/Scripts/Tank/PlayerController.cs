using UnityEngine;

public class PlayerController : Controller
{
    private Pawn pawn;

    protected override void Start()
    {
        pawn = GetComponent<TankPawn>();
    }

    void Update()
    {
        if (pawn != null)
        {
            ProcessInput();
        }
    }

    public override void ProcessInput() // These inputs will give the tank a more realistic turning, such as 2d rotation is reversed
    {
        float moveInput = 0f;
        float turnInput = 0f;

        if (Input.GetKey(KeyCode.W)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.S)) moveInput = -1f;

        if (Input.GetKey(KeyCode.A)) turnInput = 1f; 
        else if (Input.GetKey(KeyCode.D)) turnInput = -1f;

        pawn.Move(moveInput);
        pawn.Turn(turnInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pawn.Shoot();
        }
    }
}




