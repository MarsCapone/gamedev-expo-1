using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

	public Camera camera;
	public float minFov;
	public float maxFov;
	public float defaultFov = 70f;
	//public float sensitivity = 10f;
	public KeyCode increaseZoom = KeyCode.Q;
	public KeyCode decreaseZoom = KeyCode.E;
	public KeyCode resetZoom = KeyCode.R;

	private float fov;
	
	// Update is called once per frame
	void Update () {
		if (OpenCamera.cameraIsOpen) {
			fov = camera.fieldOfView;
			if (Input.GetKeyDown (increaseZoom)) {
				fov += 5;
			} else if (Input.GetKeyDown (decreaseZoom)) {
				fov -= 5;
			} else if (Input.GetKeyDown (resetZoom)) {
				fov = defaultFov;
			}
			fov = Mathf.Clamp (fov, minFov, maxFov);
			camera.fieldOfView = fov;
		} else {
			camera.fieldOfView = defaultFov;
		}
	}
}
