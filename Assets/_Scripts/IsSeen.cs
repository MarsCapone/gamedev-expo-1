using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IsSeen : MonoBehaviour {
	// do something if this object is seen by a camera

	public Camera cam = null;
	public string creatureName = null;

	private float margin = -0.2f;
	private float lowerMargin;
	private float upperMargin;
	private bool onScreen;

	private static Dictionary<string, int> capturedCountMap = new Dictionary<string, int> ();
	public static Dictionary<string, bool> discoveredCreatures = new Dictionary<string, bool> ();

	// Use this for initialization
	void Start () {
		if (cam == null) {
			cam = GameObject.Find ("Polaroid").GetComponent<Camera> ();
		}

		if (creatureName != null) {
			gameObject.name = creatureName;
		}

		lowerMargin = 0 - margin;
		upperMargin = 1 + margin;
		// if this script is starting, there any photos that are taken will be the first that are taken for the parent object
		capturedCountMap.Add (gameObject.name, 0);

		if (!capturedCountMap.ContainsKey ("other")) {
			capturedCountMap.Add ("other", 0);
		}

		// when the script starts, nothing has yet been photographed
		discoveredCreatures.Add (gameObject.name, false);
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
			if (capturedCountMap.ContainsKey(gameObject.name)) {
				// save as deer/img1, deer/img2, etc.
				TakePhoto.DoCaptureScreen(gameObject); 
				discoveredCreatures.Add (gameObject.name, true);
				capturedCountMap [gameObject.name] += 1;
			} else {
				Debug.Log("Somehow the name of this object has not yet been initialised.");
			}
		} else if (TakePhoto.takePhoto) {
			TakePhoto.DoCaptureScreen (gameObject);
			capturedCountMap ["other"] += 1;
			Debug.Log ("A scenic photo was taken.");
		}
	}
}
