using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Static instance for global access
    public static ScoreManager instance;

    // Current score
    [Header("Score Settings")]
    [SerializeField]
    private int score = 0;


    // Property to get the current score
    public int Score
    {
        get { return score; }
    }

    private void Awake()
    {
        // Singleton pattern - only one ScoreManager should exist
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add score when a collectible is destroyed
    public void AddScore(int points)
    {
        score += points;
    }

    // Reset score (useful for restarting the game)
    public void ResetScore()
    {
        score = 0;
        Debug.Log("Score reset to zero.");
    }
}
