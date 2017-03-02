using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenJournal : MonoBehaviour {

	public GameObject mainRoot;
	public GameObject journalRoot;

	public static bool journalIsOpen = false;
	bool prevState = false;
	public KeyCode toggle; 

	// Update is called once per frame
	void Update () {
		prevState = journalIsOpen;
		if (Input.GetKeyDown (toggle) && !OpenCamera.cameraIsOpen) {
			journalIsOpen = !journalIsOpen;
		}
	}

	void LateUpdate () {
		if (journalIsOpen && prevState != journalIsOpen && !OpenCamera.cameraIsOpen) {
			Open ();
		} else if (!journalIsOpen && prevState != journalIsOpen) {
			Close ();
		}
	}

	public void Close () {
		mainRoot.gameObject.SetActive (true);
		journalRoot.gameObject.SetActive (false);
		Time.timeScale = 1;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void Open () {
		mainRoot.gameObject.SetActive (false);
		journalRoot.gameObject.SetActive (true);
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
