using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorPushButton : MonoBehaviour {
    public bool locked;
    public bool pressed;
    public bool toggle;

    public void Press() {
        if(toggle) {
            pressed = !pressed;
        } else {
            pressed = true;
        }
    }
}
