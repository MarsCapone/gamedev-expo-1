using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TakePhoto : MonoBehaviour {

	public static bool takePhoto = false;
	public KeyCode photoCapture;
	public AudioClip shutterSound;
	public int resWidth = 480;
	public int resHeight = 480;

	private Camera cam;
	private AudioSource audioSource;


	void Start () {
		cam = GameObject.Find ("Polaroid").GetComponent<Camera> ();
		audioSource = GetComponent<AudioSource> ();
	}

	public static string GetPhotoName(int w, int h) {
		return string.Format ("{0}/photos/{1}x{2}_{3}.png",
			Application.persistentDataPath, w, h, System.DateTime.Now.ToString ("yyyyMMdd-HHmmss"));
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
			PlayShutterSound ();
			CaptureScreen ();
		}
	}
		
	void CaptureScreen () {

		RenderTexture rt = new RenderTexture (resWidth, resHeight, 24);
		cam.targetTexture = rt;
		cam.Render ();
		RenderTexture.active = rt;

		Texture2D photo = new Texture2D (resWidth, resHeight, TextureFormat.RGB24, false);
		photo.ReadPixels (new Rect (0, 0, resWidth, resHeight), 0, 0);
		photo.Apply ();

		byte[] bytes = photo.EncodeToPNG ();
		string filename = GetPhotoName (resWidth, resHeight);
		System.IO.File.WriteAllBytes (filename, bytes);
		//Debug.Log ("Took photo to: " + filename);
		takePhoto = false;

		cam.targetTexture = null;
		Destroy (rt);
	}

	void PlayShutterSound () {
		audioSource.clip = shutterSound;
		audioSource.Play ();
	}
}
