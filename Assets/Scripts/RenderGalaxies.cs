using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RenderGalaxies : MonoBehaviour
{
	private bool useHomeScreen = true;
	private bool isMac = false;
	private struct Particle
	{
		public Vector3 position;
		public Vector3 velocity;
		public Vector3 color;
		public float mass;
	}
	private const int particleStructSize = (3+3+3+1) * 4;

	public Material material;
	public ComputeShader computeShader;
	private ComputeBuffer particleBuffer;

	private int kernelID;
	private int numThreadsInGroup;
	private int numGroups;

	private Particle[] particleArray;

	private string galaxyName;
	private GalaxyData galaxyData;
	private float e, h;
	private int particleCount;
	private int lastParticleIndex;
	private int EXTRA_PARTICLES = 15000;

	private float starMass = 5.0f;

	private string bufferName;
	private string kernelName;


	//VR input
	Vector3 lTouchPos, rTouchPos;

	bool LeftIndexPressed;
	bool LeftMiddlePressed;
	bool RightIndexPressed;
	bool RightMiddlePressed;
	bool simulate;
	
	GameObject camera;
	GameObject cube;
	GameObject pointer;

	void Start()
	{
		camera = (GameObject) GameObject.Find("CameraParent");
		setStrings();
		if (useHomeScreen) {
			getHomeSceneInput();
		}
		initalizeGalaxyData();
		setUpBuffers();
		setUpShaderConstants();
		setUpVR();

		setUpMenu();
	}

	void setStrings() {
		bufferName = "particleBuffer";
		if (isMac == true) {
			kernelName = "computeMac";
			numThreadsInGroup = 512;
		}
		else {
			kernelName = "computePC";
			numThreadsInGroup = 1024;
		}
	}

	void getHomeSceneInput() {
		GameObject savedData = GameObject.Find("savedData");
		GameObject galaxy = savedData.transform.GetChild(0).gameObject;
		if (galaxy != null) {
			galaxyName = galaxy.name;
		}
		Destroy(savedData);
	}

	void initalizeGalaxyData() {
		if (useHomeScreen) {
			galaxyData = new GalaxyData(galaxyName);
		}
		else {
			galaxyData = new GalaxyData();
	        galaxyData.use30k();
		}
		
		//Delete this maybe:
		{	
			if (isMac == true) {
				galaxyData.setCustomNumStars(1000);
			}
			else {
				//galaxyData.setCustomNumStars(35000);
			}

			//galaxyData.use20000(); 
			//galaxyData.use100000(); // framerate SUCKS...
			//galaxyData.useChainsaw();
	        //galaxyData.useThreePassing(); 
	        //galaxyData.useBunny();
		}

		camera.transform.position = galaxyData.getCameraPos();
		e = galaxyData.getE();
		h = galaxyData.getH();
		particleCount = galaxyData.getNumStars();

		Debug.Log(galaxyData.minStarPos);
		Debug.Log(galaxyData.maxStarPos);
		Debug.Log(particleCount);

		numGroups = Mathf.CeilToInt((float)(particleCount + EXTRA_PARTICLES) / numThreadsInGroup);

		particleArray = new Particle[particleCount + EXTRA_PARTICLES];
		for (int i = 0; i < particleCount; i++) {
			particleArray[i].position = galaxyData.getStarAt(i).position;
			particleArray[i].velocity = galaxyData.getStarAt(i).velocity;
			particleArray[i].color = galaxyData.getStarAt(i).color;
			// particleArray[i].color = new Vector3(1.0f, 1.0f, 1.0f);
			particleArray[i].mass = galaxyData.getStarAt(i).mass;
		}
		for (int i = 0; i < EXTRA_PARTICLES; i++) {
			int iOffset = i + particleCount;
			particleArray[iOffset].position = new Vector3(0.0f, 0.0f, 0.0f);
			particleArray[iOffset].velocity = new Vector3(0.0f, 0.0f, 0.0f);
			particleArray[iOffset].color = new Vector3(0.0f, 0.0f, 0.0f);
			particleArray[iOffset].mass = 0.0f;
		}
	}

	void setUpBuffers() {
		particleBuffer = new ComputeBuffer(particleCount + EXTRA_PARTICLES, particleStructSize);
		particleBuffer.SetData(particleArray);

		kernelID = computeShader.FindKernel(kernelName);

		computeShader.SetBuffer(kernelID, bufferName, particleBuffer);
		material.SetBuffer(bufferName, particleBuffer);
	}

	void setUpShaderConstants() {
		computeShader.SetFloat("e", e);
		//computeShader.SetFloat("h", 0.007f);
        computeShader.SetFloat("h", h);
        computeShader.SetFloat("bufSize", particleCount);
	}

	void setUpVR() {
		simulate = true;

		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        cube.transform.localScale -= new Vector3(0.95f, 0.95f, 0.95f);

        pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointer.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        pointer.transform.localScale -= new Vector3(0.995f, 0.995f, 0.995f);
	}




	void Update() {
		GetVRInput();
		ProcessVRInput();
		if (simulate) {
			ComputeStarMovement();
		}
		Debug.Log(camera.transform.position);
	}

	bool PrevLeftIndexPressed;
	bool PrevLeftMiddlePressed;
	bool showingMenu = false;
	void GetVRInput() {
		lTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
		rTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

		PrevLeftIndexPressed = LeftIndexPressed;
		PrevLeftMiddlePressed = LeftMiddlePressed;

		LeftIndexPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
		LeftMiddlePressed = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
		RightIndexPressed = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
		// RightMiddlePressed = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);


		// move to Process VR input

		if (OVRInput.GetDown(OVRInput.Button.One)) {
			simulate = !simulate;
		}

		if (LeftIndexPressed && !PrevLeftIndexPressed) {
			showingMenu = !showingMenu;
		}
		if (showingMenu) {
			ShowMenu();
		}
		else {
			HideMenu();
		}

		if (LeftMiddlePressed) {
			ChangeStarMass();
		}
	}

	int i = 0;
	int particlesAdded = 0;
	Scale scale = new Scale(0.0f, 100.0f);
	void ProcessVRInput() {
		Vector3 offset = new Vector3(0.0f, 0.0f, 00.0f);
		pointer.transform.position = camera.transform.position + rTouchPos + offset;
		if (RightIndexPressed) {
			//if (i++ % 2 == 0) {
				Debug.Log("pressed");
				Particle particleToAdd = new Particle();
				particleToAdd.position = camera.transform.position + (rTouchPos) + offset;
				particleToAdd.velocity = new Vector3(0.0f, 0.0f, 0.0f);
				particleToAdd.color = scale.getRainbow((float)(particlesAdded%100)) + scale.getRainbow((float)(particlesAdded%100));
				// particleToAdd.color = new Vector3(1.0f, 1.0f, 0.5f);
				particleToAdd.mass = starMass;
				particlesAdded++;

				AddParticleToBuffer(particleToAdd);
			//}
		}
	}

	void ComputeStarMovement() {
		computeShader.Dispatch(kernelID, numGroups, 1, 1);
	}

	void AddParticleToBuffer(Particle particleToAdd) {
		particleBuffer.GetData(particleArray);
		particleArray[particleCount] = particleToAdd;
		particleBuffer.SetData(particleArray, particleCount, particleCount, 1);
		particleCount++;
		computeShader.SetFloat("bufSize", particleCount);
	}

	void OnRenderObject() {
		material.SetPass(0);
		Graphics.DrawProcedural(MeshTopology.Points, 1, particleCount);
	}

	void OnDestroy() {
		if (particleBuffer != null) {
			particleBuffer.Release();
		}
		Debug.Log("particlesAdded: " + particlesAdded);
	}

	GameObject message;

	void setUpMenu() {
		simulate = false;
		message = new GameObject("StarMassMessage");
		message.transform.position =  camera.transform.position + camera.transform.forward * 15.0f;
		message.transform.rotation = this.transform.rotation;
		addTextComponent(message, "Star Mass: " + starMass);
	}
	void ShowMenu() {
		simulate = false;
		message.transform.position =  camera.transform.position + camera.transform.forward * 15.0f;
		//message.transform.rotation = this.transform.rotation;
		GameObject messageText = message.transform.GetChild(0).gameObject;
		messageText.GetComponent<Text>().enabled = true;
	}
	void HideMenu() {
		//simulate = true;
		GameObject messageText = message.transform.GetChild(0).gameObject;
		messageText.GetComponent<Text>().enabled = false;
	}
	void ChangeStarMass() {
		starMass += 1.0f;
		GameObject messageText = message.transform.GetChild(0).gameObject;
		messageText.GetComponent<Text>().text = "Star Mass: " + starMass;
		//addTextComponent(message, "Star Mass: " + starMass);
	}

	void addTextComponent(GameObject go, string text) {
		GameObject child = new GameObject();
		child.name = "Text";
		child.transform.parent = go.transform;

		Canvas canvas = child.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.WorldSpace;
		CanvasScaler cs = child.AddComponent<CanvasScaler>();
		cs.scaleFactor = 20.0f;
		cs.dynamicPixelsPerUnit = 20.0f;

		Text t = child.AddComponent<Text>();
		t.GetComponent<RectTransform>().sizeDelta = new Vector2 (1, 1);
		Vector3 textPos = go.transform.position;
		Quaternion textRot = go.transform.rotation;
		t.GetComponent<RectTransform>().position = textPos + new Vector3(0, 2f, 0);
		t.GetComponent<RectTransform>().rotation = textRot;
		t.alignment = TextAnchor.MiddleCenter;
		t.horizontalOverflow = HorizontalWrapMode.Overflow;
		t.verticalOverflow = VerticalWrapMode.Overflow;
		Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
		t.font = ArialFont;
		t.fontSize = 1;
		t.text = text;
		t.enabled = true;
		t.color = Color.white;
	}
}