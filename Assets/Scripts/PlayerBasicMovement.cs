using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool directionLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("speed",Mathf.Abs(horizontal));

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // flip sprite when player changes direction (right)
        if(horizontal > 0 && !directionLeft){
            flip();
        }
        else if (horizontal < 0 && directionLeft){
            flip();
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        
    }

    // flip sprite horizontally
    void flip(){
        directionLeft = !directionLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
