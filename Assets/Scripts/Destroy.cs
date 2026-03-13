using UnityEngine;

public class Destroy : MonoBehaviour
{
    [Header("Score Settings")]
    [Tooltip("Points awarded for collecting this star")]
    [SerializeField]
    private int scoreValue = 10;

    // Called when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            // Add score to the ScoreManager
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(scoreValue);
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene!");
            }

            // Disable the star and reposition it (Object Pooling)
            if (SpawnManager.instance != null)
            {
                gameObject.SetActive(false); // Disable first
                SpawnManager.RepositionStar(gameObject); // Reposition and re-enable
            }
            else
            {
                Debug.LogError("SpawnManager not found in the scene!");
            }
        }
    }
}
