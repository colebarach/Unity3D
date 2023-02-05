using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorElevatorButton : MonoBehaviour {
    public bool elevatorActiveState;
    public BehaviorElevator elevator;
    
    BehaviorPushButton button;

    void Start() {
        button   = GetComponent<BehaviorPushButton>();
    }

    void Update() {
        if(button.pressed) {
            elevator.active = elevatorActiveState;
        } else {
            elevator.active = !elevatorActiveState;
        }
    }
}
