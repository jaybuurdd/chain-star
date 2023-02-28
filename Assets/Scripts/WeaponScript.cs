using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public float forceMultiplier = 10f;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Ball"))
        {
            Debug.Log("Collision detected with " + otherCollider.name);

            Vector2 direction = new Vector2(0f, 1f);
            Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
            ballRigidbody.AddForce(direction * forceMultiplier, ForceMode2D.Impulse);
        }
    }
}

