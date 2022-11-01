using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
	Player Interaction
	
	Author -  Cole Barach
	Created - 2022.09.18
	Updated - 2021.09.22
	
	Function
        -Interface player interaction with door behavior
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class InteractionBehaviorDoor : MonoBehaviour {
    PlayerInteraction interaction;
    BehaviorDoor doorScript;

    void Start() {
        interaction = GameObject.FindObjectOfType<PlayerInteraction>();
        doorScript = GetComponent<BehaviorDoor>();
    }

    void Update() {
        if(interaction.CheckInteractee(gameObject)) {
            if(doorScript.open || !doorScript.locked) {
                doorScript.open = !doorScript.open;
            }
        }
    }
}
