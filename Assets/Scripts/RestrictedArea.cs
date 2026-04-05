using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    // Reference to the player
    private GameObject player;

    // Array to track all enemies with FollowThePath component
    private FollowThePath[] enemies;

    // Flag to prevent processing trigger events during scene shutdown
    private bool isSceneShuttingDown = false;

    private void OnDestroy()
    {
        // Mark that scene is shutting down to prevent trigger logic
        isSceneShuttingDown = true;
    }

    private void Start()
    {
        // Find the player GameObject (assumes it has tag "Player")
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("RestrictedArea: Player GameObject not found! Make sure it has the 'Player' tag.");
        }
    }

    // Called when a collider enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered is the player
        if (collision.CompareTag("Player"))
        {
            // Find all enemies with FollowThePath component
            enemies = FindObjectsByType<FollowThePath>(FindObjectsSortMode.None);

            // Start chasing for all enemies
            foreach (FollowThePath enemy in enemies)
            {
                enemy.StartChase(player.transform);
            }

            Debug.Log("Player entered restricted area. " + enemies.Length + " enemies are now chasing!");
        }
    }

    // Called when a collider exits the trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Skip if scene is shutting down to prevent accessing destroyed objects
        if (isSceneShuttingDown)
        {
            return;
        }

        // Check if the object that exited is the player
        if (collision.CompareTag("Player"))
        {
            // Find all enemies with FollowThePath component
            enemies = FindObjectsByType<FollowThePath>(FindObjectsSortMode.None);

            // Stop chasing for all enemies and resume patrolling
            foreach (FollowThePath enemy in enemies)
            {
                // Null check to prevent accessing destroyed enemies
                if (enemy != null && enemy.gameObject != null)
                {
                    enemy.StopChase();
                }
            }

            Debug.Log("Player exited restricted area. Enemies are now patrolling!");
        }
    }
}
