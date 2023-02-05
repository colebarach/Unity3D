using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDishScript : MonoBehaviour {
    public Vector3 targetEuler;

    public float state;
    public float repairTime;

    Quaternion startingRotation;
    Quaternion targetRotation;

    PlayerInteraction interaction;
    ProgressMaster    progress;

    public string requiredState;
    public string targetState;

    void Start() {
        interaction = GameObject.FindObjectOfType<PlayerInteraction>();
        progress    = GameObject.FindObjectOfType<ProgressMaster>();

        startingRotation = transform.rotation;
        targetRotation = Quaternion.Euler(targetEuler + transform.parent.eulerAngles);
    }

    void Update() {
        if(state < 1) {
            if(interaction.CheckInteracteeStay(gameObject)) {
                state += Time.deltaTime / repairTime;
            }
            transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, state);
        } else {
            if(progress.CheckState(requiredState)) {
                progress.SetState(targetState);
            }
        }
    }
}