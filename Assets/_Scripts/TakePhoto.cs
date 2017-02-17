using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TakePhoto : MonoBehaviour {

	bool takePhoto = false;
	public KeyCode photoCapture;
	public int resWidth = 480;
	public int resHeight = 480;
	Camera cam;

	float minCameraFOV = 150;
	float maxCameraFOV = 10;
	float currentCameraFOV;
	public float defaultCameraFOV = 70;
	float scrollSpeed = 0.5f;

	void Start () {
		cam = GameObject.Find ("Polaroid").GetComponent<Camera> ();
		currentCameraFOV = defaultCameraFOV;
	}

	public static string GetPhotoName(int w, int h) {
		return string.Format ("{0}/photos/{1}x{2}_{3}.png",
			Application.persistentDataPath, w, h, System.DateTime.Now.ToString ("yyyyMMdd-HHmmss"));
	}

	void Update () {
//		if (OpenCamera.cameraIsOpen) {
//			currentCameraFOV += Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
//			if (currentCameraFOV > minCameraFOV) {
//				currentCameraFOV = minCameraFOV;
//			} else if (currentCameraFOV < maxCameraFOV) {
//				currentCameraFOV = maxCameraFOV;
//			}
//
//			cam.fieldOfView = currentCameraFOV;
//		} else {
//			cam.fieldOfView = defaultCameraFOV;
//		}
	}
	
	void LateUpdate () {
		if (Input.GetKeyDown (photoCapture) && OpenCamera.cameraIsOpen) {
			takePhoto = true;
		} else {
			takePhoto = false;
		}
	}

	void OnGUI () {
		if (takePhoto) {
			RenderTexture rt = new RenderTexture (resWidth, resHeight, 24);
			cam.targetTexture = rt;
			cam.Render ();
			RenderTexture.active = rt;
			Texture2D photo = new Texture2D (resWidth, resHeight, TextureFormat.RGB24, false);
			photo.ReadPixels (new Rect (0, 0, resWidth, resHeight), 0, 0);
			Destroy (rt);

			byte[] bytes = photo.EncodeToPNG ();
			string filename = GetPhotoName (resWidth, resHeight);
			System.IO.File.WriteAllBytes (filename, bytes);
			Debug.Log ("Took photo to: " + filename);
			takePhoto = false;
		}
	}
}
