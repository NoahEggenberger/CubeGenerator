using UnityEngine;

public class BlueCubeBehavior : MonoBehaviour, CubeBehavior
{
    private readonly string COLOR_LABEL = "blue";

    private readonly float[] blueHueRange = { 220f, 240f };


    public string GetColorLabel()
    {
        return COLOR_LABEL;
    }

    private Color RandomColorInHueRange(float[] hueRange)
    {
        float randomHue = Random.Range(hueRange[0], hueRange[1]) / 360f; 
        
        return Color.HSVToRGB(randomHue, 1f, 1f);
    }

    public GameObject SetToRandomColorRange(GameObject cube) {
            if (cube.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.sharedMaterial.color = this.RandomColorInHueRange(this.blueHueRange);
            }
            else
            {
                Debug.LogError("Renderer component not found.");
            }
            return cube;
    }
}
