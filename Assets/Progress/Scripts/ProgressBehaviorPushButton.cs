using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBehaviorPushButton : MonoBehaviour {
    public string[] activeStates;
    public string   targetState;
    [Header("Parameters")]
    public bool pressOnActive;
    public bool setPressedWhenActive;
    public bool pressed;
    public bool setLockWhenActive;
    public bool locked;

    bool active;
    bool activePrior;
    bool pressedPrior;
    ProgressMaster progress;
    BehaviorPushButton button;
    
    void Start() {
        progress = FindObjectOfType<ProgressMaster>();
        button = GetComponent<BehaviorPushButton>();
    }

    void Update() {
        active = progress.CheckState(activeStates);
        if(active) {
            if(setPressedWhenActive) button.pressed = pressed;
            if(setLockWhenActive)    button.locked  = locked;
            if(!activePrior) {
                activePrior = true;
                if(pressOnActive) button.Press();
            }
        } else {
            if(setPressedWhenActive) button.pressed = !pressed;
            if(setLockWhenActive)    button.locked  = !locked;
        }
        if(button.pressed && !pressedPrior) {
            pressedPrior = true;
            progress.SetState(targetState);
        }
    }
}
