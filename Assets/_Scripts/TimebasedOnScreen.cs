using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimebasedOnScreen : IsOnScreenable
{
	public TimeController timeController;
	public float eventTime;
	public float margin = 0.5f;

	void Start ()
	{
		Logging.Info (string.Format ("{0} initialised as something that can be on screen and is time based.", seenName));
	}

	public abstract bool IsVisible ();

	public override bool IsOnScreen ()
	{
		return IsWithinTime () && IsVisible ();
	}

	// USEFUL FUNCTIONS

	protected bool IsWithinTime ()
	{
		float currentTime = timeController.GetTime ();
		bool inTimeBoundary = currentTime + margin >= eventTime && currentTime - margin <= eventTime;
		return inTimeBoundary;
	}
}
