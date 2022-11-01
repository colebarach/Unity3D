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

public class InteractionBehaviorSlidingDoor : MonoBehaviour {
    PlayerInteraction interaction;
    BehaviorSlidingDoor doorScript;

    void Start() {
        interaction = GameObject.FindObjectOfType<PlayerInteraction>();
        if(doorScript == null) doorScript = GetComponent<BehaviorSlidingDoor>();
    }

    void Update() {
        if(interaction.CheckInteractee(gameObject)) doorScript.open = !doorScript.open;
    }
}
