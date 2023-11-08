using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CubePositions
{
    public string one;
    public string two;
    public string three;
    public string four;
    public string five;
    public string six;
    public string seven;
    public string eight;
}

public class VisibilityChecker : MonoBehaviour
{
    private readonly List<GameObject> cubes = new List<GameObject>();

    private readonly List<GameObject> visibleCubes = new List<GameObject>();
    
    private void CreateResult()
    {
        CubePositions cubePositions = new CubePositions();

        foreach (GameObject cube in this.visibleCubes)
        {

            Vector3 cubePosition = cube.transform.position;

            CubeBehavior cubeBehavior = cube.GetComponent<CubeBehavior>();

            if (cubePosition.x == 1.1f && cubePosition.y == 0f && cubePosition.z == 0f)
            {
                cubePositions.one = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 1.1f && cubePosition.y == 0f && cubePosition.z == 1.1f)
            {
                cubePositions.two = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 0f && cubePosition.z == 1.1f)
            {
                cubePositions.three = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 0f && cubePosition.z == 0f)
            {
                cubePositions.four = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 1.1f && cubePosition.y == 1f && cubePosition.z == 0f)
            {
                cubePositions.five = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 1.1f && cubePosition.y == 1f && cubePosition.z == 1.1f)
            {
                cubePositions.six = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 1f && cubePosition.z == 1.1f)
            {
                cubePositions.seven = cubeBehavior.GetColorLabel();
            }
            else
            {
                cubePositions.eight = cubeBehavior.GetColorLabel();
            }
        }

        Debug.Log("1: " + cubePositions.one);
        Debug.Log("2: " + cubePositions.two);
        Debug.Log("3: " + cubePositions.three);
        Debug.Log("4: " + cubePositions.four);
        Debug.Log("5: " + cubePositions.five);
        Debug.Log("6: " + cubePositions.six);
        Debug.Log("7: " + cubePositions.seven);
        Debug.Log("8: " + cubePositions.eight);

    }

    private void GetVisible()
    {

        // Get all active GameObjects containing the tag "Cube"
        this.cubes.AddRange(GameObject.FindGameObjectsWithTag("Cube"));

        Camera mainCamera = Camera.main;

        foreach (GameObject cube in cubes)
        {
            // Get center of the cube
            Vector3 cubePosition = cube.transform.position;

            // Get Position of the camera
            Vector3 cameraPosition = mainCamera.transform.position;

            // Calculate the direction from the camera to the cube center
            Vector3 direction = cubePosition - cameraPosition;

            // Calcualte the distance from the camerea to the cube center
            float distance = Vector3.Distance(cameraPosition, cubePosition);


            Debug.DrawLine(cameraPosition, cubePosition, Color.red, 60f);

            // The ray should only detect collisions with cubes
            int layerMask = 1 << LayerMask.NameToLayer("Cubes");

            // Get all the collisions of the RayCast
            RaycastHit[] hits = Physics.RaycastAll(cameraPosition, direction, distance, layerMask);


            List<RaycastHit> tempHits = new List<RaycastHit>();

            // Remove collisions with the current cube itself
            foreach(var hit in hits)
            {
                if (!(hit.collider.Equals(cube.GetComponent<Collider>())))
                {
                    tempHits.Add(hit);
                }
            }

            if (tempHits.Count == 0)
            {
                this.visibleCubes.Add(cube);
            }
        }

        this.CreateResult();

        
    }



    void Start()
    {
        this.GetVisible();
    }
}
