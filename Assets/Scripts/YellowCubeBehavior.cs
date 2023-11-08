using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowCubeBehavior : MonoBehaviour, CubeBehavior
{
    public string COLOR_LABEL = "yellow";

    public string GetColorLabel()
    {
        return COLOR_LABEL;
    }
}
