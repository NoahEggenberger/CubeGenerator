using System.Collections;
using System.Collections.Generic;
using System.IO;
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

public class ResultGenerator : MonoBehaviour
{

    public bool IN_DEBUG_MODE = true;

    public string SESSION_ID;

    private readonly List<GameObject> visibleCubes = new List<GameObject>();

    private string resultPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Temp", "Generated");

    private void GetVisibleCubes()
    {

        var cubes = new List<GameObject>();

        // Get all active GameObjects containing the tag "Cube"
        cubes.AddRange(GameObject.FindGameObjectsWithTag("Cube"));

        var mainCamera = Camera.main;

        foreach (GameObject cube in cubes)
        {
            // Get center of the cube
            var cubePosition = cube.transform.position;

            // Get Position of the camera
            var cameraPosition = mainCamera.transform.position;

            // Calculate the direction from the camera to the cube center
            var direction = cubePosition - cameraPosition;

            // Calcualte the distance from the camerea to the cube center
            var distance = Vector3.Distance(cameraPosition, cubePosition);

            if (IN_DEBUG_MODE)
            {
                Debug.DrawLine(cameraPosition, cubePosition, Color.red, 60f);
            }
            
            // The ray should only detect collisions with cubes
            var layerMask = 1 << LayerMask.NameToLayer("Cubes");

            // Get all the collisions of the RayCast
            var hits = Physics.RaycastAll(cameraPosition, direction, distance, layerMask);

            var tempHits = new List<RaycastHit>();

            foreach(var hit in hits)
            {
                var position = hit.collider.gameObject.transform.position;

                // Remove collisions with the current cube itself and cubes above the current cube
                if (!(hit.collider.Equals(cube.GetComponent<Collider>())) && !(position.x == cubePosition.x && position.y != cubePosition.y && position.z == cubePosition.z))
                {
                    tempHits.Add(hit);
                }
            }

            // Only cubes with no remaining collision are seen as visible
            if (tempHits.Count == 0)
            {
                this.visibleCubes.Add(cube);
            }
        }
    }

    private void GenerateResultLabels()
    {
        var cubePositions = new CubePositions();

        foreach (GameObject cube in this.visibleCubes)
        {
            var cubePosition = cube.transform.position;

            var cubeBehavior = cube.GetComponent<CubeBehavior>();

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

        var fileName = "label_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
        var labelDir = Path.Combine(this.resultPath, this.SESSION_ID, "Labels");

        var filePath = Path.Combine(labelDir, fileName);

        if (IN_DEBUG_MODE)
        {
            Debug.Log("1: " + cubePositions.one);
            Debug.Log("2: " + cubePositions.two);
            Debug.Log("3: " + cubePositions.three);
            Debug.Log("4: " + cubePositions.four);
            Debug.Log("5: " + cubePositions.five);
            Debug.Log("6: " + cubePositions.six);
            Debug.Log("7: " + cubePositions.seven);
            Debug.Log("8: " + cubePositions.eight);
            Debug.Log(filePath);
        }
        else
        {
            this.GenerateDirectory(labelDir);

            string json = JsonUtility.ToJson(cubePositions);
            File.WriteAllText(filePath, json);
        }
    }

    private void GenerateResultImages()
    {
        var fileName = "Image_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";
        var imageDir = Path.Combine(this.resultPath, this.SESSION_ID, "Images");
        var filePath = Path.Combine(imageDir, fileName);

        if (IN_DEBUG_MODE)
        {
            Debug.Log(filePath);
        }
        else
        {
            this.GenerateDirectory(imageDir);
            ScreenCapture.CaptureScreenshot(filePath);
        }
    }

    private void GenerateDirectory(string path)
    {
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
    }

    public void GenerateResultOutput(string sessionId)
    {
        this.SESSION_ID = sessionId;

        this.GetVisibleCubes();
        this.GenerateResultLabels();
        this.GenerateResultImages();

        this.visibleCubes.Clear();
    }
}