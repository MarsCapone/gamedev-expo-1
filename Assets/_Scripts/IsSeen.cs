using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IsSeen : MonoBehaviour {
	// do something if this object is seen by a camera

	public Camera cam = null;
	public TakePhoto takePhoto;

	private float margin = -0.2f;
	private float lowerMargin;
	private float upperMargin;
	private bool onScreen;


	// Use this for initialization
	void Start () {
		if (cam == null) {
			cam = GameObject.Find ("Polaroid").GetComponent<Camera> ();
		}

		lowerMargin = 0 - margin;
		upperMargin = 1 + margin;

	}
	
	void Update () {
		Vector3 screenPoint = cam.WorldToViewportPoint (gameObject.transform.position);
		onScreen = screenPoint.x > lowerMargin && screenPoint.y > lowerMargin &&
		           screenPoint.x < upperMargin && screenPoint.y < upperMargin &&
		           screenPoint.z > 0;
	}

	void OnGUI () {
		if (onScreen && TakePhoto.takePhoto) {
			Debug.Log (gameObject.name + " is in the photo.");
			takePhoto.DoCaptureScreen(gameObject.name); 
		} else if (TakePhoto.takePhoto) {
			takePhoto.DoCaptureScreen ("other");
			Debug.Log ("A scenic photo was taken.");
		}
	}
}
