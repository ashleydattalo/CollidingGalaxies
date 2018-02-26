using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour {
	string[] galaxyOptionNames = new string[] {
		"galaxy20000",
		"30k_galaxy",
		"chainsawVsGalaxy",
		"galaxy100000",
		"ThreePassingGalaxies"
	};
	GameObject [] galaxyOptions; 
	float m_lastPressed = 0.0f;
	int i;
	private int numGalaxyOptions;

    //Keyboard Input
    private bool returnPressed;
    private bool rightArrowPressed;
    private bool leftArrowPressed;

    //VR Input
	private Vector2 primary;
    private Vector2 secondary;
    private bool aPressed;
	private bool leftSecondaryPressed;
	private bool rightSecondaryPressed;
	private bool leftPrimaryPressed;
	private bool rightPrimaryPressed;


	void Start () {
		returnPressed = false;
		rightArrowPressed = false;
		leftArrowPressed = false;

		aPressed = false;
		leftSecondaryPressed = false;
		rightSecondaryPressed = false;
		leftPrimaryPressed = false;
		rightPrimaryPressed = false;

		i = 0;
		numGalaxyOptions = galaxyOptionNames.Length;
		galaxyOptions = new GameObject[numGalaxyOptions];
		CreateGalaxySprites();
		HighlightCorrectSprite();
	}
	
	
	void Update () {
		SetKeyboardInput();
		SetVRInput();
		HandleUserInput();
		HighlightCorrectSprite();
	}

	void SetKeyboardInput() {
		returnPressed = Input.GetKeyDown(KeyCode.Return);
		rightArrowPressed = Input.GetKeyDown(KeyCode.RightArrow);
		leftArrowPressed = Input.GetKeyDown(KeyCode.LeftArrow);
	}
	void SetVRInput() {
		primary = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        secondary = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        aPressed = OVRInput.GetDown(OVRInput.Button.One);
	}

	void HandleUserInput() {
		HandleKeyboardInput();
		HandleVRInput();
	}

	void HandleKeyboardInput() {
		if (returnPressed) {
            GoToRenderGalaxies();
		}
		if (rightArrowPressed) {
			IterateRight();
		}
		if (leftArrowPressed) {
			IterateLeft();
		}
	}

	void HandleVRInput() {
		if (aPressed) {
            GoToRenderGalaxies();
		}

		//Set secondary
		if (secondary.x < -0.02f) {
            leftSecondaryPressed = true;
        }
        if (secondary.x > 0.2f) {
            rightSecondaryPressed = true;
        }
        if (secondary.x == 0.0f) {
        	if (leftSecondaryPressed) {
        		IterateLeft();
        	}
        	if (rightSecondaryPressed) {
        		IterateRight();
        	}
        	leftSecondaryPressed = false;
        	rightSecondaryPressed = false;
        }

     	//set primary
        if (primary.x < -0.02f) {
            leftPrimaryPressed = true;
        }
        if (primary.x > 0.2f) {
            rightPrimaryPressed = true;
        }
        if (primary.x == 0.0f) {
        	if (leftPrimaryPressed) {
        		IterateLeft();
        	}
        	if (rightPrimaryPressed) {
        		IterateRight();
        	}
        	leftPrimaryPressed = false;
        	rightPrimaryPressed = false;
        }

	}

	void GoToRenderGalaxies() {
		GameObject go = new GameObject();
		go.name = "savedData";

		GameObject galaxyToRender = new GameObject();
		galaxyToRender.name = galaxyOptionNames[i];
		galaxyToRender.transform.parent = go.transform;

		DontDestroyOnLoad(go);
		SceneManager.LoadScene("RenderGalaxies", LoadSceneMode.Single);
	}

	void IterateRight() {
		if (m_lastPressed != Time.time) {
		    m_lastPressed = Time.time;
			Increment();
			Debug.Log(galaxyOptionNames[i]);
		}
	}

	void IterateLeft() {
		if (m_lastPressed != Time.time) {
		    m_lastPressed = Time.time;
			Decrement();
			Debug.Log(galaxyOptionNames[i]);
		}
	}

	void Increment() {
		if (i + 1 >= numGalaxyOptions) {
			i = 0;
		}
		else {
			i++;
		}
	}

	void Decrement() {
		if (i - 1 < 0) {
			i = numGalaxyOptions - 1;
		}
		else {
			i--;
		}
	}

	void CreateGalaxySprites() {
		CalcMovement calcMovement = new CalcMovement(numGalaxyOptions, numGalaxyOptions);
		int count = 0;

		float xPos, yPos, zPos;
		float numDevicesInRow = (float) numGalaxyOptions; 
		float radius = 35.0f;
		float thetaInit = 180.0f;
		float thetaFinal = 0.0f;
		float thetaInc = (thetaInit - thetaFinal) / numDevicesInRow;
		float theta = thetaInit;


		foreach (string s in galaxyOptionNames) {
			Vector3 newPos = calcMovement.getNewPos();
			GameObject sprite = new GameObject();
			
			theta -= thetaInc;
			xPos = radius * Mathf.Cos(theta * Mathf.Deg2Rad);
			zPos = radius * Mathf.Sin(theta * Mathf.Deg2Rad);
			yPos = 0.0f;
			sprite.transform.position = new Vector3(xPos, yPos, zPos);
	        
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
}