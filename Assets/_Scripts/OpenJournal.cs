using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenJournal : MonoBehaviour {

	public static bool journalIsOpen = false;
	bool prevState = false;
	public KeyCode toggle; 

	// Update is called once per frame
	void Update () {
		prevState = journalIsOpen;
		if (Input.GetKeyDown (toggle)) {
			journalIsOpen = !journalIsOpen;
		}
	}

	void OnGUI () {
		if (journalIsOpen && prevState != journalIsOpen) {
			Debug.Log ("The journal has just been opened.");
		} else if (!journalIsOpen && prevState != journalIsOpen) {
			Debug.Log ("The journal has just been closed.");
		}
	}
}
