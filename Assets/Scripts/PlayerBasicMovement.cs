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
    bool attack = false;
    bool strikeDown = false;
    bool jab = false;

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
            }
            //animator.SetBool("isStrikeDown", true);
        } else if (Input.GetKeyUp(KeyCode.Z))
        {
            attack = strikeDown = false;
        }

        // if jab attack
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(!attack) {
                attack = jab = true;
            }
        } else  if (Input.GetKeyUp(KeyCode.X))
        {
            attack = jab = false;
        }


    
    }

    public void OnLanding() {
        animator.SetBool("isJumping", false);
        
    }

    public void OnAttacking (bool isAttacking)
    {
        animator.SetBool("isAttack", isAttacking);

        if(attack == strikeDown){
            animator.SetBool("isStrikeDown", isAttacking);
        }
        if(attack == jab){
            animator.SetBool("isJab", isAttacking);
        }
      
    }


    void FixedUpdate() {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, attack);
        jump = false;
        // attack = false;
    }
}
