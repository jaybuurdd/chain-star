using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private Collider2D objectCollider;
    public Animator animator;
    public CharacterController2D controller;
    private Rigidbody2D player;
    public float forceMultiplier = 10f;

    private AudioSource hitSound;
    private AudioClip ballHitSound;
    private AudioClip[] weaponHitSounds;
    private AudioClip[] weaponMissSounds;

    private bool hasBeenHit = false;

    public ScoreManager scoreManager;
    public int hitPoints = 1;

    public GameObject weakHitEffectPrefab;
    public float hitEffectOffset = 0.5f;

    private bool hasWeakHitEffect = false;
    private bool collision = false;
    private float consecutiveCeilingCollisions = 0f;

    private void Start()
    {
        objectCollider = GetComponent<Collider2D>();
        controller = FindObjectOfType<CharacterController2D>();
        player = transform.parent.GetComponent<Rigidbody2D>();
     
        hitSound = GetComponent<AudioSource>();
        ballHitSound = Resources.Load<AudioClip>("Sounds/hit");
        weaponHitSounds = Resources.LoadAll<AudioClip>("Sounds/weapon-hit");
        weaponMissSounds = Resources.LoadAll<AudioClip>("Sounds/weapon-miss");
        //Debug.Log("The collider assigned to the script is: " + objectCollider.name);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        collision = false;
        //if (otherCollider == null) return;

        if(objectCollider.name == "hitbox")
        {
            collision = true;

            if (otherCollider.CompareTag("Ball"))
            {
                // hit sound
                int randomIndex = Random.Range(0, weaponHitSounds.Length - 1);
                hitSound.PlayOneShot(weaponHitSounds[randomIndex]);

                //Debug.Log("Collision detected with " + otherCollider.name);
                consecutiveCeilingCollisions = 0f;

                Vector2 direction = new Vector2(0f, 2f);
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
                    weakHitEffect.transform.position += new Vector3(0f, hitEffectOffset, 0f);
                    // Destroy hit effect after 0.05 second
                    Destroy(weakHitEffect, 0.05f);
                    
                    hasWeakHitEffect = true; // Set flag to true to indicate that weak hit effect has been instantiated
                }

                
                
                // hit stop effect
                StartCoroutine(FreezeWeapon());    
            }

            if (otherCollider.CompareTag("Ground") && !collision){
                // miss sound
                Debug.Log("Collision detected with " + otherCollider.name);
                int randomMissIndex = Random.Range(0, weaponMissSounds.Length);
                hitSound.PlayOneShot(weaponMissSounds[randomMissIndex]);
            }
        }
        
        if(objectCollider.name == "ceiling")
        {
            if (otherCollider.CompareTag("Ball"))
            {
                //player.velocity = Vector2.zero;
                //controller.StopMoving();
                //Debug.Log("Collision detected with " + otherCollider.name);
                consecutiveCeilingCollisions += 5f;
                //Debug.Log(" head streak = " + consecutiveCeilingCollisions);

                // Generate a random angle between 0 and 360 degrees
                float randomAngle = Mathf.Pow(Random.Range(0f, 1f), 2f) * 45f;

                // Convert the angle to a direction vector
                Vector2 direction = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
                Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
                ballRigidbody.AddForce(direction * consecutiveCeilingCollisions, ForceMode2D.Impulse);

                if (scoreManager != null)
                {
                    scoreManager.AddPoints(20);
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
                    weakHitEffect.transform.position += new Vector3(0f, hitEffectOffset, 0f);
                    // Destroy hit effect after 0.05 second
                    Destroy(weakHitEffect, 0.05f);
                    
                    hasWeakHitEffect = true; // Set flag to true to indicate that weak hit effect has been instantiated
                    hasBeenHit = true;
                }
            
                //Debug.Log("hit sound " + hitSound);
                hitSound.PlayOneShot(ballHitSound);
                animator.SetBool("hit", true);
                // hit stop effect
                //StartCoroutine(FreezeWeapon());  
            }
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



    // IEnumerator FreezeWeapon()
    // {
    //     Debug.Log("HIT STOP ACTIVATED!");
    //     objectCollider.enabled = false;

    //     float freezeTime = 0.1f; // hit stop length

    //     Rigidbody2D ballRigidbody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
    //     ballRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    //     ballRigidbody.gravityScale = 0f;

    //     yield return new WaitForSecondsRealtime(freezeTime);

    //     ballRigidbody.constraints = RigidbodyConstraints2D.None;
    //     ballRigidbody.gravityScale = 1f;

    //      float randomAngle = Mathf.Pow(Random.Range(0f, 1f), 2f) * 45f;

    //     // Convert the angle to a direction vector
    //     Vector2 direction = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), Mathf.Cos(randomAngle * Mathf.Deg2Rad));
    //     //Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
    //     ballRigidbody.AddForce(direction * forceMultiplier, ForceMode2D.Impulse);
    //     // Vector2 direction = new Vector2(0f, 1f);
    //     // //Rigidbody2D ballRigidbody = otherCollider.GetComponent<Rigidbody2D>();
    //     // ballRigidbody.AddForce(direction * forceMultiplier, ForceMode2D.Impulse);

    //     StartCoroutine(EnableCollider());
    //}


    IEnumerator FreezeWeapon()
    {
        Debug.Log("HIT STOP ACTIVATED!");
        //objectCollider.enabled = false;

        float freezeTime = 0.1f; // hit stop length
        Rigidbody2D weaponRigidbody = GetComponent<Rigidbody2D>();
        Rigidbody2D ballRigidbody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1f; 
        ballRigidbody.gravityScale = 1.5f;
        StartCoroutine(EnableCollider());
    
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSecondsRealtime(0.1f); // Wait for 0.05 seconds
        //objectCollider.enabled = true; // Re-enable the collider
        hasWeakHitEffect = false;
    }

    public void OnHitAnimationEnd()
    {
        // Reset the flag after the "hit" animation finishes
        animator.SetBool("hit", false);
        //hasBeenHit = false;
        Debug.Log("hit ended");
    }


}


