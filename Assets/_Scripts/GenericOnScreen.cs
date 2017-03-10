using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericOnScreen : IsOnScreenable
{

	public Camera cam;

	protected const float lowerMargin = 0.2f;
	protected const float upperMargin = 0.8f;

	public override bool IsOnScreen ()
	{
		Vector3 screenPoint = cam.WorldToViewportPoint (gameObject.transform.position);
		bool inDisplay = screenPoint.x > lowerMargin && screenPoint.y > lowerMargin &&
		                 screenPoint.x < upperMargin && screenPoint.y < upperMargin &&
		                 screenPoint.z > 0;
		return inDisplay; 
	}
}
