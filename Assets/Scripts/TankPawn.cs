using UnityEngine;

// Central hub for the tank. Connects movement, shooting, and other components.
public class TankPawn : Pawn
{
    private TankMover mover;
    private TankShooter shooter;
    private Health health;   

    private void Awake()
    {

        mover = GetComponent<TankMover>(); 
        shooter = GetComponent<TankShooter>();
        health = GetComponent<Health>();
    }

    public override void Move(float input)
    {
        if (mover != null)
            mover.Move(input);
    }

    public override void Turn(float input)
    {
        if (mover != null)
            mover.Turn(input);
    }

    public override void Shoot()
    {
        if (shooter != null)
        {
           shooter.Fire();
        }
    }

     public void TakeDamage(float amount)
     {
         if (health != null)
             health.TakeDamage(amount);
    }
}


