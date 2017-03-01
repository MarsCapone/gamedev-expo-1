using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TakePhoto : MonoBehaviour {

	private const string CAMERA_NAME = "Polaroid";

	public static bool takePhoto = false;
	public KeyCode photoCapture;
	public AudioClip shutterSound;

	public static int photoSize = 200;
	public static string photoDirectory = "/photos";
	public static int thumbnailSize = 600;

	private AudioSource audioSource;


	void Start () {
		// create the directory where the photos are going to go, otherwise there is an error
		Directory.CreateDirectory (string.Format ("{0}/thumbnails", photoDirectory)); 
		audioSource = GetComponent<AudioSource> ();
	}

	public static string GetPhotoName(string name) {
		return string.Format ("{0}/{1}.png",
			photoDirectory, name);
	}

	public static string GetThumbnailName(string name) {
		return string.Format ("{0}/thumbnails/{1}.png",
			photoDirectory, name);
	}
	
	void LateUpdate () {
		if ((Input.GetKeyDown (photoCapture) || Input.GetMouseButtonDown(0)) && OpenCamera.cameraIsOpen) {
			takePhoto = true;
		} else {
			takePhoto = false;
		}
	}

	void OnGUI () {
		if (takePhoto) {
			PlayShutterSound ();
			// do this is IsSeen instead
			//DoCaptureScreen ("default");
		}
	}
		
	public static Sprite CaptureScreen (int size, GameObject go) {
		Camera cam = GameObject.Find (CAMERA_NAME).GetComponent<Camera> ();

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

	public static void DoCaptureScreen (GameObject go) {
		Sprite photo = CaptureScreen (photoSize, go);
		Sprite thumnail = CaptureScreen (thumbnailSize, go);

		List<Sprite> spriteList;

		if (Journal.mainImages.ContainsKey (go.name)) {
			spriteList = Journal.mainImages [go.name];
		} else {
			spriteList = new List<Sprite> ();
		}
		spriteList.Add (photo);
		Journal.mainImages [go.name] = spriteList;
	

		if (Journal.thumbnailImages.ContainsKey (go.name)) {
			spriteList = Journal.mainImages [go.name];
		} else {
			spriteList = new List<Sprite> ();
		}
		spriteList.Add (photo);
		Journal.thumbnailImages [go.name] = spriteList;
	}

	void PlayShutterSound () {
		audioSource.clip = shutterSound;
		audioSource.Play ();
	}
}
