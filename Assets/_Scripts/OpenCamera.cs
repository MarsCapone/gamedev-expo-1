using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class OpenCamera : MonoBehaviour
{

	public static bool cameraIsOpen = false;
	public KeyCode toggle;

	Camera mainCam;
	Camera photoCam;
	Canvas photoCanvas;
	Canvas iconCanvas;
	FirstPersonController player;

	// Use this for initialization
	void Start ()
	{
		mainCam = GameObject.Find ("Character").GetComponent<Camera> ();
		photoCam = GameObject.Find ("Polaroid").GetComponent<Camera> ();
		photoCanvas = GameObject.Find ("PhotoCameraCanvas").GetComponent<Canvas> ();
		iconCanvas = GameObject.Find ("IconCanvas").GetComponent<Canvas> ();
		player = GameObject.Find ("Player").GetComponent<FirstPersonController> ();

		photoCanvas.gameObject.SetActive (false);
		photoCam.gameObject.SetActive (false);


		Logging.Info ("Camera opener initialised.");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (toggle) && !OpenJournal.journalIsOpen) {
			cameraIsOpen = !cameraIsOpen;

			if (cameraIsOpen) {
				EnablePhotoCamera ();
			} else {
				DisablePhotoCamera ();		
			}
		}
	}

	void EnablePhotoCamera ()
	{
		Logging.Info ("Opening camera.");
		// stop the player from moving
		cameraIsOpen = true;
		player.m_WalkSpeed = 0;
		player.m_RunSpeed = 0;
		player.m_GravityMultiplier = 100;

		photoCam.gameObject.SetActive (true); // activate the photo camera

		photoCam.transform.localPosition = mainCam.transform.localPosition;
		photoCam.transform.localRotation = mainCam.transform.localRotation;

		mainCam.gameObject.SetActive (false); // deactivate the player camera
		photoCanvas.gameObject.SetActive (true); // show the viewfinder
		iconCanvas.gameObject.SetActive (false); // hide the icons
	}


	void DisablePhotoCamera ()
	{
		Logging.Info ("Closing camera.");
		cameraIsOpen = false;
		player.m_WalkSpeed = 5;
		player.m_RunSpeed = 10;
		player.m_GravityMultiplier = 2;

		photoCam.gameObject.SetActive (false); // deactivate the photo camera

		mainCam.transform.localPosition = photoCam.transform.localPosition;
		mainCam.transform.localRotation = photoCam.transform.localRotation;

		mainCam.gameObject.SetActive (true); // activate the player camera
		photoCanvas.gameObject.SetActive (false); // hide the viewfinder
		iconCanvas.gameObject.SetActive (true); // show the icons
	}
}
