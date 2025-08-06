using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;       // Speed of projectile
    public float lifetime = 5f;     // Time before it's auto-destroyed

    public float damage = 25f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * speed;  // Move forward in local direction
        }

        Destroy(gameObject, lifetime); // Destroys object
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile hit: " + other.name);

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }

        Destroy(gameObject); // destroy bullet on hit
    }
}




