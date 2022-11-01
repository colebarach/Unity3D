using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Lighting Volume

  Author -  Cole Barach
  Created - 2022.03.27
  Updated - 2022.03.28

  Function
    -Container for RenderSettings variables
    -Use VolumeLightingMaster for application
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class VolumeLighting : MonoBehaviour {
    [Header("Skybox")]
    public Color skyboxColor         = Color.white;
    public float skyboxExposure      = 1;
    [Header("Environment Lighting")]
    public Color ambientLight        = Color.gray;
    [Header("Fog")]
    public Color fogColor            = Color.black;
    public float fogDensity          = 0;
    
    void Awake() {
        if(GetComponent<Volume>() == null) Debug.LogError("NullReferenceExpection: Volume.cs component missing from object");
    }
}