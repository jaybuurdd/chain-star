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
    bool comboStrike = false;

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

        // if strike down attack
        if (Input.GetKeyDown(KeyCode.Z))
        {
        
            if (!attack) {
                
                attack = strikeDown = true;
                OnAttacking();
            }
            attack = strikeDown = false;
            //animator.SetBool("isStrikeDown", true);
        } 

        // if jab attack
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(!attack) {
                
                attack = jab = true;
                OnAttacking();
            }
            attack = jab = false;
        } 

        // if combo strike
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(!attack) {
                attack = comboStrike = true;
                OnAttacking();
            }

            attack = comboStrike = false;
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
    }

    public void OnAttackEnd()
	{
        
        if(!attack && !strikeDown)
            animator.SetBool("isStrikeDown", false);
        if(!attack && !jab)
            animator.SetBool("isJab", false);
        if(!attack && !comboStrike)
            animator.SetBool("isComboStrike", false);
		//m_wasAttacking = false;
		//OnAttackEvent.Invoke();
	}

    
    void FixedUpdate() {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        jump = false;
        attack = false;
    }
}