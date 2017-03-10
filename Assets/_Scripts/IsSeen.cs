using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSeen : MonoBehaviour
{

	public TakePhoto takePhoto;
	private HashSet<string> registeredCreatures;
	private Sprite image;

	// Use this for initialization
	void Start ()
	{
		registeredCreatures = new HashSet<string> ();
		Logging.Info ("Initialising photo registerer.");
	}

	void LateUpdate ()
	{
		Process ();
	}

	public void Process ()
	{
		if (TakePhoto.takePhoto) {

			Logging.Info ("Saving current photo.");
			Sprite image = takePhoto.CaptureScreen ();

			// nothing has been seen, but a photo has been taken, so it is a scenic photo
			if (registeredCreatures.Count == 0) {
				Logging.Debug ("Add scenic image to static ViewCreatures.");
				ViewCreatures.AddCreatureImage ("scenic", image);
			} else {
				// there are some photos of some creatures
				Logging.Debug (string.Format ("Adding creatures to journal for current frame. [registeredCreatures {0}]", registeredCreatures.Count));
				foreach (string creature in registeredCreatures) {
					Logging.Debug (string.Format ("Adding sprite image for {0} to static ViewCreatures.", creature));
					ViewCreatures.AddCreatureImage (creature, image);
				}
			}

			// clear out everything from this frame
			registeredCreatures.Clear ();
		}
	}

	public void RegisterCreaturePhoto (string creatureName)
	{
		registeredCreatures.Add (creatureName.ToLower ());
	}
}
