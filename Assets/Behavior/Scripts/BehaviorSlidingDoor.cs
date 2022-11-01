using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  	Behavior Door

  	Author -  Cole Barach
  	Created - 2020.05.28
  	Updated - 2022.09.17

  	Function
    	- Reads interactedObject variable of PlayerInteraction for changes
		- Calls Transform.Rotate on attached object to match targeted rotation
		
	Dependencies
	    - PlayerInteraction
        - GlobalFunctions
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class BehaviorSlidingDoor : MonoBehaviour {
	[Header("State")]
	public bool             open;
    public bool             locked;
	[Header("Rotation")]
	public float            translationalVelocity = 0.1f;
	public float            translationMagnitude  = 1;
	public float            translationEpsilon = 1;
    public Vector3          translationAxis = Vector3.up;
    [Header("Colliders")]
	public LayerMask        obstacleMask;
    public Vector3          positiveColliderOffset;
    public Vector3          negativeColliderOffset;

	[HideInInspector] public float   openPosition;
	[HideInInspector] public float   closedPosition;

    BoxCollider       boxCollider;

	void Start() {
        // References
        boxCollider = GetComponent<BoxCollider>();
        // Getting Rotation Targets
        if(open) {
			openPosition = GetPosition();
			closedPosition = openPosition - translationMagnitude;
		} else {
			closedPosition = GetPosition();
			openPosition = closedPosition + translationMagnitude;
		}
	}

	void Update() {
		Translate(GetTranslation());
	}
	float GetTranslation() {
        // Default closed
		float currentPosition = GetPosition();
		float targetPosition = closedPosition;
        // Check if open
		if(open) targetPosition = openPosition;
		
        // Check angle to travel
		float positionOffset = targetPosition-currentPosition;
		float translation = 0;
		
        // Check to see if door has reached epsilon
		if(Mathf.Abs(positionOffset) < translationEpsilon) {
			translation = 0;
		} else {
			translation = Mathf.Sign(positionOffset)*translationalVelocity*Time.deltaTime;
		}
		return translation;
	}
	float GetPosition() {
        return Vector3.Scale(transform.localPosition,translationAxis.normalized).magnitude;
    }
    public void Translate(float translation) {
        if(translation != 0) {
            Vector3 offset;
            if(translation > 0) {
                Vector3 colliderCenter = boxCollider.center + positiveColliderOffset;
                offset = colliderCenter.x*transform.right + colliderCenter.y*transform.up + colliderCenter.z*transform.forward;
            } else {
                Vector3 colliderCenter = boxCollider.center + negativeColliderOffset;
                offset = colliderCenter.x*transform.right + colliderCenter.y*transform.up + colliderCenter.z*transform.forward;
            }
            if(!Physics.CheckBox(transform.position+offset,boxCollider.size/2,transform.rotation,obstacleMask)) {
                transform.localPosition += translationAxis*translation;
			}
		}
	}
}