using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArialAttackController : MonoBehaviour
{
    public GameObject player; // the player object
    public CharacterController2D controller;
    public Animator animator; // the animator component attached to the player
    public float attackDelay = 0.5f; // the delay between attacks in seconds

    private bool isAttacking = false; // flag to prevent attacking during an attack animation
    private string[] attacks = {"Player_Arial_Spin_Strike","Player_Strike_Down"}; //"duplex", "arialStrike", "arialSlash" }; // array of all possible attacks
    private int currentAttack = -1; // the index of the current attack in the array

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isJumping = stateInfo.IsName("Player_Jumping");

        if (isJumping && Input.GetKeyDown(KeyCode.W) && !isAttacking)
        {
            animator.SetBool("isJumping", false);
            isAttacking = true;
            StartCoroutine(DoArialAttack());
        }
    }

    IEnumerator DoArialAttack()
    {
        int attacksPlayed = 0; // count how many attacks have been played

        while (attacksPlayed < attacks.Length)
        {
            bool inAir = !controller.m_Grounded;
        // select a random attack from the array, ensuring that it's not the same as the previous attack
        int newAttack;
        do {
            newAttack = Random.Range(0, attacks.Length);
            
        } while (newAttack == currentAttack);
        currentAttack = newAttack;
        animator.CrossFade(attacks[currentAttack], 0.5f);

        // reset the animation state for the current attack
        switch (attacks[currentAttack])
        {
            case "Player_Arial_Spin_Strike":
                Debug.Log("start arial spin");
                animator.SetBool("arialSpin", true);
                break;
            case "Player_Combo_Strike":
                Debug.Log("start arial strike down");
                animator.SetBool("arialSlash", true);
                break;
            // add more cases for each attack
        }

        // increase the attack count
        attacksPlayed++;

        // wait for the attack animation to finish
        yield return new WaitForSeconds(attackDelay);
        }

        currentAttack = -1; // reset the index to allow attacks to be played again
    }


    public void OnArialAttackEnd()
    {

            Debug.Log("END ARIAL ATTACK");

            animator.SetBool("arialSpin", false);
            animator.SetBool("arialSlash", false);
            isAttacking = false;

        // call other code that needs to happen after the attack ends
        // ...
    }

}
