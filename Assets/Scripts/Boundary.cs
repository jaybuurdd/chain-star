using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public float borderSize = 1f;
    public Transform player;

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

        if (player.position.y > topBorder)
        {
            player.position = new Vector3(player.position.x, topBorder, player.position.z);
        }
        else if (player.position.y < bottomBorder)
        {
            player.position = new Vector3(player.position.x, bottomBorder, player.position.z);
        }
    }
}


