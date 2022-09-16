using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
	Player Interaction Script
	
	Author -  Cole Barach
	Created - 2018.03.18
	Updated - 2021.09.18
	
	Function
		-Calls Physics.Raycast() upon Interact()
		-Store public references to interacted objects
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class PlayerInteractionScript : MonoBehaviour {
	[Header("Interaction")]
	public float maxDistance;
	
	[HideInInspector]public GameObject interactee;
	[HideInInspector]public GameObject interacteeStay;
	
	Camera mainCamera;

	void Start() {
		mainCamera = Camera.main;
	}
	void Update() {
			if(Input.GetMouseButtonDown(0)) interactee = GetInteractee();
			if(Input.GetMouseButton(0)) interacteeStay = GetInteractee();
	}
	public GameObject GetInteractee() {
			Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
			RaycastHit rayHit;
			GameObject hitObject = null;
			if(Physics.Raycast(ray, out rayHit, maxDistance)) hitObject = rayHit.collider.gameObject;
			return hitObject;
	}
	public bool CheckInteractee(GameObject subject) {
		bool interacted = interactee == subject;
		if(interacted) interactee = null;
		return interacted;
	}
}
