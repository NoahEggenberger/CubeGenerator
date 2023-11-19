using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntervalBehavior : MonoBehaviour
{

    public int ROUNDS = 10;
    private readonly string SESSION_ID = Guid.NewGuid().ToString();
    private int currentRound = 0;

    private void ClearScene()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");

        foreach (GameObject cube in cubes)
        {
            cube.SetActive(false);
            Destroy(cube);
        }
    }
    
    private void FixedUpdate()
    {
        Debug.Log("NEXT SCENE: ");

        this.currentRound++;

        var manager = GameObject.Find("Manager");

        this.ClearScene();

        if (manager != null)
        {
            var cameraBehavior = manager.GetComponent<CameraBehavior>();
            cameraBehavior.SetCameraPosition();

            var gridGenerator = manager.GetComponent<GridManager>();
            gridGenerator.GenerateCubeScene();

            var resultGenerator = manager.GetComponent<ResultGenerator>();
            resultGenerator.GenerateResultOutput(this.SESSION_ID);
        }

        if (this.currentRound >= this.ROUNDS)
        {
            // ToDo: This doesn't stop the unity editor
            Application.Quit();
        }
    }
}
