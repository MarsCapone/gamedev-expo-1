using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrack : MonoBehaviour {

	public TimeController timeController;
	public Sunlight sunlight;
	public AudioClip[] daytimeSounds;
	public AudioClip[] nighttimeSounds;

	private List<AudioSource> daytimeSources = new List<AudioSource> ();
	private List<AudioSource> nighttimeSources = new List<AudioSource> ();

	private float sunrise;
	private float sunset;

	void Awake () {
		foreach (AudioClip ac in daytimeSounds) {
			AddAudio (true, ac, true, true);
		}

		foreach (AudioClip ac in nighttimeSounds) {
			AddAudio (false, ac, true, true);
		}
	}

	void Start () {
		sunrise = sunlight.sunriseStartTime;
		sunset = sunrise + 12;
	}
	
	// Update is called once per frame
	void Update () {
		float time = timeController.GetTime ();

		if (time > sunrise && time < sunset) {
			// daytime, disable nighttime sounds
			DisbableAll (nighttimeSources);
			EnableAll (daytimeSources);
		} else {
			// disable daytime sounds
			DisbableAll (daytimeSources);
			EnableAll (nighttimeSources);
		}
		
	}

	private void DisbableAll(List<AudioSource> l) {
		foreach (AudioSource s in l) {
			s.mute = true;
		}
	}

	private void EnableAll(List<AudioSource> l) {
		foreach (AudioSource s in l) {
			s.mute = false;
		}
	}

	private void AddAudio(bool day, AudioClip clip, bool loop, bool onWake) {
		AudioSource s = gameObject.AddComponent<AudioSource> ();
		s.clip = clip;
		s.loop = loop;
		s.playOnAwake = onWake;

		if (day) {
			daytimeSources.Add (s);
		} else {
			nighttimeSources.Add (s);
		}
	}
}
