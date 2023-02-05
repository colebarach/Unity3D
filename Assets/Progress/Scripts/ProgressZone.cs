using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressZone : MonoBehaviour {
    public string targetState;
    
    ProgressMaster progress;
    PlayerScript   player;

    void Start() {
        progress = GameObject.FindObjectOfType<ProgressMaster>();
        player   = GameObject.FindObjectOfType<PlayerScript>();
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject == player.gameObject) {
            progress.SetState(targetState);
            this.enabled = false;
        }
    }
}
