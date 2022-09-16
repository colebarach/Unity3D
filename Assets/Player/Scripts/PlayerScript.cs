using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  First-Person Player Script
	
	Author -  Cole Barach
	Created - 2018.03.18
	Updated - 2021.12.04
	
	Function
		-Movement of CharacterController component
		-Rotation of MainCamera child
		-Rotation of Player gameObject
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class PlayerScript : MonoBehaviour {
	[Header("Movement")]
	public float walkSpeed =            3.00f;
	public float runSpeed =             4.00f;
	public float jumpHeight =           0.40f;
	[Header("Camera")]
	public float cameraSensitivity =    3.00f;
	[Header("Physics")]
	public float gravity =             -9.81f;
	public LayerMask playerMask;
	[Header("References")]
	public Camera mainCamera;
	public SphereCollider collisionDetector;
	
	[HideInInspector]public CharacterController playerController;
	[HideInInspector]public Vector3 velocity;
    [HideInInspector]public Vector3 velocityAdditive;
	[HideInInspector]public bool grounded;
	[HideInInspector]public bool mouseLock = true;

	void Start() {
		playerController = GetComponent<CharacterController>();
		CheckMouse();
	}
	void Update() {
		CheckKeys();
		CalculateGravity();
		MoveController();
		RotateCamera();
	}

	public void CalculateGravity() {
		grounded = Physics.CheckSphere(collisionDetector.transform.position, collisionDetector.radius, ~playerMask, QueryTriggerInteraction.Ignore);
		if(Input.GetButtonDown("Jump") && grounded) velocity.y = Mathf.Sqrt(-2*jumpHeight*gravity);
		if(grounded && velocity.y <= 0) {
			velocity.y = -0.1f;
		} else {
			velocity.y += gravity*Time.deltaTime;
		}
	}
	public void MoveController() {
		Vector3 velocityRaw = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Vector3.ClampMagnitude(velocityRaw, 1);
		if(Input.GetKey(KeyCode.LeftShift) && velocityRaw.z > 0) {
			velocityRaw.z *= runSpeed;
		} else {
			velocityRaw.z *= walkSpeed;
		}
		velocityRaw.x *= walkSpeed;
		velocity = velocityRaw.x*transform.right + velocity.y*transform.up + velocityRaw.z*transform.forward;
		if(velocityAdditive != Vector3.zero) {
            velocity.y = 0;
        }
        playerController.Move(velocity*Time.deltaTime + velocityAdditive);
        velocityAdditive = Vector3.zero;
	}
	public void RotateCamera() {
		if(mouseLock) {
            Vector2 angularVelocity = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))*cameraSensitivity;
            float eulerX = EulerUnsignedToSigned(mainCamera.transform.eulerAngles.x) - angularVelocity.y;
            eulerX = Mathf.Clamp(eulerX,-90,90);
            mainCamera.transform.localRotation = Quaternion.Euler(eulerX, 0f, 0f);
            transform.Rotate(Vector3.up*angularVelocity.x);
		}
	}
	public void CheckKeys() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			mouseLock = false;
			CheckMouse();
		}
		if(Input.GetMouseButtonDown(0)) {
			mouseLock = true;
			CheckMouse();
		}
	}
	public void CheckMouse() {
		if (mouseLock) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	float EulerUnsignedToSigned(float rotation) {
		return (rotation+180)%360 -180;
	}
}
