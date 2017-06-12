using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {
    [SerializeField]
    public Material nightSkyBox;
    [SerializeField]
    public Material daySkyBox;

    public void setDaySkyBox()
    {
        RenderSettings.skybox = daySkyBox;
    }

    public void setNightSkyBox()
    {
        RenderSettings.skybox = nightSkyBox;
    }
}
