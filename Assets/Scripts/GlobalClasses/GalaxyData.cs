using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GalaxyData {
    private string galaxy20000 = "./Assets/GalaxyData/SuedasTextFiles/galaxy20000.txt";
    private string g_30k_galaxy = "./Assets/GalaxyData/SuedasTextFiles/30k_galaxy.txt";
    private string chainsawVsGalaxy = "./Assets/GalaxyData/SuedasTextFiles/chainsawVsGalaxy.txt";
    private string galaxy100000 = "./Assets/GalaxyData/SuedasTextFiles/galaxy100000.txt";
    private string ThreePassingGalaxies = "./Assets/GalaxyData/SuedasTextFiles/ThreePassingGalaxies.txt";
    private string bunny = "./Assets/GalaxyData/Objs/bunny.obj";

    private string path = "./Assets/GalaxyData/SuedasTextFiles/";

    public struct Star
	{
		public Vector3 position;
		public Vector3 velocity;
		public Vector3 color;
		public float mass;
		public float radius;
	}

	public Vector3 minStarPos;
	public Vector3 maxStarPos;

	public Star [] stars;
	public int numStars;
	public float e, h;

	public bool useDefaultNumStars;
	public int maxNumStars;

	public string fileName;

	private bool usePosOffset;
	private Vector3 posOffset;

	private bool parseObjFile;

	public GalaxyData() {
		useDefaultNumStars = true;
		fileName = galaxy20000;
		usePosOffset = false;
		parseObjFile = false;
	}

	public GalaxyData(string inputName) {
		useDefaultNumStars = true;
		fileName = path + inputName + ".txt";
		usePosOffset = false;
		parseObjFile = false;
	}

	public void setPosOffset(Vector3 posOffset) {
		this.posOffset = posOffset;
		usePosOffset = true;
	}

	public Star [] getGalaxyData() {
		return stars;
	}

	public Star getStarAt(int i) {
		return stars[i];
	}

	public int getNumStars() {
		return numStars;
	}

	public float getE() {
		return this.e;
	}

	public float getH() {
		return this.h;
	}

	public void parseFile() {
		if (parseObjFile) {
			parsingObjFile();
		}
		else {
			parseNonObjFile();			
		}
	}

	public void parseNonObjFile() {
		StreamReader sr = new StreamReader(this.fileName);
		string line = sr.ReadLine();
		string[] firstLine = line.Split(' ');

		if (useDefaultNumStars) {
			this.numStars = int.Parse(firstLine[0]);
		}
		else {
			this.numStars = maxNumStars;
		}
		Debug.Log(this.numStars);
		this.stars = new Star[this.numStars];
		this.h = float.Parse(firstLine[1]);
		this.e = float.Parse(firstLine[2]);

		float m, r;
		Vector3 pos, vel, col;
		
		int starIdx = 0;
		while ((line = sr.ReadLine()) != null) 
		{
			if (!useDefaultNumStars) {
				if (starIdx >= maxNumStars) {
					break;
				}
			}
			string[] arr = line.Split(' ');
			int i = 0;
			m = float.Parse(arr[i++]);
			pos = new Vector3(float.Parse(arr[i++]), float.Parse(arr[i++]), float.Parse(arr[i++]));
			vel = new Vector3(float.Parse(arr[i++]), float.Parse(arr[i++]), float.Parse(arr[i++]));
			col = new Vector3(float.Parse(arr[i++]), float.Parse(arr[i++]), float.Parse(arr[i++]));
			r = float.Parse(arr[i++]);
			stars[starIdx] = createStar(m, pos, vel, col, r);
			
			// set the max and min star pos to the first star;
			if (starIdx == 0) {
				minStarPos = pos;
				maxStarPos = pos;
			}
			else {
				// change the max
				if (pos.x > maxStarPos.x) {
					maxStarPos.x = pos.x;
				}
				if (pos.y > maxStarPos.y) {
					maxStarPos.y = pos.y;
				}
				if (pos.z > maxStarPos.z) {
					maxStarPos.z = pos.z;
				}
				// change the min
				if (pos.x < minStarPos.x) {
					minStarPos.x = pos.x;
				}
				if (pos.y < minStarPos.y) {
					minStarPos.y = pos.y;
				}
				if (pos.z < minStarPos.z) {
					minStarPos.z = pos.z;
				}
			}

			starIdx++;
		}
		sr.Close();
	}

	public void parsingObjFile() {
		StreamReader sr = new StreamReader(this.fileName);
		string line = sr.ReadLine();
		string[] firstLine = line.Split(' ');

		if (useDefaultNumStars) {
			this.numStars = 2 * int.Parse(firstLine[0]);
			//num faces = int.Parse(firstLine[1]);
		}
		else {
			this.numStars = maxNumStars;
		}

		this.stars = new Star[this.numStars];
		this.h = 0.004f;
		this.e = 0.0001f;

		float m, r;
		Vector3 pos, vel, col, pos2, vel2, col2, pos3, vel3, col3;
		
		int starIdx = 0;
		Scale scale = new Scale(0.0f, (float)this.numStars);
		while ((line = sr.ReadLine()) != null) 
		{
			if (!useDefaultNumStars) {
				if (starIdx >= maxNumStars) {
					break;
				}
			}
			string[] arr = line.Split(' ');
			if (arr[0] == "v") {
				// add the vertex to the star data
				int i = 1;
				if (i % 400 == 0) {
					//m = scale.getScale((float)starIdx);
					m = UnityEngine.Random.Range(0.01f, 0.1f);
				}
				else {
					m = UnityEngine.Random.Range(0.0001f, 0.001f);
				}

				pos = new Vector3(float.Parse(arr[i++]), float.Parse(arr[i++]), float.Parse(arr[i++]));
				pos *= 20.0f;
				//vel = new Vector3(float.Parse(arr[i++]), float.Parse(arr[i++]), float.Parse(arr[i++]));
				//col = new Vector3(float.Parse(arr[i++]), float.Parse(arr[i++]), float.Parse(arr[i++]));
				//r = float.Parse(arr[i++]);
				//Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
				//vel = pos;
				float vx, vy, vz;
				vx = UnityEngine.Random.Range(0.001f, 0.1f);
				vy = UnityEngine.Random.Range(0.001f, 0.1f);
				vz = UnityEngine.Random.Range(0.001f, 0.1f);
				vel = new Vector3(vx, vy, vz);
				col = new Vector3(0.5f, 1.0f, 1.0f);
				r = 1.0f;
				stars[starIdx++] = createStar(m, pos, vel, col, r);
				
				pos *= 2f;
				pos2 = pos + new Vector3(0.5f, -2f, 0.0f);
				vx = UnityEngine.Random.Range(0.0001f, 0.1f);
				vy = UnityEngine.Random.Range(0.0001f, 0.1f);
				vz = UnityEngine.Random.Range(0.0001f, 0.1f);
				vel2 = new Vector3(vx, vy, vz);
				col2 = new Vector3(1.0f, 0.5f, 1.0f);
				m = UnityEngine.Random.Range(0.000001f, 0.001f);
				stars[starIdx++] = createStar(m, pos2, vel2, col2, r);


				/*
				pos *= 1.6f;
				pos3 = pos + new Vector3(0.5f, -4f, 0.0f);
				vy = UnityEngine.Random.Range(0.00001f, 0.001f);
				vz = UnityEngine.Random.Range(0.00001f, 0.001f);
				vel3 = new Vector3(vx, vy, vz);
				col3 = new Vector3(1.0f, 1.0f, 1.0f);
				m = UnityEngine.Random.Range(0.00001f, 0.001f);
				stars[starIdx++] = createStar(m, pos3, vel3, col3, r);
				*/

				/*// set the max and min star pos to the first star;
				if (starIdx == 0) {
					minStarPos = pos;
					maxStarPos = pos;
				}
				else {
					// change the max
					if (pos.x > maxStarPos.x) {
						maxStarPos.x = pos.x;
					}
					if (pos.y > maxStarPos.y) {
						maxStarPos.y = pos.y;
					}
					if (pos.z > maxStarPos.z) {
						maxStarPos.z = pos.z;
					}
					// change the min
					if (pos.x < minStarPos.x) {
						minStarPos.x = pos.x;
					}
					if (pos.y < minStarPos.y) {
						minStarPos.y = pos.y;
					}
					if (pos.z < minStarPos.z) {
						minStarPos.z = pos.z;
					}
				}*/
			}
			
		}
		sr.Close();
	}

	private Star createStar(float m, Vector3 pos, Vector3 vel, Vector3 col, float r) {
		Star star = new Star();
		star.position = pos;
		star.velocity = vel;
		star.mass = m;
		star.color = col;
		star.radius = r;
		return star;
	}

	// Set a custom amount of stars to use
	public void setCustomNumStars(int customNumStars) {
		useDefaultNumStars = false;
		this.maxNumStars = customNumStars;
	}

	// Set which galaxy to draw
	public void use30k() {
		this.fileName = g_30k_galaxy;
	}
	public void useChainsaw() {
		this.fileName = chainsawVsGalaxy;
	}
	public void use20000() {
		this.fileName = galaxy20000;
	}
	public void use100000() {
		this.fileName = galaxy100000;
	}
	public void useThreePassing() {
		this.fileName = ThreePassingGalaxies;
	}
	public void useBunny() {
		this.fileName = bunny;
		this.parseObjFile = true;
	}
}