using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  	Behavior Door

  	Author -  Cole Barach
  	Created - 2020.05.28
  	Updated - 2022.12.26

  	Function
		- Calls Transform.Rotate on attached object to match targeted rotation
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class BehaviorDoor : MonoBehaviour {
	[Header("State")]
	public bool             open;
    public bool             locked;
	[Header("Rotation")]
	public float            angularVelocity = 90;
	public float            sweepAngle      = 90;
	public float            rotationEpsilon = 1;
    public Vector3          hingeAxis = Vector3.up;
    [Header("Colliders")]
	public LayerMask        obstacleMask;
    public Vector3          positiveColliderOffset;
    public Vector3          negativeColliderOffset;

	[HideInInspector] public float   openRotation;
	[HideInInspector] public float   closedRotation;

    BoxCollider       boxCollider;

	void Start() {
        // References
        boxCollider = GetComponent<BoxCollider>();
        // Getting Rotation Targets
        if(open) {
			openRotation = GetAngle();
			closedRotation = openRotation - sweepAngle;
		} else {
			closedRotation = GetAngle();
			openRotation = closedRotation + sweepAngle;
		}
	}

	void Update() {
		Rotate(GetTorque());
	}
	float GetTorque() {
        // Default closed
		float currentRotation = EulerTransformPole(GetAngle(), closedRotation);
		float targetRotation = closedRotation;
        // Check if open
		if(open) targetRotation = openRotation;
		
        // Check angle to travel
		float rotationOffset = targetRotation-currentRotation;
	
        // Check to see if door has reached epsilon
		if(Mathf.Abs(rotationOffset) < rotationEpsilon) {
			return 0;
		} else {
			return Mathf.Sign(rotationOffset)*angularVelocity*Time.deltaTime;
		}
	}
	float GetAngle() {
        return Vector3.Scale(transform.localEulerAngles,hingeAxis.normalized).magnitude;
    }
    public void Rotate(float torque) {
        if(torque == 0) return;
        Vector3 offset;
        if(torque > 0) {
            Vector3 colliderCenter = boxCollider.center + positiveColliderOffset;
            offset = colliderCenter.x*transform.right + colliderCenter.y*transform.up + colliderCenter.z*transform.forward;
        } else {
            Vector3 colliderCenter = boxCollider.center + negativeColliderOffset;
            offset = colliderCenter.x*transform.right + colliderCenter.y*transform.up + colliderCenter.z*transform.forward;
        }
        if(!Physics.CheckBox(transform.position+offset,boxCollider.size/2,transform.rotation,obstacleMask)) {
            transform.localEulerAngles += hingeAxis*torque;
        }
	}
    
    static float EulerUnsignedToSigned(float theta) {
        if(theta > 180) theta -= 360;
        return theta;
    }

    static float EulerTransformPole(float theta, float pole) {
        if(theta > pole+180) theta -= 360;
        if(theta < pole-180) theta += 360;
        return theta;
    }
}