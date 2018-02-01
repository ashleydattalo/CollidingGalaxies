using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code modeled off of: https://learnopengl.com/#!Getting-started/Camera
public class CameraMovement : MonoBehaviour {
	float speed;

	// movement in x dir
	bool moveRight;
	bool moveLeft;
	// movement in y dir
	bool moveUp;
	bool moveDown;
	// movement in z dir
	bool moveIn;
	bool moveOut;

	bool mousePressed;
	float currMouseX, currMouseY;
	float prevMouseX, prevMouseY;
	float mouseSensitivity;

	float yaw = -90f;
	float pitch = 0f;

	bool firstFrame = true;

	Vector3 front = new Vector3();
	Vector3 right = new Vector3();
	Vector3 worldUp = new Vector3(0.0f, 0.1f, 0.0f);

	void Start () {
		speed = 0.3f;
		mouseSensitivity = 1f;
		front = this.transform.forward;
        right = Vector3.Cross(front, worldUp);
	}
	
	// Update is called once per frame
	void Update () {
		//HandleMouseInput();
		HandleKeyInput();
		// HandleVRInput();
		firstFrame = false;
	}

	void HandleMouseInput() {
		if (Input.GetMouseButtonDown(0)) {
			mousePressed = true;
		}
		if (Input.GetMouseButtonUp(0)) {
			mousePressed = false;			
		}
        

		
		if (mousePressed) {
	        if (!firstFrame) {
	        	prevMouseX = currMouseX;
	        	prevMouseY = currMouseY;
	        }
	        else {
	        	prevMouseX = Input.mousePosition.x;
	        	prevMouseY = Input.mousePosition.y;
	        }
			if (Input.GetAxis("Mouse X") != 0) {
				currMouseX = Input.GetAxis("Mouse X");
				//currMouseX = Input.mousePosition.x;
			}
			if (Input.GetAxis("Mouse Y") != 0) {
				currMouseY = Input.GetAxis("Mouse Y");
				//currMouseY = Input.mousePosition.y;
			}
			UpdateCamVector();
		}
	}

	void UpdateCamVector() {
		float mouseXoffset = currMouseX;
		float mouseYoffset = currMouseY;

		Debug.Log(mouseXoffset + " " + mouseYoffset);
		
		float xoffset = mouseXoffset * mouseSensitivity;
        float yoffset = mouseYoffset * mouseSensitivity;

        yaw += xoffset;
        pitch += yoffset;

        if (pitch > 89.0f)
            pitch = 89.0f;
        if (pitch < -89.0f)
            pitch = -89.0f;

        float yawInRad = yaw * Mathf.Deg2Rad;
        float pitchInRad = pitch * Mathf.Deg2Rad;
        front.x = Mathf.Cos(yawInRad) * Mathf.Cos(pitchInRad);
        front.y = Mathf.Sin(pitchInRad);
        front.z = Mathf.Sin(yawInRad) * Mathf.Cos(pitchInRad);

        front = Vector3.Normalize(front);
        right = Vector3.Cross(front, worldUp);
		this.transform.forward = front;

	
	}

	void HandleKeyInput() {
		// move in x dir
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			moveRight = true;
		}
		if (Input.GetKeyUp(KeyCode.RightArrow)) {
			moveRight = false;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			moveLeft = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow)) {
			moveLeft = false;
		}		
		
		// move in z dir
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			moveOut = true;
		}
		if (Input.GetKeyUp(KeyCode.UpArrow)) {
			moveOut = false;
		}		
		
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			moveIn= true;
		}
		if (Input.GetKeyUp(KeyCode.DownArrow)) {
			moveIn = false;
		}
		
		// move in y dir
		if (Input.GetKeyDown(KeyCode.W)) {
			moveUp = true;
		}
		if (Input.GetKeyUp(KeyCode.W)) {
			moveUp = false;
		}		
		
		if (Input.GetKeyDown(KeyCode.S)) {
			moveDown = true;
		}
		if (Input.GetKeyUp(KeyCode.S)) {
			moveDown = false;
		}

        Move();
	}

	void HandleVRInput() {
        // transform.position = OVRInput.GetLocalControllerPosition(controller);
        // transform.rotation = OVRInput.GetLocalControllerRotation(controller);
 
        // Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        // Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Debug.Log(primaryAxis);
        // Debug.Log(secondaryAxis);

        // Debug.Log(UnityEngine.Input.GetJoystickNames());
	}

	void Move() {
		if (moveRight) {
			MoveRight();
		}
		if (moveLeft) {
			MoveLeft();
		}
		if (moveUp) {
			MoveUp();
		}
		if (moveDown) {
			MoveDown();
		}	

		if (moveOut) {
			MoveOut();
		}
		if (moveIn) {
			MoveIn();
		}
	}

	void MoveRight() {
		MoveInDir(-right);
	}

	void MoveLeft() {
		MoveInDir(right);
	}

	void MoveUp() {
		MoveInDir(new Vector3(0.0f, 0.1f, 0.0f));
	}

	void MoveDown() {
		MoveInDir(new Vector3(0.0f, -0.1f, 0.0f));
	}
	
	void MoveOut() {
		MoveInDir(front);
	}

	void MoveIn() {
		MoveInDir(-front);
	}

	void MoveInDir(Vector3 dir) {
		this.transform.localPosition += speed * dir;
	}
}
