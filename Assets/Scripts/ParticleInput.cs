using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInput : MonoBehaviour {
	Vector3 lTouchPos, rTouchPos;

	bool LeftIndexPressed;
	bool LeftMiddlePressed;
	bool RightIndexPressed;
	bool RightMiddlePressed;
	
	GameObject camera;
	GameObject cube;
	GameObject pointer;

	void Start() {
		camera = (GameObject) GameObject.Find("MainCamera");
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        cube.transform.localScale -= new Vector3(0.95f, 0.95f, 0.95f);

        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        pointer.transform.localScale -= new Vector3(0.985f, 0.985f, 0.985f);
	}

	int i = 0;
	int particleCount = 0;
	Scale scale = new Scale(0.0f, 100.0f);
	void Update () {
		SetPosInput();
		SetTriggerInput();
		//Debugging();
		pointer.transform.position = camera.transform.position + rTouchPos;

		if (LeftIndexPressed) {
			if (i++ % 2 == 0) {
				Vector3 pos = camera.transform.position + (lTouchPos);
				GameObject copy = Instantiate(cube, pos, this.transform.rotation);
				Vector3 scaleCol = scale.getRainbow((float)(i%100));
				Color col = new Color(scaleCol.x, scaleCol.y, scaleCol.z, 1.0f);
				copy.GetComponent<Renderer>().material.color = col;
				particleCount++;
			}
		}
		if (RightIndexPressed) {
			if (i++ % 2 == 0) {
				Vector3 pos = camera.transform.position + (rTouchPos);
				GameObject copy = Instantiate(cube, pos, this.transform.rotation);
				Vector3 scaleCol = scale.getRainbow((float)(i%100));
				Color col = new Color(scaleCol.x, scaleCol.y, scaleCol.z, 1.0f);
				copy.GetComponent<Renderer>().material.color = col;
				particleCount++;
			}
		}
	}
	void OnApplicationQuit() {
		Debug.Log(particleCount);
	}
	
	void Debugging() {
		//Debug.Log("left: " + lTouchPos + " right: " + rTouchPos);

		if (LeftIndexPressed) {
			Debug.Log("LeftIndexPressed");
		}
		if (LeftMiddlePressed) {
			Debug.Log("LeftMiddlePressed");
		}
		if (RightIndexPressed) {
			Debug.Log("RightIndexPressed");
		}
		if (RightMiddlePressed) {
			Debug.Log("RightMiddlePressed");
		}
	}
	void SetPosInput() {
		lTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		rTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
	}

	void SetTriggerInput() {
		LeftIndexPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
		LeftMiddlePressed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
		RightIndexPressed = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
		RightMiddlePressed = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
	}
}
