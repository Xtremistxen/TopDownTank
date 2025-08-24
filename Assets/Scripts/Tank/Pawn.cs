using System;
using UnityEngine;


/// Base class for all pawn types. Inherit from this to make new controllable units.
public abstract class Pawn : MonoBehaviour
{
    // Handles movement input to be implemented by subclasses
   
    public abstract void Move(float moveInput);
    // Handles turning input to be implemented by subclasses.
    public abstract void Turn(float turnInput);

    public virtual void Shoot() 
    {
        Debug.Log("TankPawn Shoot() called");
    }
}



