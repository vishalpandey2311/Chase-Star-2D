// This is Game Manager Script.

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static instance for global access
    public static GameManager instance;


    //Scene for Restarting the game
    [Header("Scene Settings")]
    [SerializeField]
    private string mainSceneName = "MainScene";

    [Header("Blur Effect Settings")]
    [SerializeField]
    private GameObject globalVolume;

    // Game state
    private bool isGameOver = false;

    // Property to check if game is over
    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    private void Awake()
    {
        // Singleton pattern - only one GameManager should exist
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when the player is caught
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Pause the game

        // Activate the blur effect
        if (globalVolume != null)
        {
            globalVolume.SetActive(true);
            Debug.Log("Global Volume activated - Blur effect applied.");
        }
        else
        {
            Debug.LogWarning("Global Volume is not assigned in GameManager!");
        }

        Debug.Log("Game Over! Game paused.");
    }

    // Reset game state (for restarting)
    public void RestartGame()
    {
        // Resume time before reloading
        Time.timeScale = 1f;

        // Reset all singleton instances
        if (ScoreManager.instance != null)
        {
            Destroy(ScoreManager.instance.gameObject);
            ScoreManager.instance = null;
        }

        if (EnemySpawner.instance != null)
        {
            Destroy(EnemySpawner.instance.gameObject);
            EnemySpawner.instance = null;
        }

        if (SpawnManager.instance != null)
        {
            Destroy(SpawnManager.instance.gameObject);
            SpawnManager.instance = null;
        }

        // Deactivate the blur effect
        if (globalVolume != null)
        {
            globalVolume.SetActive(false);
            Debug.Log("Global Volume deactivated - Blur effect removed.");
        }

        // Reset game state
        isGameOver = false;

        // Reload the scene
        SceneManager.LoadScene(mainSceneName);
        Debug.Log("Reloading scene: " + mainSceneName);
    }

    // Quit the game
    public void QuitGame()
    {
        Time.timeScale = 1f; // Make sure time is running before quitting
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
