using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimebasedIsSeen : MonoBehaviour, IsSeenableEvent {

	public TimeController timeController;
	public float[] times;
	public int margin;
	public Camera cam;

	protected bool onScreen;
	
	// Update is called once per frame
	void Update () {
		onScreen = IsOnScreen ();
	}

	public abstract bool IsOnScreen ();
}
