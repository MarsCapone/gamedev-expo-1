﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

	public Camera cam;
	public float minFov;
	public float maxFov;
	public float defaultFov = 70f;
	//public float sensitivity = 10f;
	public KeyCode increaseZoom = KeyCode.Q;
	public KeyCode decreaseZoom = KeyCode.E;
	public KeyCode resetZoom = KeyCode.R;

	private float fov;
	
	// Update is called once per frame
	void Update ()
	{
		if (OpenCamera.cameraIsOpen) {
			fov = cam.fieldOfView;
			if (Input.GetKeyDown (increaseZoom)) {
				Logging.Info ("Increasing camera zoom.");
				fov += 5;
			} else if (Input.GetKeyDown (decreaseZoom)) {
				Logging.Info ("Decreasing camera zoom."); 
				fov -= 5;
			} else if (Input.GetKeyDown (resetZoom)) {
				Logging.Info ("Resetting camera zoom.");
				fov = defaultFov;
			}
			fov = Mathf.Clamp (fov, minFov, maxFov);
			cam.fieldOfView = fov;
		} else {
			cam.fieldOfView = defaultFov;
		}
	}
}
