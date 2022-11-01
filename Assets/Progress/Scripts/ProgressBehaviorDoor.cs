using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBehaviorDoor : MonoBehaviour {
    public string[] activeStates;
    [Header("Parameters")]
    public bool openOnActive;
    public bool setOpenWhenActive;
    public bool open;
    public bool setLockWhenActive;
    public bool locked;

    bool active;
    bool activePrior;
    ProgressMaster progress;
    BehaviorDoor door;

    void Start() {
        progress = FindObjectOfType<ProgressMaster>();
        door = GetComponent<BehaviorDoor>();
    }

    void Update() {
        active = progress.CheckState(activeStates);
        if(active) {
            if(setOpenWhenActive) door.open = open;
            if(setLockWhenActive) door.locked = locked;
            if(!activePrior) {
                activePrior = true;
                if(openOnActive) door.open = true;
            }
        } else {
            if(setOpenWhenActive) door.open = !open;
            if(setLockWhenActive) door.locked = !locked;
        }
    }
}
