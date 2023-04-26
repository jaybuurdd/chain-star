using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Collider2D objectCollider;
    public float forceMultiplier = 10f;

    public ScoreManager scoreManager;
    public int hitPoints = 1;
    public GameObject weakHitEffectPrefab;
    public float hitEffectOffset = 0.5f;

    private bool hasWeakHitEffect = false;

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

            if (scoreManager != null)
            {
                scoreManager.AddPoints(hitPoints);
            }
            else
            {
                Debug.LogError("ScoreManager is not assigned in WeaponScript.");
            }

            if (!hasWeakHitEffect) // Check if weak hit effect has already been instantiated
            {
                // Instantiate hit effect prefab
                GameObject weakHitEffect = Instantiate(weakHitEffectPrefab, otherCollider.transform.position, Quaternion.identity);
                // Offset hit effect position so it's not behind the ball
                weakHitEffect.transform.position += new Vector3(0f, 0.5f, 0f);
                // Destroy hit effect after 0.05 second
                Destroy(weakHitEffect, 0.05f);
                
                hasWeakHitEffect = true; // Set flag to true to indicate that weak hit effect has been instantiated
            }
            
            // hit stop effect
            StartCoroutine(FreezeWeapon());

            
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Ball"))
        {
            Debug.Log("Ball collided with border.");

            Vector2 direction = otherCollider.transform.position - GetComponent<Collider2D>().bounds.center;
            Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
            ballRigidbody.AddForce(direction.normalized * 1f, ForceMode2D.Impulse);
        }
    }

    IEnumerator FreezeWeapon()
    {
        Debug.Log("HIT STOP ACTIVATED!");
        objectCollider.enabled = false;

        float freezeTime = 0.01f; // hit stop length
        Rigidbody2D weaponRigidbody = GetComponent<Rigidbody2D>();

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1f; 

        StartCoroutine(EnableCollider());
    
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSecondsRealtime(0.05f); // Wait for 0.05 seconds
        objectCollider.enabled = true; // Re-enable the collider
        hasWeakHitEffect = false;
    }


}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WeaponScript : MonoBehaviour
// {
//     private Collider2D objectCollider;
//     public float forceMultiplier = 10f;

//     public ScoreManager scoreManager;
//     public int hitPoints = 1;

//     private void Start()
//     {
//         objectCollider = GetComponent<Collider2D>();
//         Debug.Log("The collider assigned to the script is: " + objectCollider.name);
//     }

//     private void OnTriggerEnter2D(Collider2D otherCollider)
//     {
//         if (otherCollider.CompareTag("Ball"))
//         {
//             Debug.Log("Collision detected with " + otherCollider.name);

//             Vector2 direction = new Vector2(0f, 1f);
//             Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
//             ballRigidbody.AddForce(direction * forceMultiplier, ForceMode2D.Impulse);

//             if (scoreManager != null)
//             {
//                 scoreManager.AddPoints(hitPoints);
//             }
//             else
//             {
//                 Debug.LogError("ScoreManager is not assigned in WeaponScript.");
//             }

//             // hit stop effect
//             StartCoroutine(FreezeWeapon());
//         }
        
//     }

//     private void OnTriggerExit2D(Collider2D otherCollider)
//     {
//         if (otherCollider.CompareTag("Ball"))
//         {
//             Debug.Log("Ball collided with border.");

//             Vector2 direction = otherCollider.transform.position - transform.position;
//             Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
//             ballRigidbody.AddForce(direction.normalized * forceMultiplier, ForceMode2D.Impulse);
//         }
//     }

//     IEnumerator FreezeWeapon()
//         {
//             float freezeTime = 0.1f; // hit stop length
//             Rigidbody2D weaponRigidbody = GetComponent<Rigidbody2D>();
            
//             weaponRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
//             yield return new WaitForSeconds(freezeTime);
//             weaponRigidbody.constraints = RigidbodyConstraints2D.None;
//         }
// }

