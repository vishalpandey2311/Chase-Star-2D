using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Called when the scene loads
    private void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text is not assigned in ScoreDisplay!");
            return;
        }

        // Update the display immediately
        UpdateScoreDisplay();
    }

    // Called every frame to update the score display
    private void Update()
    {
        UpdateScoreDisplay();
    }

    // Update the score text with formatted display
    private void UpdateScoreDisplay()
    {
        if (ScoreManager.instance != null)
        {
            // Format: {Score - 005} where 005 is the score padded with zeros
            int currentScore = ScoreManager.instance.Score;
            scoreText.text = $"Score - {currentScore:D3}";
        }
    }
}
