using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{

	public Light flash1;
	public Light flash2;
	public Light flash3;
	public Light flash4;
	public int framesToFade;

	public const int MAX_LIGHT = 8;
	public const int MIN_LIGHT = 0;

	public static bool doFlash = false;

	private int totalIntensity;
	private bool runningFlash = false;
	private int currentLevel;
	private int jumpPerFrame;

	// Use this for initialization
	void Start ()
	{
		flash1.intensity = 0;
		flash2.intensity = 0;
		flash3.intensity = 0;
		flash4.intensity = 0;

		runningFlash = false;
		totalIntensity = 32;
		jumpPerFrame = (int)totalIntensity / framesToFade;

		Logging.Info ("Camera flash initialised.");
	}

	void LateUpdate ()
	{
		// this should happen the frame after a photo is taken
		// so actual photo doesn't have the flash in it
		if (doFlash) {
			Logging.Debug ("Starting camera flash.");
			DoFlash ();
		}

		// the end of a flash
		if (flash4.intensity == 0) {
			runningFlash = false;
		}

		// logic behind a flash happening 
		// basically turn down lighs incrementally
		if (runningFlash) {
			currentLevel = currentLevel - jumpPerFrame;
			if (currentLevel > 3 * 8) {
				flash1.intensity = currentLevel % 8;
			} else if (currentLevel > 2 * 8) {
				flash1.intensity = 0;
				flash2.intensity = currentLevel % 8;
			} else if (currentLevel > 1 * 8) {
				flash1.intensity = 0;
				flash2.intensity = 0;
				flash3.intensity = currentLevel % 8;
			} else if (currentLevel > 0) {
				flash1.intensity = 0;
				flash2.intensity = 0;
				flash3.intensity = 0;
				flash4.intensity = currentLevel % 8;
			} else {
				runningFlash = false;
			}
		}

		if (TakePhoto.takePhoto) {
			Logging.Debug ("Prepping to do camera flash on next frame.");

			// switch off the light in preparation for an imminent photo
			SetMinLight ();
			doFlash = true;
			runningFlash = false;
		}
	}

	void DoFlash ()
	{
		runningFlash = true;
		doFlash = false;
		currentLevel = totalIntensity;

		// turn the flash on
		SetMaxLight ();
	}

	void SetMaxLight ()
	{
		flash1.intensity = MAX_LIGHT;
		flash2.intensity = MAX_LIGHT;
		flash3.intensity = MAX_LIGHT;
		flash4.intensity = MAX_LIGHT;
	}

	void SetMinLight ()
	{
		flash1.intensity = MIN_LIGHT;
		flash2.intensity = MIN_LIGHT;
		flash3.intensity = MIN_LIGHT;
		flash4.intensity = MIN_LIGHT;
	}
}
