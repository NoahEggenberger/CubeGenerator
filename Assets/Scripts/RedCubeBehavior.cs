using UnityEngine;

public class RedCubeBehavior : MonoBehaviour, CubeBehavior
{
    public string COLOR_LABEL = "red";

    public string GetColorLabel()
    {
        return COLOR_LABEL;
    }
}
