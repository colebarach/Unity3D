using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  	Progress Master

  	Author -  Cole Barach
  	Created - 2020.09.18
  	Updated - 2022.09.18

  	Function
    	- Management of global story progress
        - Identifies progress with progressStates
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class ProgressMaster : MonoBehaviour {
    public static ProgressMaster master; //Singleton Behavior
    
    public int progress;
    public string[] progressStates;

    void Awake() {
        if(master == null) {
            master = this;
        } else if(master != this) {
            Destroy(this); 
        }
    }

    public void ProgressIncrement() {
        progress++;
    }

    public bool CheckState(string state) {
        return progressStates[progress] == state;
    }
    public void SetState(string state) {
        for(int x = 0; x < progressStates.Length; x++) {
            if(progressStates[x] == state) progress = x;
        }
    }
    public bool CheckState(string[] states) {
        bool active = false;
        for(int x = 0; x < states.Length; x++) {
            if(CheckState(states[x])) active = true;
        }
        return active;
    }
}
