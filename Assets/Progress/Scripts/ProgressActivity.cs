using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressActivity : MonoBehaviour {
    public string[] activeStates;

    ProgressMaster progress;
    GameObject[] children;

    bool isEnabled;
    bool enabledPrime;

    void Start() {
        progress = GameObject.FindObjectOfType<ProgressMaster>();
        children = new GameObject[transform.childCount];
        for(int z = 0; z < children.Length; z++) {
            children[z] = transform.GetChild(z).gameObject;
            children[z].SetActive(false);
        }
        isEnabled = false;
        enabledPrime = false;
    }

    void Update() {
        isEnabled = false;
        for(int x = 0; x < activeStates.Length; x++) {
            if(progress.CheckState(activeStates[x])) {
                isEnabled = true;
                break;
            }
        }

        if(isEnabled != enabledPrime) {
            enabledPrime = isEnabled;
            for(int y = 0; y < children.Length; y++) {
                children[y].SetActive(isEnabled);
            }
        }
    }
}
