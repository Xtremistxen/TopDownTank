using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TankMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float input)
    {
        Vector2 moveDirection = transform.up * input * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    public void Turn(float input)
    {
        float rotationAmount = -input * turnSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);
    }

    // Helper methods used by your AI states:
    public void MoveForward()  => Move(1f);
    public void MoveBackward() => Move(-1f);

    public void TurnLeft()  => Turn(-1f); // rotate counter‑clockwise
    public void TurnRight() => Turn(1f);  // rotate clockwise
}



