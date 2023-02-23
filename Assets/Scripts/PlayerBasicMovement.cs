using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float moveSpeed = 40f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;

    float horizontalMove = 0f;
    bool jump = false;
    bool attack = false;
    bool strikeDown = false;

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
        
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jump = true;
            animator.SetBool("isJumping", true);
      
        }

        if (Input.GetKeyDown(KeyCode.S)){

            if (!attack) {
                attack = strikeDown = true;
            }
            //animator.SetBool("isStrikeDown", true);
        } else if (Input.GetKeyUp(KeyCode.S))
        {
            attack = strikeDown = false;
        }


    
    }

    public void OnLanding() {
        animator.SetBool("isJumping", false);
        
    }

    public void OnAttacking (bool isAttacking)
    {
        animator.SetBool("isStrikeDown", isAttacking);
    }


    void FixedUpdate() {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, strikeDown);
        jump = false;
        // attack = false;
    }
}
