using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenJournal : MonoBehaviour {

	public static bool journalIsOpen = false;
	bool prevState = false;
	public KeyCode toggle; 

	public GameObject journalRoot;
	public GameObject root;

	// Update is called once per frame
	void Update () {
		prevState = journalIsOpen;
		if (Input.GetKeyDown (toggle)) {
			journalIsOpen = !journalIsOpen;
		}
	}

	void OnGUI () {
		if (journalIsOpen && prevState != journalIsOpen) {
//			SceneManager.UnloadSceneAsync ("Main");
//			SceneManager.LoadScene ("Journal");
			journalRoot.SetActive(true);
			root.SetActive (false);
			Journal.showSlide = false;
		} else if (!journalIsOpen && prevState != journalIsOpen && !Journal.showSlide) {
//			SceneManager.LoadScene ("Main");
//			SceneManager.UnloadSceneAsync ("Journal");
			journalRoot.SetActive(false);
			root.SetActive (true);
		}
	}
}
