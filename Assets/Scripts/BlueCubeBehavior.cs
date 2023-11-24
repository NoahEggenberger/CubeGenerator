using UnityEngine;

public class BlueCubeBehavior : MonoBehaviour, CubeBehavior
{
    public string COLOR_LABEL = "blue";

    public string GetColorLabel()
    {
        return COLOR_LABEL;
    }
}
