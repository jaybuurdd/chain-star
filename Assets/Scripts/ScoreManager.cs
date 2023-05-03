using UnityEngine;
using UnityEngine.UI; // For Unity 2019.3 or older
using TMPro; // For Unity 2019.4 or newer
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    // For Unity 2019.3 or older:
    // public Text scoreText;

    // For Unity 2019.4 or newer:
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button replayButton;

    private int score;
    private int endScore;


    void Start()
    {
        score = 0;
        UpdateScoreText();

        // hide game over and replay ui at start
        gameOverText.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);

        // add listener for replay button
        replayButton.onClick.AddListener(RestartGame);
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public int GetScore(){
        return score;
    }

    public void GameOver()
    {
        Debug.Log("Game Over triggered");
        endScore = score;
        score = -1;
        UpdateScoreText();

        gameOverText.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
         Debug.Log("Restarting game");
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        // Re-enable the ball's SpriteRenderer and Collider2D components
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            SpriteRenderer ballSpriteRenderer = ball.GetComponent<SpriteRenderer>();
            Collider2D ballCollider = ball.GetComponent<Collider2D>();
            ballSpriteRenderer.enabled = true;
            ballCollider.enabled = true;
        }  
    }


    private void UpdateScoreText()
    {
        if(score == -1){
            scoreText.text = "--";
        }
        else{
            scoreText.text = "Score: " + score;
        }
    }
}

