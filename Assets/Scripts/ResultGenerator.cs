using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class SceneResult {
    public CubePositions positions;
    public string[] imagePaths;
}

[System.Serializable]
public class CubePositions
{
    public string one = "";
    public string two = "";
    public string three = "";
    public string four = "";
    public string five = "";
    public string six = "";
    public string seven = "";
    public string eight = "";
}

public class ResultGenerator : MonoBehaviour
{

    public bool IN_DEBUG_MODE = true;

    private string SESSION_ID;

    private int scene;

    private StageEnum currentStage;

    private StrategyEnum currentStrategy;

    private readonly List<GameObject> visibleCubes = new List<GameObject>();

    private string resultPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Temp", "Generated");

    private List<string> imagePaths = new List<string>();

    private List<SceneResult> sceneResults = new List<SceneResult>();

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

    private string SerializeToJson(List<SceneResult> results)
    {
        return JsonConvert.SerializeObject(results, Formatting.Indented);
    }

    private void GenerateResultLabels()
    {
        var cubePositions = new CubePositions();

        foreach (GameObject cube in this.visibleCubes)
        {
            var cubePosition = cube.transform.position;

            var cubeBehavior = cube.GetComponent<CubeBehavior>();

            if (cubePosition.x == 1.1f && cubePosition.y == 0f && cubePosition.z == 1.1f)
            {
                cubePositions.one = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 0f && cubePosition.z == 1.1f)
            {
                cubePositions.two = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 0f && cubePosition.z == 0f)
            {
                cubePositions.three = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 1.1f && cubePosition.y == 0f && cubePosition.z == 0f)
            {
                cubePositions.four = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 1.1f && cubePosition.y == 1f && cubePosition.z == 1.1f)
            {
                cubePositions.five = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 1f && cubePosition.z == 1.1f)
            {
                cubePositions.six = cubeBehavior.GetColorLabel();
            }
            else if (cubePosition.x == 0f && cubePosition.y == 1f && cubePosition.z == 0f)
            {
                cubePositions.seven = cubeBehavior.GetColorLabel();
            }
            else
            {
                cubePositions.eight = cubeBehavior.GetColorLabel();
            }
        }

        var fileName = "scene_results.json";

        var resultDir = Path.Combine(this.resultPath, $"Strategy_{this.currentStrategy.HumanName()}", this.SESSION_ID, this.currentStage.HumanName());

        var filePath = Path.Combine(resultDir, fileName);

        var result = new SceneResult
        {
            positions = cubePositions,
            imagePaths = imagePaths.ToArray()
        };

        sceneResults.Add(result);
        Debug.Log(sceneResults.Count);

        if (IN_DEBUG_MODE)
        {
            Debug.Log(result.ToString());
        }
        else
        {
            this.GenerateDirectory(resultDir);

            string json = this.SerializeToJson(sceneResults);
            
            File.WriteAllText(filePath, json);
        }
    }

    private void GenerateResultImages(int id = 0)
    {
        string fileName;
        if (id == 0) {
            fileName = $"Image_{this.scene}.jpg";
        } else {
            fileName = $"Image_{this.scene}_{id}.jpg";
        }

        var imageDir = Path.Combine(this.resultPath, $"Strategy_{this.currentStrategy.HumanName()}", this.SESSION_ID, this.currentStage.HumanName(), "Images");
        var filePath = Path.Combine(imageDir, fileName);

        imagePaths.Add(Path.Combine(this.currentStage.HumanName(), "Images", fileName));

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

    public void GenerateResultOutput(string sessionId, StrategyEnum strategy, StageEnum stage, int scene, bool generateLabels = true, int id = 0)
    {
        this.SESSION_ID = sessionId;

        this.scene = scene;

        this.currentStrategy = strategy;

        if (stage != this.currentStage) {
            this.currentStage = stage;
            this.sceneResults.Clear();
        }
        
        if (strategy == StrategyEnum.One) {
           this.GetVisibleCubes(); 
        } else if (strategy == StrategyEnum.Two) {
            this.visibleCubes.AddRange(GameObject.FindGameObjectsWithTag("Cube"));
        } else {
            return;
        }
        
        this.GenerateResultImages(id);

        if (generateLabels) {
            this.GenerateResultLabels();
            this.imagePaths.Clear();
        }

        this.visibleCubes.Clear();

    }
}
