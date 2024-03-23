
using UnityEngine;

public interface CubeBehavior
{
    public string GetColorLabel();

    public GameObject SetToRandomColorRange(GameObject cube);

    public void SetCubePosition(int position);

    public int GetCubePosition(); 
}
