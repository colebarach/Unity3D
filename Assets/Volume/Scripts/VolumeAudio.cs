using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAudio : MonoBehaviour {
    public Volume volume;
    public AudioSource source;
    public Vector2 volumeDomain;

    void Start() {
        // source = GetComponent<AudioSource>();
        // if(source == null) {
        //     source = gameObject.AddComponent<AudioSource>();
        // }
    }
}
