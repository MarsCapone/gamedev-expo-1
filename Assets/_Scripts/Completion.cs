using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Completion : MonoBehaviour {

	private Text completionText;

	// Use this for initialization
	void Start () {
		completionText = gameObject.GetComponent<Text> ();
		completionText.text = string.Format ("{0}/{1}", 0, Seeable.seeableCreatures.Count);
	}
	
	// Update is called once per frame
	void Update () {
		completionText.text = string.Format ("{0}/{1}", ViewCreatures.foundCreatures, Seeable.seeableCreatures.Count);
	}
}
