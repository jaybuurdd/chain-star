using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform ballTransform;
    public Transform playerTransfrom;
    public Camera mainCamera;
    public float arrowOffset = 0.5f;
    public float borderSize = 0.1f;

    private void Start()
    {
        // Set the Arrow's position to be above the top of the screen
        Vector3 arrowPos = new Vector3(0f, mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + arrowOffset, 0f);
        transform.position = arrowPos;
    }

    private void Update()
    {
        // Check if the ball is above the top of the screen
        if (ballTransform.position.y > mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)
        {
            // Calculate the direction from the Arrow to the Ball
            Vector2 direction = (ballTransform.position - transform.position).normalized;

            // Calculate the rotation angle for the Arrow based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Set the Arrow's rotation to point towards the Ball
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Set the Arrow's position to be right below the top border of the Camera view
            float topBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
            Vector3 arrowPos = new Vector3(ballTransform.position.x, topBorder - arrowOffset, 0f);
            transform.position = arrowPos;
        }
        else
        {
            // Set the Arrow's position to be above the top of the screen
            Vector3 arrowPos = new Vector3(0f, mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + arrowOffset, 0f);
            transform.position = arrowPos;
        }
    }
}
