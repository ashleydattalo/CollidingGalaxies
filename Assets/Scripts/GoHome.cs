using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject savedData = GameObject.Find("savedData");
		GameObject galaxy = savedData.transform.GetChild(0).gameObject;
		if (galaxy != null) {
			Debug.Log(galaxy.name);
		}
		Destroy(savedData);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Backspace)) {
            GoToHomeScene();
		}
		if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) {
			Debug.Log("back pressed");
            GoToHomeScene();
		}
	}

	void GoToHomeScene() {
		SceneManager.LoadScene("HomeScreen", LoadSceneMode.Single);
	}
}
