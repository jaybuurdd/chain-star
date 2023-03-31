using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Collider2D objectCollider;
    public float forceMultiplier = 10f;

    public ScoreManager scoreManager;
    public int hitPoints = 1;

    private void Start()
    {
        objectCollider = GetComponent<Collider2D>();
        Debug.Log("The collider assigned to the script is: " + objectCollider.name);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Ball"))
        {
            Debug.Log("Collision detected with " + otherCollider.name);

            Vector2 direction = new Vector2(0f, 1f);
            Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
            ballRigidbody.AddForce(direction * forceMultiplier, ForceMode2D.Impulse);
            scoreManager.AddPoints(hitPoints);
        }
    }
}

