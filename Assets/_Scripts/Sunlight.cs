using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunlight : MonoBehaviour, ITimeBasedObject
{

	private Light lightComponent;

	public float whiteLightTime, yellowLightTime, redLightTime, sunriseStartTime;

	// Use this for initialization
	void Start ()
	{
		lightComponent = this.GetComponent<Light> ();
		Logging.Info ("Initialising sunlight.");
	}

	public void updateTime (float time)
	{
		float anglePerHour = 360F / 24F;
		float directionalLightAngle = (anglePerHour * time) - 90;
		transform.eulerAngles = new Vector3 (directionalLightAngle % 360, 90, 0);
		//set colour based on time of day
		float timeDistanceFromMidnight = time < 12 ? time : 24 - time;
		Color lightColor;
		if (timeDistanceFromMidnight > whiteLightTime) {
			lightColor = new Color (1.0F, 1.0F, 1.0F);
		} else if (timeDistanceFromMidnight > yellowLightTime) { //sunset: white to yellow
			lightColor = new Color (1.0F, 1.0F, (timeDistanceFromMidnight - yellowLightTime) / (whiteLightTime - yellowLightTime));
		} else if (timeDistanceFromMidnight > redLightTime) { //sunset: yellow to red
			lightColor = new Color (1.0F, (timeDistanceFromMidnight - redLightTime) / (yellowLightTime - redLightTime), 0.0F);
		} else if (timeDistanceFromMidnight > sunriseStartTime) { //fade to black
			lightColor = new Color ((timeDistanceFromMidnight - sunriseStartTime) / (redLightTime - sunriseStartTime), 0, 0);
		} else {
			lightColor = new Color (0, 0, 0);
		}
		if (lightComponent != null) { //prevents crash when time updated before start() is called
			lightComponent.color = lightColor;
			lightComponent.intensity = timeDistanceFromMidnight < sunriseStartTime ? 0 : (timeDistanceFromMidnight - sunriseStartTime) / (12 - sunriseStartTime) * 1.5F;
		}
	}

	public void newDay ()
	{
	}
}
