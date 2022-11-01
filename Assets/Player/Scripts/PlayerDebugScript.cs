using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Player Debug Script
	
	Author -  Cole Barach
	Created - 2021.12.03
	Updated - 2021.12.03
	
	Function
		-Printing of Player public variables

    Dependencies
        -PlayerScript
        -PlayerInteraction
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class PlayerDebugScript : MonoBehaviour {
    public bool printVelocity;
    public bool printGrounded;
    public bool printInteractee;
    public bool printInteracteeStay;

    PlayerScript player;
    PlayerInteraction interaction;

    void Start() {
        player = gameObject.GetComponent<PlayerScript>();
        interaction = gameObject.GetComponent<PlayerInteraction>();
    }

    void Update() {
        string output = "";
        if(printVelocity) output += "velocity: " + player.velocity + " - ";
        if(printGrounded) output += "isGrounded: " + player.grounded + " - ";
        if(printInteractee) output += "interacted: " + interaction.interactee + " - ";
        if(printInteracteeStay) output += "interactedStay: " + interaction.interacteeStay + " - ";
        if(output != "") {
            print(output);
        }
    }
}
