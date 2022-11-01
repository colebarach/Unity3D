using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Volume Class

  Author -  Cole Barach
  Created - 2022.03.27
  Updated - 2022.03.27

  Function
    -Generic component for uniting volume behaviors, post-process, lighting, fog, audio, etc.
    -Public functions for volume masters
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class Volume : MonoBehaviour {
    public bool isGlobal       = false;
    public float blendDistance = 0;
    [Range(0.0f, 1.0f)]
    public float weight        = 1;

    public float GetWeight(Vector3 position, Volume[] volumes) {
        return GetWeight(this,position,volumes);
    }
    public float GetWeight(Vector3 position) {
        return GetWeight(this,position,null);
    }
    public float GetDistance(Vector3 position) {
        return GetDistance(this,position);
    }

    public static float GetWeight(Volume volume, Vector3 position, Volume[] volumes) {
        if(volume.blendDistance == 0) volume.blendDistance = 0.0001f;
        if(volume.isGlobal) {
            float cumulativeWeight = 0;
            for(int x = 0; x < volumes.Length; x++) {
                if(!volumes[x].isGlobal) cumulativeWeight += volumes[x].GetWeight(position);
            }
            return Mathf.Clamp01(1-cumulativeWeight)*volume.weight;
        } else {
            float distance = GetDistance(volume, position);
            return (1-Mathf.Clamp01(distance/volume.blendDistance))*volume.weight;
        }
    }
    public static float GetDistance(Volume volume, Vector3 position) {
        Collider[] colliders = volume.GetComponents<Collider>();
        float minDistance = float.PositiveInfinity;
        foreach(Collider collider in colliders) {
            float distance = Vector3.Distance(position, collider.ClosestPoint(position));
            minDistance = Mathf.Min(minDistance, distance);
        }
        return minDistance;
    }
    public static float[] GetWeights(Volume[] volumes, Vector3 position) {
        float[] weights = new float[volumes.Length];
        for(int x = 0; x < volumes.Length; x++) {
            weights[x] = volumes[x].GetWeight(position,volumes);
        }
        return weights;
    }
}