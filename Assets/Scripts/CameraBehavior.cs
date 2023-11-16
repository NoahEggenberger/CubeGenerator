using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public float circleRadius = 5.0f;


    void Start()
    {
        // Calculate a random angle for the initial position on the circle
        float randomAngle = Random.Range(0f, 360f);

        // Calculate the position on the circle based on the angle and radius
        Vector3 randomPosition = new Vector3(
            circleRadius * Mathf.Cos(Mathf.Deg2Rad * randomAngle),
            transform.position.y,
            circleRadius * Mathf.Sin(Mathf.Deg2Rad * randomAngle)
        );

        // Set the camera's position
        transform.position = randomPosition;

        transform.LookAt(new Vector3(0.5f, 0, 0.5f));

    }
}
