using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInput : MonoBehaviour {
    private Vector2 primary;
    private Vector2 secondary;

    private Vector3 worldUp;
    private Vector3 forward;

    private Vector3 front; //Camera.main.transform.forward
    private Vector3 up;
    private Vector3 right;

    private float speed = 0.01f;
    bool movedRight;
    bool movedLeft;
    bool movedUp;
    bool movedDown;
    bool movedIn;
    bool movedOut;

    enum Direction {RIGHT, LEFT, UP, DOWN, IN, OUT, NONE};

    void Start () {
        worldUp = new Vector3(0.0f, 1.0f, 0.0f);
        forward = Camera.main.transform.forward;
        UpdateCameraVectors();
    }

    void Update () {
        GetVRInput();

        if (forward != Camera.main.transform.forward) {
            forward = Camera.main.transform.forward;
            forward.y = 0.0f;
            UpdateCameraVectors();
        }
        GetMovementDirection();
        ProcessVRInput();
    }

    void GetVRInput() {
        primary = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        secondary = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //Debug.Log("primary: " + primary + " secondary: " + secondary);
    }

    void ProcessVRInput() {
        if(movedRight) {
            this.transform.position += right * speed;
        }
        if(movedLeft) {
            this.transform.position -= right * speed;
        }
        if(movedUp) {
            this.transform.position += up * speed;
        }
        if(movedDown) {
            this.transform.position -= up * speed;
        }
        if(movedIn) {
            this.transform.position -= front * speed * 2.0f;
        }
        if(movedOut) {
            this.transform.position += front * speed * 2.0f;
        }
    }

    // The direction represents the direction the scene
    void GetMovementDirection() {
        movedRight = false;
        movedLeft = false;
        movedUp = false;
        movedDown = false;
        movedIn = false;
        movedOut = false;

        if (secondary.x < -0.02f) {
            movedRight = true;
        }
        if (secondary.x > 0.2f) {
            movedLeft = true;
        }
        if (primary.y > 0.2f) {
            movedUp = true;
        }
        if (primary.y < -0.2f) {
            movedDown = true;
        }
        if (secondary.y < -0.2f) {
            movedIn = true;
        }
        if (secondary.y > 0.2f) {
            movedOut = true;
        }
    }

    void UpdateCameraVectors() {
        front = Vector3.Normalize(forward);
        right = Vector3.Normalize(Vector3.Cross(front, worldUp));
        up = Vector3.Normalize(Vector3.Cross(right, front));
    }


}
