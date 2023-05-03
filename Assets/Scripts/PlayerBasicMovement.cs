using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    private CharacterController2D controller;
    public WeaponScript ws;
    public Animator animator;
    public ScoreManager scoreManager;

    public float moveSpeed = 60f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;

    float horizontalMove = 0f;
    bool jump = false;


     // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController2D>();
        ws = FindObjectOfType<WeaponScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Input Enabled? " + controller.isInputEnabled);
        if (controller.isInputEnabled)
        {
            //Debug.Log("Moving");
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

            animator.SetFloat("speed", Mathf.Abs(horizontalMove));
        }

        
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("You jumped!");
        //     rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //     jump = true;
        //     animator.SetBool("isJumping", true); 
              
        // }

    }


    public void OnLanding() {
        animator.SetBool("isJumping", false);
        
    }

    public void OnHitEnd(){
         // reset the animation state and allow attacking again
        //animator.SetBool("hit", false);
        ws.OnHitAnimationEnd();
        //Invoke("EnableMoving", 1f);

    }

    void FixedUpdate() {
        // move character
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        jump = false;
        //attack = false;
    }
}