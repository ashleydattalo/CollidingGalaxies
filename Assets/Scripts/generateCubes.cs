using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateCubes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject cube = (GameObject)GameObject.Find("Cube");
		for (int row = 0; row < 10; row++) {
			for (int col = 0; col < 10; col++) {
				for (int k = 0; k < 10; k++) {
					Vector3 pos = new Vector3(row * 5.0f, col * 5.0f, k *5.0f);
					Instantiate(cube, pos, this.transform.rotation);
				}
			}
		}	
	}
	
}
