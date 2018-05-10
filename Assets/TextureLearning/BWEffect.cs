using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class BWEffect : MonoBehaviour {
 
 private Shader shader;
 private float intensity;
 private float change;
 private Material material;
 
	// Creates a private material used to the effect
	void Awake ()
	{
		material = new Material( Shader.Find("BWDiffuse") );
		intensity = 0.0f;
		change = 0.001f;
	}

	void Update() {
		if (intensity >= 1.0f) {
	 		change = -0.001f;
		}
		if (intensity < 0) {
			change = 0.001f;
		}

		intensity += change;

		Debug.Log(intensity);
	}
 
	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (intensity == 0) {
			Graphics.Blit (source, destination);
		 	return;
	 	}
	 	material.SetFloat("_bwBlend", intensity);
 Graphics.Blit (source, destination, material);
	 // 	int iterations = 16;
	 // 	int width = source.width; // /2;
		// int height = source.height; // /2;
		// RenderTextureFormat format = source.format;

	 // 	RenderTexture currentDestination = RenderTexture.GetTemporary(width, height, 0, format);

	 // 	Graphics.Blit(source, currentDestination);
		// RenderTexture currentSource = currentDestination;
		// Graphics.Blit(currentSource, destination);
		// RenderTexture.ReleaseTemporary(currentSource);
		

		// for (int i = 1; i < iterations; i++) {
		// 	width /= 2;
		// 	height /= 2;
		// 	currentDestination = RenderTexture.GetTemporary(width, height, 0, format);
		// 	Graphics.Blit(currentSource, currentDestination);
		// 	RenderTexture.ReleaseTemporary(currentSource);
		// 	currentSource = currentDestination;
		// }

		// Graphics.Blit(currentSource, destination);
	}
}