using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAudioMaster : MonoBehaviour {
    public static VolumeAudioMaster master; //Singleton Behavior
    void Awake() {
        if(master == null) {
            master = this;
        } else if(master != this) {
            Destroy(this); 
        }
    }

    public VolumeAudio[] audios;
    public Volume[] volumes;
    public Transform observer;

    void Start() {
        GetVolumes();
    }

    void GetVolumes() {
        audios = FindObjectsOfType<VolumeAudio>();
        volumes = new Volume[audios.Length];
        for(int x = 0; x < audios.Length; x++) {
            volumes[x] = audios[x].volume;
        }
    }
    void GetAudio() {
        float[] weights = Volume.GetWeights(volumes, observer.position);
    }
}
