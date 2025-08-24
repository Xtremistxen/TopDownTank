using UnityEngine;

public class FoodPoints : MonoBehaviour
{
    [Tooltip("How many points this fruit gives the player.")]
    public int points = 10;

    public FoodPoints()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Award points via ScoreManager singleton
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(points);
        }

        // Remove the fruit
        Destroy(gameObject);
    }
}
