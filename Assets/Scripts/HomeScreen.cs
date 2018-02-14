using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour {
	string[] names = new string[] {
		"galaxy20000",
		"30k_galaxy",
		"chainsawVsGalaxy",
		"galaxy100000",
		"ThreePassingGalaxies"
	};
	GameObject [] galaxyOptions; 
	float m_lastPressed = 0.0f;
	int i;
	
	void Start () {
		i = 0;
		galaxyOptions = new GameObject[names.Length];
		// CreateGalaxyGameObjects();
		CreateGalaxySprites();
		// HighlightCorrectCube();
		HighlightCorrectSprite();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) || OVRInput.GetDown(OVRInput.Button.One)) {
            GoToRenderGalaxies();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) {
			IterateRight();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) || OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) {
			IterateLeft();
		}
		// HighlightCorrectCube();
		HighlightCorrectSprite();
	}


	void GoToRenderGalaxies() {
		GameObject go = new GameObject();
		go.name = "savedData";

		GameObject galaxyToRender = new GameObject();
		galaxyToRender.name = names[i];
		galaxyToRender.transform.parent = go.transform;

		DontDestroyOnLoad(go);
		SceneManager.LoadScene("RenderGalaxies", LoadSceneMode.Single);
	}

	void IterateRight() {
		if (m_lastPressed != Time.time) {
		    m_lastPressed = Time.time;
			Increment();
			Debug.Log(names[i]);
		}
	}

	void IterateLeft() {
		if (m_lastPressed != Time.time) {
		    m_lastPressed = Time.time;
			Decrement();
			Debug.Log(names[i]);
		}
	}

	void Increment() {
		if (i + 1 >= names.Length) {
			i = 0;
		}
		else {
			i++;
		}
	}

	void Decrement() {
		if (i - 1 < 0) {
			i = names.Length - 1;
		}
		else {
			i--;
		}
	}

	void CreateGalaxyGameObjects() {
		CalcMovement calcMovement = new CalcMovement(names.Length, names.Length);
		GameObject cube = (GameObject)GameObject.Find("Cube");
		int count = 0;
		foreach (string s in names) {
			Vector3 newPos = calcMovement.getNewPos();
			GameObject temp = Instantiate(cube, newPos, this.transform.rotation);
			galaxyOptions[count++] = temp;
		}
		Destroy(cube);		
	}

	void CreateGalaxySprites() {
		CalcMovement calcMovement = new CalcMovement(names.Length, names.Length);
		int count = 0;
		foreach (string s in names) {
			Vector3 newPos = calcMovement.getNewPos();
			GameObject sprite = new GameObject();
			sprite.transform.position = newPos;
	        SpriteRenderer renderer = sprite.AddComponent<SpriteRenderer>();
	        renderer.sprite = (Sprite)Resources.Load(s, typeof(Sprite));
	        sprite.transform.LookAt(Camera.main.transform.position, -Vector3.up);
			galaxyOptions[count++] = sprite;
		}
		
	}

	void HighlightCorrectSprite() {
		Renderer rend;
        foreach (GameObject go in galaxyOptions) {
	        go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
        galaxyOptions[i].transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
	}

	void HighlightCorrectCube() {
		Renderer rend;
        foreach (GameObject go in galaxyOptions) {
			rend = go.GetComponent<Renderer>();
	        rend.material.shader = Shader.Find("Specular");
	        rend.material.SetColor("_SpecColor", Color.white);
	        go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		rend = galaxyOptions[i].GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red);
        galaxyOptions[i].transform.localScale += new Vector3(2.0f, 2.0f, 2.0f);
	}
}