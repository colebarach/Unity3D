using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBehaviorPushButton : MonoBehaviour {
    PlayerInteraction  interaction;
    BehaviorPushButton button;

    void Start() {
        interaction = GameObject.FindObjectOfType<PlayerInteraction>();
        button = GetComponent<BehaviorPushButton>();
    }

    void Update() {
        if(interaction.CheckInteractee(gameObject)) {
            button.Press();
        }
    }
}
