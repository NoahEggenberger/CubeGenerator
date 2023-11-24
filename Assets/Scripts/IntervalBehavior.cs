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
        
        var cameraBehavior = manager.GetComponent<CameraBehavior>();
        cameraBehavior.SetCameraPosition(StrategyEnum.One);

        var gridGenerator = manager.GetComponent<GridManager>();
        gridGenerator.GenerateCubeScene();

        var resultGenerator = manager.GetComponent<ResultGenerator>();
        resultGenerator.GenerateResultOutput(this.SESSION_ID, StrategyEnum.One, stage);
    }

    private IEnumerator ExecuteStrategyTwo(GameObject manager) {
        Debug.Log("RUNNING STRATEGY 2");

        var cameraBehavior = manager.GetComponent<CameraBehavior>();
        var gridGenerator = manager.GetComponent<GridManager>();
        var resultGenerator = manager.GetComponent<ResultGenerator>();

        string dateTime = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

        Debug.Log("GENERATE IMAGE 1");
        cameraBehavior.SetCameraPosition(StrategyEnum.Two, 0f);
        gridGenerator.GenerateCubeScene();
        resultGenerator.GenerateResultOutput(this.SESSION_ID, StrategyEnum.Two, stage, false, 1, dateTime);

        yield return new WaitForSeconds(2);

        Debug.Log("GENERATE IMAGE 2");
        cameraBehavior.SetCameraPosition(StrategyEnum.Two, 180f);
        resultGenerator.GenerateResultOutput(this.SESSION_ID, StrategyEnum.Two, stage, true, 2, dateTime);
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

        this.currentScene++;
    }
}
