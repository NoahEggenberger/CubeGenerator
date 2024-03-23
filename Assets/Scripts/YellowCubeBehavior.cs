using UnityEngine;

public class YellowCubeBehavior : MonoBehaviour, CubeBehavior
{
    private readonly string COLOR_LABEL = "yellow";

    public int position;

    private readonly float[] yellowHueRange = { 40f, 80f };

    private Color RandomColorInHueRange(float[] hueRange)
    {
        float randomHue = Random.Range(hueRange[0], hueRange[1]) / 360f; 
        
        return Color.HSVToRGB(randomHue, 1f, 1f);
    }

    public string GetColorLabel()
    {
        return COLOR_LABEL;
    }

    public GameObject SetToRandomColorRange(GameObject cube) {
        if (cube.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.sharedMaterial.color = this.RandomColorInHueRange(this.yellowHueRange);
        }
        else
        {
            Debug.LogError("Renderer component not found.");
        }

        return cube;
    }

    public void SetCubePosition(int position)
    {
        this.position = position;
    }

    public int GetCubePosition()
    {
        return this.position;
    }
}
