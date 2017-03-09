using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TakePhoto : MonoBehaviour {

	private const int PHOTO_SIZE = 600;

	public static bool takePhoto = false;
	public KeyCode photoCapture;
	public AudioClip shutterSound;
	public Camera cam;
	public GameObject overlay;

	private AudioSource audioSource;


	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	void Update () {
		if ((Input.GetKeyDown (photoCapture) || Input.GetMouseButtonDown(0)) && OpenCamera.cameraIsOpen) {
			print ("Taking a photo.");
			takePhoto = true;
		} else {
			takePhoto = false;
		}
	}

	void OnGui () {
		PlayShutterSound ();
	}
		
	public Sprite CaptureScreen () {
		int size = PHOTO_SIZE;

		RenderTexture rt = new RenderTexture (size, size, 24);
		cam.targetTexture = rt;
		cam.Render ();
		RenderTexture.active = rt;

		Texture2D photo = new Texture2D (size, size, TextureFormat.RGB24, false);
		photo.ReadPixels (new Rect (0, 0, size, size), 0, 0);
		photo.Apply ();

		cam.targetTexture = null;
		Destroy (rt);

		TakePhoto.takePhoto = false;

		Sprite sprite;
		sprite = Sprite.Create (photo, new Rect (0, 0, photo.width, photo.height), new Vector2 (0, 0), 100f);
		return sprite;
	}

	public void DoCaptureScreen (string name) {
		overlay.SetActive (false); // don't capture the overlay in the image
		Sprite photo = CaptureScreen ();
		overlay.SetActive (true);
		ViewCreatures.AddCreatureImage (name, photo);
	}

	private void PlayShutterSound () {
		audioSource.clip = shutterSound;
		audioSource.Play ();
	}
}
