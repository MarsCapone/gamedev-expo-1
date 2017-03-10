using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IsOnScreenable : MonoBehaviour
{
	public IsSeen isSeen;
	public string seenName;
	public bool isOnScreen = false;

	void Start ()
	{
		Logging.Info (string.Format ("{0} initialised as something that can be on screen.", seenName));
	}

	void Update ()
	{
		isOnScreen = IsOnScreen ();
		//Logging.Debug (string.Format ("{0} is on screen.", seenName));

		if (TakePhoto.takePhoto && isOnScreen) {
			Logging.Info (string.Format ("{0} has been seen in a photo. Registering for journal...", seenName));
			isSeen.RegisterCreaturePhoto (seenName);
		}
	}

	public abstract bool IsOnScreen ();
}