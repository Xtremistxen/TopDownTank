using System;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject projectilePrefab;   // The projectile to fire
    public Transform firePoint;            // Where the projectile spawns
    public float fireRate = 1f;            // Shots per second

    private float fireCooldown = 0f;

    void Update()
    {
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
    }


    // Attempts to shoot a projectile if cooldown has passed.
    public void Fire() 
    {
        if (fireCooldown <= 0f)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                fireCooldown = 1f / fireRate;
            }
            else
            {
                Debug.LogWarning("ProjectilePrefab or FirePoint not assigned on TankShooter.");
            }
        }
    }
}
