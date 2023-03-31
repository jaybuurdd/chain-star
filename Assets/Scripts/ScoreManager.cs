using UnityEngine;
using UnityEngine.UI; // For Unity 2019.3 or older
using TMPro; // For Unity 2019.4 or newer

public class ScoreManager : MonoBehaviour
{
    // For Unity 2019.3 or older:
    // public Text scoreText;

    // For Unity 2019.4 or newer:
    public TextMeshProUGUI scoreText;

    private int score;

    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void GameOver()
    {
        score = -1;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if(score == -1){
            scoreText.text = "GAME OVER";
        }
        else{
            scoreText.text = "Score: " + score;
        }
    }
}

