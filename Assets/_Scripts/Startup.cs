using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{

	public GameObject mainRoot;
	public GameObject journalRoot;

	// Use this for initialization
	void Start ()
	{
		Logging.Info ("Game Starting...");
		mainRoot.gameObject.SetActive (true);
		journalRoot.gameObject.SetActive (false);
	}
}
