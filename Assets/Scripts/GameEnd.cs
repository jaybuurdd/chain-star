using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject ballPrefab;
    public Transform spawnPoint;

    private bool canDetectCollisions = false;
    private bool firstCollisionEnded = false;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canDetectCollisions) return;

        Collider2D otherCollider = collision.collider;

        if (otherCollider.CompareTag("Ground"))
        {
            Debug.Log("Collision detected with " + otherCollider.name);

            scoreManager.GameOver();
            // Disable the SpriteRenderer and Collider2D components of the ball
            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            if (ball != null)
            {
                SpriteRenderer ballSpriteRenderer = ball.GetComponent<SpriteRenderer>();
                Collider2D ballCollider = ball.GetComponent<Collider2D>();
                ballSpriteRenderer.enabled = false;
                ballCollider.enabled = false;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D otherCollider = collision.collider;

        if (!firstCollisionEnded && otherCollider.CompareTag("Ground"))
        {
            Debug.Log("First collision with the ground has ended.");
            firstCollisionEnded = true;
            canDetectCollisions = true;
        }
    }


}
