using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  	Behavior Elevator

  	Author -  Cole Barach
  	Created - 2021.12.22
  	Updated - 2022.02.05

  	Function
    	- To translate parent gameObject and move the Player Object when in bounds

    Dependencies
        - Volume
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class BehaviorElevator : MonoBehaviour {
    public float speed;
    public float lowPosition;
    public float highPosition;
    public float positionEpsilon;
    public bool  active;
    public float playerGrounding = 0.05f;

    PlayerScript player; 
    Volume volume;
    PlayerInteraction interaction;

    void Start() {
        player = GameObject.FindObjectOfType<PlayerScript>();
        interaction = GameObject.FindObjectOfType<PlayerInteraction>();
        volume = GetComponent<Volume>();
    }

    void Update() {
        bool playerRiding = (volume.GetWeight(player.transform.position) > 0.95);

        // Get Target
        float targetPosition = lowPosition;
        if(active) targetPosition = highPosition;
        
        // Move
        if(transform.position.y < targetPosition - positionEpsilon) {
            float delta = speed * Time.deltaTime;
            transform.position += new Vector3(0, delta, 0);
            if(playerRiding) {
                if(delta < 0) {
                    player.MoveInterrupt(0, delta - playerGrounding * Time.deltaTime, 0);
                } else {
                    player.MoveInterrupt(0, delta, 0);
                }
                player.ForceStop(0, 64, 0, 3); // ID 3 for elevator
            }
        }
        if(transform.position.y > targetPosition + positionEpsilon) {
            float delta = -speed * Time.deltaTime;
            transform.position += new Vector3(0, delta, 0);
            if(playerRiding) {
                if(delta < 0) {
                    player.MoveInterrupt(0, delta - playerGrounding * Time.deltaTime, 0);
                } else {
                    player.MoveInterrupt(0, delta, 0);
                }
                player.ForceStop(0, 64, 0, 3); // ID 3 for elevator
            }
        }
        if(!playerRiding) {
            player.ForceStop(0, 0, 0, 3); // ID 3 for elevator
        }
    }
}