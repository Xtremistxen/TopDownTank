using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Max health for player/AI
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)//if health reaches 0 they die
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Destroy the GameObject or trigger death effects
        Destroy(gameObject);
    }
}

