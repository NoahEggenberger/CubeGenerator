using UnityEngine;
using System;
using System.Collections;

public enum StrategyEnum {
    One,
    Two
}

public enum StageEnum {
    Train,
    Verify,
    Test
}

public class IntervalBehavior : MonoBehaviour
{
    public int TRAINING = 70;
    public int VERIFICATION = 20;
    public int TESTING = 10;

    public StrategyEnum STRATEGY = StrategyEnum.One;
    private readonly string SESSION_ID = Guid.NewGuid().ToString();
    private int currentScene = 0;
    private StageEnum stage = StageEnum.Test;

    private void ClearScene()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");

        foreach (GameObject cube in cubes)
        {
            cube.SetActive(false);
            Destroy(cube);
        }
    }

    private void ExecuteStrategyOne(GameObject manager) {
        Debug.Log("RUNNING STRATEGY 1");

        var cameraYPosition = UnityEngine.Random.Range(2f, 6f);
        var radius = UnityEngine.Random.Range(5f, 7f);
        
        var cameraBehavior = manager.GetComponent<CameraBehavior>();
        cameraBehavior.SetCameraPosition(StrategyEnum.One, cameraYPosition, radius);

        var gridGenerator = manager.GetComponent<GridManager>();
        gridGenerator.GenerateCubeScene();

        var resultGenerator = manager.GetComponent<ResultGenerator>();
        resultGenerator.GenerateResultOutput(this.SESSION_ID, StrategyEnum.One, stage, this.currentScene);
    }

    private IEnumerator ExecuteStrategyTwo(GameObject manager) {
        Debug.Log("RUNNING STRATEGY 2");

        var cameraBehavior = manager.GetComponent<CameraBehavior>();
        var gridGenerator = manager.GetComponent<GridManager>();
        var resultGenerator = manager.GetComponent<ResultGenerator>();



        Debug.Log("GENERATE IMAGE 1");
        var angle = UnityEngine.Random.Range(0f, 360f);
        // var cameraYPosition = UnityEngine.Random.Range(1f, 5f);
        var cameraYPosition = 4f;
        // var radius = UnityEngine.Random.Range(5f, 7f);
        var radius = 6f;
        
        cameraBehavior.SetCameraPosition(StrategyEnum.Two, cameraYPosition, radius, angle);
        gridGenerator.GenerateCubeScene();
        resultGenerator.GenerateResultOutput(this.SESSION_ID, StrategyEnum.Two, stage, this.currentScene, false, 1);

        yield return new WaitForSeconds(0.25f);



        Debug.Log("GENERATE IMAGE 2");
        var secondAngle = angle + 180f;
        cameraBehavior.SetCameraPosition(StrategyEnum.Two, cameraYPosition, radius, secondAngle);
        resultGenerator.GenerateResultOutput(this.SESSION_ID, StrategyEnum.Two, stage, this.currentScene, true, 2);
    }

    private void setStage() {
        if (this.currentScene <= this.TRAINING) {
            this.stage = StageEnum.Train;
        } else if (this.currentScene <= (this.TRAINING + this.VERIFICATION)) {
            this.stage = StageEnum.Verify;
        } else {
            this.stage = StageEnum.Test;
        }
    }
    
    private void FixedUpdate()
    {

        this.currentScene++;

        if (this.currentScene > (this.TRAINING + this.VERIFICATION + this.TESTING))
        {
            // ToDo: This doesn't stop the unity editor
            Application.Quit();
            Debug.Log("DATA GATHERING IS FINISHED!");
            return;
        }

        var manager = GameObject.Find("Manager");
        this.ClearScene();

        if (manager == null)
        {
            Debug.LogError("MANAGER WAS NOT FOUND!");
            Application.Quit();
            return;
        }

        this.setStage();

        Debug.Log($"---------- GENERATE SCENE: {this.currentScene} - STRATEGY: {this.STRATEGY} - STAGE: {this.stage} -----------");

        if (this.STRATEGY == StrategyEnum.One) {
            this.ExecuteStrategyOne(manager);
        } else if (this.STRATEGY == StrategyEnum.Two) {
            StartCoroutine(this.ExecuteStrategyTwo(manager));
        } else {
            Debug.LogError("INVALID STRATEGY: " + this.STRATEGY);
        }

    }
}
