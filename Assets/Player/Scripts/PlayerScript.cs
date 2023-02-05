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
	public float walkSpeed            = 3.00f;
	public float runSpeed             = 4.00f;
	public float jumpHeight           = 0.40f;
	[Header("Camera")]
	public float cameraSensitivity    = 3.00f;
	[Header("Physics")]
	public float gravity              = -9.81f;
	public LayerMask colliders;
	[Header("References")]
	public Camera mainCamera;
	public SphereCollider collisionDetector;
    [Header("Input")]
    public KeyCode keyDescend = KeyCode.C;
	
	[HideInInspector]public CharacterController playerController;
	[HideInInspector]public Vector3   velocity            = Vector3.zero;
    [HideInInspector]public Vector3[] velocityAdditive    = new Vector3[16];
    [HideInInspector]public Vector3[] velocityDamper      = new Vector3[16];
	[HideInInspector]public bool      grounded            = false;
	[HideInInspector]public bool      mouseLock           = true;
    [HideInInspector]public bool      useGravity          = true;

	void Start() {
		playerController = GetComponent<CharacterController>();
		CheckMouse();
        velocityAdditive = new Vector3[16];
        velocityDamper   = new Vector3[16];
	}
	void Update() {
		CheckKeys();
		CalculateGravity();
		MoveController();
		RotateCamera();
	}

    public void Move(float x, float y, float z, int id) {
        velocityAdditive[id] = new Vector3(x,y,z);
    }
    public void MoveInterrupt(float x, float y, float z) {
        playerController.Move(new Vector3(x, y, z));
    }

	public void CalculateGravity() {
		grounded = Physics.CheckSphere(collisionDetector.transform.position, collisionDetector.radius, colliders, QueryTriggerInteraction.Ignore);
        if(!useGravity) { 
            velocity.y = 0;
            return;
        }
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
        playerController.Move((Vector3.Scale(velocity, GetDamper()) + GetAdditive())*Time.deltaTime);
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

    public Vector3 GetAdditive() {
        Vector3 additive = Vector3.zero;
        for(int x = 0; x < velocityAdditive.Length; x++) {
            additive += velocityAdditive[x];
        }
        return additive;
    }
    public Vector3 GetDamper() {
        for(int x = 0; x < velocityDamper.Length; x++) {
            if(velocityDamper[x] != Vector3.zero) {
                return new Vector3(Mathf.Pow(2,-velocityDamper[x].x), Mathf.Pow(2,-velocityDamper[x].y), Mathf.Pow(2,-velocityDamper[x].z));
            }
        }
        return Vector3.one;
    }

    public void ForceLook(Vector3 target, float strength) {
        Vector3 offset = target - mainCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(offset,transform.up);
        Quaternion newRotation = Quaternion.Slerp(mainCamera.transform.rotation,targetRotation,strength*Time.deltaTime);
        mainCamera.transform.localEulerAngles = new Vector3(newRotation.eulerAngles.x,0,0);
        transform.localEulerAngles = new Vector3(0,newRotation.eulerAngles.y,0);
    }
    public void ForceStop(float strength, int id) {
        ForceStop(strength, strength, strength, id);
    }
    public void ForceStop(float xStrength, float yStrength, float zStrength, int id) {
        if(yStrength == 0) {
            useGravity = true;
        } else {
            useGravity = false;
        }
        velocityDamper[id] = new Vector3(xStrength, yStrength, zStrength);
    }
    public void Climb(Vector3 entranceVector, float bottomHeight, float topHeight, float ascendSpeed, float descendSpeed) {
        if(entranceVector.magnitude == 0) {
            ForceStop(0, 0);         // ID 0 for Climbing
            return;
        }

        float strength = Vector3.Dot(Vector3.ClampMagnitude(velocity, 1), Vector3.ClampMagnitude(entranceVector, 1));
        float height   = transform.position.y;
        
        useGravity = false;
        if(height < bottomHeight + 1) {
            float delta = Mathf.Clamp(height - bottomHeight, 0, 1);
            ForceStop(delta * 64, 0);
        } else if(height > topHeight - 1) {
            float delta = Mathf.Clamp(topHeight - height, 0, 1);
            ForceStop(delta * 64, 0);
        } else {
            ForceStop(64, 0);
        }

        if(strength > 0) {
            Move(0, strength * ascendSpeed, 0, 0);
        } else if(strength < 0) {
            Move(0, strength * descendSpeed, 0, 0);
        } else if(Input.GetKey(keyDescend)) {
            Move(0, -descendSpeed, 0, 0);
        } else {
            Move(0, 0, 0, 0);
        }
    }
}
