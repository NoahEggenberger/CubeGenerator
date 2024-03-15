using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    public void SetCameraPosition(StrategyEnum strategy, float y, float circleRadius, float degrees = 0f)
    {
        Camera mainCamera = Camera.main;
        float angle = 0f;

        if (strategy == StrategyEnum.One) {
            // Calculate a random angle for the initial position on the circle
            angle = Random.Range(0f, 360f);
        } else if (strategy == StrategyEnum.Two) {
            angle = degrees; 
        } else {
            return;
        }

        // Calculate the position on the circle based on the angle and radius
        Vector3 position = new Vector3(
            circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle) + 0.5f,
            y,
            circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle) + 0.5f
        );

        // Set the camera's position
        mainCamera.transform.position = position;

        mainCamera.transform.LookAt(new Vector3(0.5f, -1.5f, 0.5f));
    }
}
