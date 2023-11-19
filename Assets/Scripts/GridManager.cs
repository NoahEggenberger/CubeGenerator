using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int GRID_WIDTH = 2;
    public int GRID_HEIGHT = 2;
    public int GRID_DEPTH = 2;
    public int CELL_SIZE = 1;
    public float SPACING = 0.1f;
    public float SPAWN_PROBABILITY = 0.8f;

    public GameObject[] cubes;

    private readonly List<Vector3Int> notAllowedSpawnPositions = new List<Vector3Int>();

    private GameObject GetRandomCube()
    {
        return cubes[Random.Range(0, cubes.Length)];
    }

    private void SetNotAllowed(int x, int z)
    {
        this.notAllowedSpawnPositions.Add(new Vector3Int(x, 0, z));
    }

    private bool IsAllowed(int x, int y, int z)
    {
        if (y > 0)
        {
            return !this.notAllowedSpawnPositions.Any(v => v.x == x && v.z == z);
        }

        return true;
    }

    private void CreateGrid()
    {
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int z = 0; z < GRID_DEPTH; z++)
                {
                    if (this.IsAllowed(x, y, z))
                    {
                        if (Random.value < SPAWN_PROBABILITY)
                        {
                            Vector3 spawnPosition = new Vector3(x * (CELL_SIZE + SPACING), y * (CELL_SIZE), z * (CELL_SIZE + SPACING));
                            Instantiate(this.GetRandomCube(), spawnPosition, Quaternion.identity);

                        }
                        else
                        {
                            if (y == 0)
                            {
                                this.SetNotAllowed(x, z);
                            }
                        }
                    }
                }
            }
        }
    }
    
    public void GenerateCubeScene()
    {
        this.CreateGrid();

        this.notAllowedSpawnPositions.Clear();
    }
}
