using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Area Coordinates")]
    [SerializeField]
    private Vector2 upLeft = new Vector2(-5.8f, 3.8f);

    [SerializeField]
    private Vector2 upRight = new Vector2(5.8f, 3.8f);

    [SerializeField]
    private Vector2 downLeft = new Vector2(-5.8f, -3.8f);

    [SerializeField]
    private Vector2 downRight = new Vector2(5.8f, -3.8f);

    [Header("Spawn Settings")]
    [SerializeField]
    private GameObject starPrefab;

    [SerializeField]
    private int starsToSpawn = 5;

    [SerializeField]
    private float spawnInterval = 0.5f;

    // Static instance for global access
    public static SpawnManager instance;

    // Object Pool - List to store all star instances
    private List<GameObject> starPool = new List<GameObject>();
    private int activeStars = 0;

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
    }

    private void Start()
    {
        // Create the object pool with all star instances
        CreateStarPool();
        // Spawn initial batch of stars
        SpawnStars();
    }

    // Create the object pool - instantiate all stars at start
    private void CreateStarPool()
    {
        if (starPrefab == null)
        {
            Debug.LogError("StarPrefab is not assigned in SpawnManager!");
            return;
        }

        for (int i = 0; i < starsToSpawn; i++)
        {
            // Create star instance
            GameObject star = Instantiate(starPrefab);
            star.SetActive(false); // Disable initially
            starPool.Add(star);
        }

        Debug.Log("Star pool created with " + starsToSpawn + " stars.");
    }

    // Spawn a batch of stars with intervals
    private void SpawnStars()
    {
        StartCoroutine(SpawnStarsWithInterval());
    }

    // Coroutine to spawn stars with a delay between each spawn
    private IEnumerator SpawnStarsWithInterval()
    {
        for (int i = 0; i < starPool.Count && i < starsToSpawn; i++)
        {
            // Get a star from the pool
            GameObject star = starPool[i];
            
            // Set random position
            Vector2 randomPosition = GetRandomPositionInArea();
            star.transform.position = randomPosition;
            
            // Enable the star
            star.SetActive(true);
            activeStars++;

            // Wait for the interval before spawning the next star
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Get a random position within the rectangular spawn area
    private Vector2 GetRandomPositionInArea()
    {
        // Get the boundaries from the four corners
        float minX = Mathf.Min(upLeft.x, downLeft.x);
        float maxX = Mathf.Max(upRight.x, downRight.x);
        float minY = Mathf.Min(downLeft.y, downRight.y);
        float maxY = Mathf.Max(upLeft.y, upRight.y);

        // Generate random position within the area
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    // Reposition a collected star to a new random location
    public static void RepositionStar(GameObject star)
    {
        if (instance != null)
        {
            // Get new random position
            Vector2 newPosition = instance.GetRandomPositionInArea();
            star.transform.position = newPosition;
            
            // Re-enable the star
            star.SetActive(true);
            
            Debug.Log("Star repositioned to: " + newPosition);
        }
    }

    // Called when a star is destroyed (kept for compatibility)
    public static void StarDestroyed()
    {
        // This can be removed now as we're using RepositionStar instead
        Debug.Log("StarDestroyed called (using Object Pooling now)");
    }
}
