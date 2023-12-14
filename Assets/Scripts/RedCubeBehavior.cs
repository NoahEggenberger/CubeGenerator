using UnityEngine;

public class RedCubeBehavior : MonoBehaviour, CubeBehavior
{
    public string COLOR_LABEL = "red";

    private readonly float[] redHueRange = { 0f, 10f };

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
                renderer.sharedMaterial.color = this.RandomColorInHueRange(this.redHueRange);
            }
            else
            {
                Debug.LogError("Renderer component not found.");
            }

            return cube;
    }
}
