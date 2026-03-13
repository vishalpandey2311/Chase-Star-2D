using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    [Header("Game Over UI Settings")]

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private string gameOverMessage = "Game Over!";

    [SerializeField]
    private Color gameOverTextColor = Color.red;

    [SerializeField]
    private float textSize = 80f;

    // Flag to track if game over has been displayed
    private bool hasDisplayedGameOver = false;

    private void Start()
    {
        if (gameOverText == null)
        {
            Debug.LogError("Game Over Text is not assigned in GameOverDisplay!");
            return;
        }

        // Initially hide the game over panel, game over text and restart button
        gameOverPanel.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check if game is over
        if (GameManager.instance != null && GameManager.instance.IsGameOver && !hasDisplayedGameOver)
        {
            DisplayGameOver();
            hasDisplayedGameOver = true;
        }
    }

    // Display the game over text and restart button
    private void DisplayGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);

        // Show the game over text
        gameOverText.gameObject.SetActive(true);

        // Set the message
        gameOverText.text = gameOverMessage;

        // Set the color
        gameOverText.color = gameOverTextColor;

        // Set the font size
        gameOverText.fontSize = textSize;

        // Show the restart button
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
            Debug.Log("Restart button activated!");
        }
        else
        {
            Debug.LogWarning("Restart button is not assigned in GameOverDisplay!");
        }

        Debug.Log("Game Over UI Displayed!");
    }

    // Optional: Reset for restart
    public void ResetDisplay()
    {
        gameOverPanel.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        hasDisplayedGameOver = false;
    }
}
