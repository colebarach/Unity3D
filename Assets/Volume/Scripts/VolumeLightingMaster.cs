using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Lighting Volume Master

  Author -  Cole Barach
  Created - 2022.03.27
  Updated - 2022.03.28

  Function
    -Application of RenderSettings from VolumeLighting objects
    -Interpolation using Volume.GetWeights()
    -Instantiation of procedural skybox material 
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class VolumeLightingMaster : MonoBehaviour {
    public static VolumeLightingMaster master; //Singleton Behavior
    
    public Shader skyboxShader;
    Material skyboxMaterial;

    [HideInInspector]public VolumeLighting[] lightingVolumes;
    [HideInInspector]public Volume[]         volumes;
    [HideInInspector]public Transform        observer;

    void Awake() {
        if(master == null) {
            master = this;
        } else if(master != this) {
            Destroy(this); 
        }
    }
    void Start() {
        SetObserver();
        SetSkyboxMaterial();
    }
    void Update() {
        GetVolumes();
        GetLighting();
    }

    void SetObserver() {
        observer = Camera.main.transform;
    }
    void SetSkyboxMaterial() {
        skyboxMaterial = new Material(skyboxShader);
        RenderSettings.skybox = skyboxMaterial;
    }

    void GetVolumes() {
        lightingVolumes = FindObjectsOfType<VolumeLighting>();
        volumes = new Volume[lightingVolumes.Length];
        for(int x = 0; x < lightingVolumes.Length; x++) {
            volumes[x] = lightingVolumes[x].GetComponent<Volume>();
        }
    }
    void GetLighting() {
        float[] weights = Volume.GetWeights(volumes, observer.position);
        Color skyboxColor    = Color.clear;
        float skyboxExposure = 0;
        Color ambientLight   = Color.clear;
        Color fogColor       = Color.clear;
        float fogDensity     = 0;
        for(int x = 0; x < lightingVolumes.Length; x++) {
            VolumeLighting lighting = lightingVolumes[x];
            float weight = weights[x]; 
            skyboxColor    += lighting.skyboxColor    * weight;
            skyboxExposure += lighting.skyboxExposure * weight;
            ambientLight   += lighting.ambientLight   * weight;
            fogColor       += lighting.fogColor       * weight;
            fogDensity     += lighting.fogDensity     * weight;
        }
        skyboxMaterial.SetColor("_Tint",     skyboxColor);
        skyboxMaterial.SetFloat("_Exposure", skyboxExposure);
        RenderSettings.ambientLight = ambientLight;
        RenderSettings.fogColor     = fogColor;
        RenderSettings.fogDensity   = fogDensity;
    }
}