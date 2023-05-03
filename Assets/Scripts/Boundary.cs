// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Boundary : MonoBehaviour
// {
//     public float borderSize = 0.1f;
//     public Transform player;
//     public Transform ball;

//     void Update()
//     {
//         float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + borderSize;
//         float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - borderSize;
//         //float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - borderSize;
//         float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + borderSize;

//         if (player.position.x < leftBorder)
//         {
//             player.position = new Vector3(leftBorder, player.position.y, player.position.z);
//         }   
//         else if (player.position.x > rightBorder)
//         {
//             player.position = new Vector3(rightBorder, player.position.y, player.position.z);
//         }

//         // if (player.position.y > topBorder)
//         // {
//         //     player.position = new Vector3(player.position.x, topBorder, player.position.z);
//         // }
//         if (player.position.y < bottomBorder)
//         {
//             player.position = new Vector3(player.position.x, bottomBorder, player.position.z);
//         }

//         if (ball.position.x < leftBorder)
//         {
//             ball.position = new Vector3(leftBorder, ball.position.y, ball.position.z);
//             ball.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(ball.GetComponent<Rigidbody2D>().velocity.x), ball.GetComponent<Rigidbody2D>().velocity.y);
//         }   
//         else if (ball.position.x > rightBorder)
//         {
//             ball.position = new Vector3(rightBorder, ball.position.y, ball.position.z);
//             ball.GetComponent<Rigidbody2D>().velocity = new Vector2(-Mathf.Abs(ball.GetComponent<Rigidbody2D>().velocity.x), ball.GetComponent<Rigidbody2D>().velocity.y);
//         }

//         // if (ball.position.y > topBorder)
//         // {
//         //     ball.position = new Vector3(ball.position.x, topBorder, ball.position.z);
//         //     ball.GetComponent<Rigidbody2D>().velocity = new Vector2(ball.GetComponent<Rigidbody2D>().velocity.x, -Mathf.Abs(ball.GetComponent<Rigidbody2D>().velocity.y));
//         // }
//         if (ball.position.y < bottomBorder)
//         {
//             ball.position = new Vector3(ball.position.x, bottomBorder, ball.position.z);
//             ball.GetComponent<Rigidbody2D>().velocity = new Vector2(ball.GetComponent<Rigidbody2D>().velocity.x, Mathf.Abs(ball.GetComponent<Rigidbody2D>().velocity.y));
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public float borderSize = 0.1f;
    public Transform player;
    public Transform ball;

    void Update()
    {

        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + borderSize;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - borderSize;
        float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - borderSize;
        float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + borderSize;

        

        if (player.position.x < leftBorder)
        {
            player.position = new Vector3(leftBorder, player.position.y, player.position.z);
        }   
        else if (player.position.x > rightBorder)
        {
            player.position = new Vector3(rightBorder, player.position.y, player.position.z);
        }
        if (player.position.y < bottomBorder)
        {
            player.position = new Vector3(player.position.x, bottomBorder, player.position.z);
        }

        if (ball.position.x < leftBorder)
        {
            Vector2 normal = Vector2.right;
            Vector2 reflectedDirection = Vector2.Reflect(ball.GetComponent<Rigidbody2D>().velocity.normalized, normal);
            ball.GetComponent<Rigidbody2D>().velocity = reflectedDirection * ball.GetComponent<Rigidbody2D>().velocity.magnitude;
            ball.position = new Vector3(leftBorder, ball.position.y, ball.position.z);
        }   
        else if (ball.position.x > rightBorder)
        {
            Vector2 normal = Vector2.left;
            Vector2 reflectedDirection = Vector2.Reflect(ball.GetComponent<Rigidbody2D>().velocity.normalized, normal);
            ball.GetComponent<Rigidbody2D>().velocity = reflectedDirection * ball.GetComponent<Rigidbody2D>().velocity.magnitude;
            ball.position = new Vector3(rightBorder, ball.position.y, ball.position.z);
        }

        // if (ball.position.y > topBorder)
        // {
        //     Vector2 normal = Vector2.down;
        //     Vector2 reflectedDirection = Vector2.Reflect(ball.GetComponent<Rigidbody2D>().velocity.normalized, normal);
        //     ball.GetComponent<Rigidbody2D>().velocity = reflectedDirection * ball.GetComponent<Rigidbody2D>().velocity.magnitude;
        //     ball.position = new Vector3(ball.position.x, topBorder, ball.position.z);
        // }
        if (ball.position.y < bottomBorder)
        {
            Vector2 normal = Vector2.up;
            Vector2 reflectedDirection = Vector2.Reflect(ball.GetComponent<Rigidbody2D>().velocity.normalized, normal);
            ball.GetComponent<Rigidbody2D>().velocity = reflectedDirection * ball.GetComponent<Rigidbody2D>().velocity.magnitude;
            ball.position = new Vector3(ball.position.x, bottomBorder, ball.position.z);
        }
    }
}