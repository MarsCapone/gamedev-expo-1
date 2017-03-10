using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericOnScreen : IsOnScreenable
{

	public override bool IsOnScreen ()
	{
		return IsWithinViewport ();
	}
}
