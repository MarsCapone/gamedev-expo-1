using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunIsSeen : MonoBehaviour, IsSeenableEvent {

	public TakePhoto takePhoto;
	public string photoName;
	public TimeController timeController;
	public float[] times;
	public int timeMargin;
	public Camera cam;

	private const float lowerMargin = 0.2f;
	private const float upperMargin = 0.8f;

	protected bool onScreen;

	// Update is called once per frame
	void Update () {
		onScreen = IsOnScreen ();
	}

	public bool IsOnScreen () {
		Vector3 screenPoint = cam.WorldToViewportPoint (gameObject.transform.position);
		bool inDisplay = screenPoint.x > lowerMargin && screenPoint.y > lowerMargin &&
			screenPoint.x < upperMargin && screenPoint.y < upperMargin &&
			screenPoint.z > 0;


		float currentTime = timeController.GetTime ();
		bool correctTime = true;
		foreach (float t in times) {
			correctTime |= (currentTime + timeMargin >= t && currentTime - timeMargin <= t);
		}
		return correctTime && inDisplay;
	}


	void OnGUI () {
		if (onScreen && TakePhoto.takePhoto) {
			print ("taking photo of " + photoName);
			takePhoto.DoCaptureScreen (photoName);
		}
	}
}
