using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewCreatures : MonoBehaviour {

	private string current = null;

	public SlideChanger slideShow;
	public Image mainImage;
	public Text mainText;
	public Text description;

	public static Dictionary<string, List<Sprite>> mainImages = new Dictionary<string, List<Sprite>> ();
	public static int foundCreatures = 0;
	
	// Update is called once per frame
	void Update () {
		if (current == null) {
			description.gameObject.SetActive (false);
			mainText.gameObject.SetActive (true);

			// try to find an object that has images
			foreach (string creature in mainImages.Keys) {
				List<Sprite> creatureImages = mainImages [creature];
				if (creatureImages.Count > 0) {
					current = creature;
				}
				break;
			}
			if (current == null) {
				mainImage.sprite = null;
				mainText.text = "None selected";
			}
		} else {
			description.gameObject.SetActive (true);
			description.text = Capitalize(current);
			if (!mainImages.ContainsKey (current)) {
				mainImages.Add (current, new List<Sprite> ());
			}

			List<Sprite> sprites = mainImages [current];
			if (sprites.Count == 0) { // no photos taken yet
				mainText.gameObject.SetActive (true);
				mainText.text = "None found";
				mainImage.sprite = null;
			} else {
				mainText.gameObject.SetActive (false);
				// show the last photo taken
				mainImage.sprite = sprites [sprites.Count - 1];
			}
		}
	}

	public void ShowCreature () {
		if (current == null) {
			return;
		}

		if (mainImages.ContainsKey(current)) {
			slideShow.ShowSlides(mainImages[current]);
		} else {
			return;
		}
	}

	public void SetCurrentCreature(string creature) {
		creature = creature.ToLower ();
		current = creature;
	}

	public static void AddCreatureImage(string name, Sprite sprite) {
		name = name.ToLower ();
		if (!mainImages.ContainsKey (name)) {
			mainImages.Add (name, new List<Sprite> ());
		}

		if (mainImages [name].Count == 0) {
			foundCreatures += 1; // finding a new creature
		}
		mainImages [name].Add (sprite);
	}

	private string Capitalize(string input) {
		return char.ToUpper (input [0]) + input.Substring (1);
	}
}
