using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunEvent : TimebasedOnScreen
{
	void Start ()
	{
		Logging.Info (string.Format ("{0} event initialised.", seenName));
	}

	public override bool IsVisible ()
	{
		return IsWithinViewport ();
	}
}
