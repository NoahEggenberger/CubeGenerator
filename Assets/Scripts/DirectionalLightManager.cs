using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightManager : MonoBehaviour
{
    private static DirectionalLightManager _instance;
    public Light directionalLight;

    public static DirectionalLightManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DirectionalLightManager>();
                if (_instance == null)
                {
                    Debug.LogError("No DirectionalLightManager found in scene!");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy duplicate if it exists
            return;
        }

        _instance = this;
        directionalLight = GetComponent<Light>();
    }
}
