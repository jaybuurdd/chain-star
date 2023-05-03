using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour
{
    public GameObject player; // the player object

    GameObject ball;    
    private float ballHitForce = 10f;


    public GameObject weaponPrefab;

    private GameObject spawnedWeapon;
    public float weaponSpeed = 5f;
    private bool weaponReturning = false;

    public CharacterController2D characterController;
    public ScoreManager scoreManager;

    public AudioSource audioSource;
    //public AudioClip clip;

    public Animator animator; // the animator component attached to the player
    public float attackDelay = 0.5f; // the delay between attacks in seconds
    public Dictionary<string, AudioClip> attackSounds; // map of attack move names to audio clips

    private bool isAttacking = false; // flag to prevent attacking during an attack animation
    private string[] attacks = {"jab", "upStrike", "duplex", "spinStrike","suplex","powerStrike"}; // array of all possible attacks
    private string[] upgradeAttacks = {"t"};
    private int currentAttack = -1; // the index of the current attack in the array
    private bool upgradeApplied = false;

     void Start()
    {
        // Get the AudioSource component attached to the player object
        audioSource = player.GetComponent<AudioSource>();
        ball = GameObject.Find("Ball");
        weaponPrefab = Resources.Load<GameObject>("Prefabs/Weapon");

        // Initialize the attack sounds dictionary
        attackSounds = new Dictionary<string, AudioClip>();
        attackSounds.Add("jab", Resources.Load<AudioClip>("Sounds/attack-light"));
        attackSounds.Add("upStrike", Resources.Load<AudioClip>("Sounds/attack-med"));
        attackSounds.Add("duplex", Resources.Load<AudioClip>("Sounds/attack-heavy"));
        attackSounds.Add("spinStrike", Resources.Load<AudioClip>("Sounds/attack-heavy"));
        attackSounds.Add("suplex", Resources.Load<AudioClip>("Sounds/attack-heavy"));
        attackSounds.Add("powerStrike", Resources.Load<AudioClip>("Sounds/attack-med"));


    }


    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isJumping = stateInfo.IsName("Player_Jumping");
        if (!isJumping && Input.GetKeyDown(KeyCode.W) && !isAttacking)
        {
            Debug.Log("attacking!");
            isAttacking = true;
            StartCoroutine(DoAttack());
            animator.SetLayerWeight(1, 0f);
        }

        int playerScore = scoreManager.GetScore();
        // if(playerScore >= 1000 && !upgradeApplied){
        //     Debug.Log("New UPGRADE!");
        //     // add a random combo to take from combo list and move into attacks list
        //     string randomCombo = upgradeAttacks[Random.Range(0, upgradeAttacks.Length)];
        //     attacks = attacks.Concat(new string[]{randomCombo}).ToArray();
        //     upgradeApplied = true;
        // }
        if(!isJumping && Input.GetKeyDown(KeyCode.Space)){
            
            Debug.Log("Last minute save!");
            Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();
            ballRigidbody.gravityScale = 0.0f; // set the gravity scale to 2

            animator.SetBool("isThrown", true);
            ballRigidbody.gravityScale = 0.1f;
            animator.SetLayerWeight(1, 1.0f);
        }

    }

    IEnumerator DoAttack()
    {

        // select a random attack from the array, ensuring that it's not the same as the previous attack
        int newAttack;
        do {
            newAttack = Random.Range(0, attacks.Length);
        } while (newAttack == currentAttack);
        currentAttack = newAttack;

        // play the corresponding animation
        animator.SetBool(attacks[currentAttack], true);
        Debug.Log("attack set");

        // play the attack sound effect
        if (attackSounds.ContainsKey(attacks[currentAttack]))
        {
            AudioClip attackClip = attackSounds[attacks[currentAttack]];
            if (attackClip != null)
            {
                Debug.Log("Play: " + attackClip);
                audioSource.clip = attackClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Audio clip is null: " + attacks[currentAttack]);
            }
        }
        else
        {
            Debug.LogWarning("Attack sound not found: " + attacks[currentAttack]);
        }

        // wait for the attack animation to finish
        yield return new WaitForSeconds(attackDelay);

    }


    public void OnThrowEnd()
    {
        // Set isThrown back to false to return to the default animation
        animator.SetBool("isThrown", false);
        animator.SetBool("stand", true);


        // Spawn the weapon and make it follow and collide with the ball
        StartCoroutine(SpawnWeaponAndFollowBall());
        // Set the layer weight back to 0 to deactivate the second layer
        //animator.SetLayerWeight(1, 0.0f);
    }

    IEnumerator SpawnWeaponAndFollowBall()
    {
        // Get the player's facing direction
        float playerFacingDirection = characterController.IsFacingLeft() ? 1f : -1f;

        // Spawn the weapon prefab in front of the player
        Vector3 spawnPosition = player.transform.position + new Vector3(1.5f * playerFacingDirection, 0, 0);
        spawnedWeapon = Instantiate(weaponPrefab, spawnPosition, Quaternion.identity);

        // Calculate the target position (ball's position)
        Vector3 target = ball.transform.position;

        // Move the weapon towards the target position
        while (Vector3.Distance(spawnedWeapon.transform.position, target) > 0.1f)
        {
            spawnedWeapon.transform.position = Vector3.MoveTowards(spawnedWeapon.transform.position, target, weaponSpeed * Time.deltaTime);
            yield return null;
        }

        // Start the Coroutine to check for ball collision
        StartCoroutine(CheckForBallCollision());

        // Perform actions on collision with the ball
        //scoreManager.AddPoints(1000);

        // Move the weapon back towards the player
        target = player.transform.position;
        while (Vector3.Distance(spawnedWeapon.transform.position, target) > 0.1f)
        {
            spawnedWeapon.transform.position = Vector3.MoveTowards(spawnedWeapon.transform.position, target, weaponSpeed * Time.deltaTime);
            yield return null;
        }

        // Destroy the weapon and reset the state
        Destroy(spawnedWeapon);
        weaponReturning = false;

        // Activate the Catch Throw animation
        animator.SetBool("catch", true);
        animator.SetBool("stand", false);
        //animator.SetLayerWeight(1, 0.0f);
    }

    IEnumerator CheckForBallCollision()
    {
        // Set up a flag to check if the weapon has collided with the ball
        bool weaponCollided = false;

        // Keep checking for collision until the weapon collides with the ball
        while (!weaponCollided)
        {
            if (spawnedWeapon != null && spawnedWeapon.GetComponent<Collider2D>().bounds.Intersects(ball.GetComponent<Collider2D>().bounds))
            {
                weaponCollided = true;

                // Get the ball's Rigidbody2D component
                Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();

                // Calculate the direction from the weapon to the ball
                Vector2 direction = (ball.transform.position - spawnedWeapon.transform.position).normalized;

                // Apply the force to the ball
                ballRigidbody.AddForce(direction * ballHitForce, ForceMode2D.Impulse);

                // Perform actions on collision with the ball
                scoreManager.AddPoints(1000);
            }

            yield return null;
        }

    }


    public void OnCatchThrowEnd()
    {
        // Reset the CatchThrow trigger
        animator.SetBool("catch", false);
        animator.SetBool("isThrown", false);

        // Transition to the EmptyState on Layer 1
        //animator.Play("EmptyState", 1);
        
        // Set the layer weight back to 0 to blend back to the base layer
        //animator.SetLayerWeight(1, 0.0f);
        animator.Play("Exit", 1);
    }



    public void OnAttackEnd()
    {
        // reset the animation state and allow attacking again
        animator.SetBool(attacks[currentAttack], false);
        isAttacking = false;

        // call other code that needs to happen after the attack ends
        // ...
    }
}
