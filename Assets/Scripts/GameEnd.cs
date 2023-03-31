using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public ScoreManager scoreManager;

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
