using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class TakePhoto : MonoBehaviour
{

	private const int PHOTO_SIZE = 600;

	public static bool takePhoto = false;
	public KeyCode photoCapture;
	public AudioClip shutterSound;
	public Camera cam;
	public GameObject overlay;

	private AudioSource audioSource;


	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void FixedUpdate ()
	{
		if ((Input.GetKeyDown (photoCapture) || Input.GetMouseButtonDown (0)) && OpenCamera.cameraIsOpen) {
			Logging.Info ("Take a photo this frame.");
			takePhoto = true;
			PlayShutterSound ();
		} else {
			takePhoto = false;
		}
	}


	public Sprite CaptureScreen ()
	{
		overlay.SetActive (false);

		int size = PHOTO_SIZE;

		RenderTexture rt = new RenderTexture (size, size, 24);
		cam.targetTexture = rt;
		cam.Render ();
		RenderTexture.active = rt;

		Texture2D photo = new Texture2D (size, size, TextureFormat.RGB24, false);
		photo.ReadPixels (new Rect (0, 0, size, size), 0, 0);
		photo.Apply ();

		RenderTexture.active = null;

		cam.targetTexture = null;
		Destroy (rt);

		TakePhoto.takePhoto = false;

		overlay.SetActive (true);

		Sprite sprite = Sprite.Create (photo, new Rect (0, 0, photo.width, photo.height), new Vector2 (0, 0), 100f);
		Logging.Info ("Saving pixels on screen to a Sprite object.");
		return sprite;
	}

	private void PlayShutterSound ()
	{
		Logging.Info ("Play the camera shutter sound.");
		audioSource.mute = false;
		audioSource.clip = shutterSound;
		audioSource.Play ();
		audioSource.mute = true;
	}
}
