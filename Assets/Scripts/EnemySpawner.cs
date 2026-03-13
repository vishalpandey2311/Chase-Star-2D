using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private int pointsPerEnemy = 10;

    // Static instance for global access
    public static EnemySpawner instance;

    // Track current enemy count
    private int currentEnemyCount = 0;
    private int lastSpawnedAtScore = -10;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Spawn the initial enemy
        SpawnEnemy();
        currentEnemyCount = 1;
        lastSpawnedAtScore = 0;
    }

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is not assigned in EnemySpawner!");
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints array is empty or not assigned in EnemySpawner!");
        }

        
    }

    private void Update()
    {
        // Check if we need to spawn a new enemy based on score
        if (ScoreManager.instance != null)
        {
            int currentScore = ScoreManager.instance.Score;
            int requiredEnemyCount = (currentScore / pointsPerEnemy) + 1;

            // Spawn new enemyif needed
            if (requiredEnemyCount > currentEnemyCount)
            {
                SpawnEnemy();
                currentEnemyCount++;
                lastSpawnedAtScore = currentScore;
                Debug.Log("New enemy spawned! Total enemies: " + currentEnemyCount + " at score: " + currentScore);
            }
        }
    }

    // Spawn a single enemy at a random waypoint
    private void SpawnEnemy()
    {
        if (enemyPrefab == null || waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Cannot spawn enemy: prefab or waypoints not assigned!");
            return;
        }

        // Select a random waypoint
        int randomWaypointIndex = Random.Range(0, waypoints.Length);
        Vector3 spawnPosition = waypoints[randomWaypointIndex].position;

        // Instantiate the enemy at the random waypoint
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Get the FollowThePath component and set its starting waypoint
        FollowThePath followThePath = newEnemy.GetComponent<FollowThePath>();
        if (followThePath != null)
        {
            // Set the waypoints and starting waypoint index
            followThePath.SetWaypoints(waypoints, randomWaypointIndex);
            Debug.Log("Enemy spawned at waypoint: " + randomWaypointIndex);
        }
        else
        {
            Debug.LogError("Enemy prefab does not have FollowThePath component!");
        }
    }
}
