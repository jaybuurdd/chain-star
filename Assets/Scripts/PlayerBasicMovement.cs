using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float moveSpeed = 60f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;

    float horizontalMove = 0f;
    bool jump = false;
    private bool isAttacking = false;
    bool attack = false;
    bool strikeDown = false;
    bool jab = false;
    bool arialStike = false;
    bool comboStrike = false;

    private bool canDoublePress = false;
    private float doublePressTime = 0.5f; // The time window for the double press

     // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("You jumped!");
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jump = true;
            animator.SetBool("isJumping", true); 
            
      
        }

        // Check if the player is in the air
        bool inAir = !controller.m_Grounded;
        if (inAir)
        {
            // Activate the ArialSpinStrike animation
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!attack)
                {
                    if (canDoublePress && Time.time <= doublePressTime)
                    {
                        // Double press detected, do something
                        Debug.Log("Double press detected!");
                    }
                    else
                    {
                        attack = arialStike = true;
                        OnAttacking();

                        canDoublePress = true;
                        doublePressTime = Time.time + 0.5f; // Set the time window for the double press
                    }
                }
                
                attack = arialStike = false;
                
            }
        }
        else {
            // if strike down attack
            if (Input.GetKeyDown(KeyCode.Q))
            {
            
                if (!attack) {
                    
                    attack = strikeDown = true;
                    OnAttacking();
                }
                attack = strikeDown = false;
                //animator.SetBool("isStrikeDown", true);
            } 

            // if jab attack
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(!attack) {
                    
                    attack = jab = true;
                    OnAttacking();
                }
                attack = jab = false;
            } 

            // if combo strike
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!attack) {
                    attack = comboStrike = true;
                    OnAttacking();
                }

                attack = comboStrike = false;
            }
        }

        // Reset the double press flag if the time window has passed
        if (Time.time > doublePressTime)
        {
            canDoublePress = false;
        }
    }

    public void OnLanding() {
        animator.SetBool("isJumping", false);
        
    }

    public void OnAttacking ()
    {
        Debug.Log("Attacking: " + attack);
        if(attack && strikeDown){
            Debug.Log("You striked!");
            animator.SetBool("isStrikeDown", true);
        }
        if(attack && jab){
            Debug.Log("You jabbed!");
            animator.SetBool("isJab", true);
        }
        if(attack && comboStrike){
            Debug.Log("You combo striked!");
            animator.SetBool("isComboStrike",true);
        }

        // Check if the player is in the air and executing the arialStrike attack
        //bool inAir = !controller.m_Grounded;
        if (attack && arialStike)
        {
            Debug.Log("You arial spin striked!");
            animator.CrossFade("Player_Arial_Spin_Strike", 0.5f);
            animator.SetBool("isArialSpinStrike", true);
        }
    }


    public void OnAttackEnd()
	{
        Debug.Log("Ending Attack");
        if(!attack && !strikeDown)
            animator.SetBool("isStrikeDown", false);
        if(!attack && !jab)
            animator.SetBool("isJab", false);
        if(!attack && !comboStrike){
            animator.SetBool("isComboStrike", false);
        }

        // Check if the player is in the air and executing the arialStrike attack
        bool inAir = !controller.m_Grounded;
        if (!attack && !arialStike)
        {
            Debug.Log("Arial strike ended");
            //animator.CrossFade("Player_Jumping", 0.5f);
            animator.SetBool("isArialSpinStrike", false);
            
        }
	}

    
    void FixedUpdate() {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        jump = false;
        attack = false;
    }
}