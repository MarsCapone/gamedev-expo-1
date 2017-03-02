using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewCreatures : MonoBehaviour {

	private GameObject current = null;

	public SlideChanger slideShow;
	public Image mainImage;
	public Text mainText;
	public Text description;

	public static Dictionary<string, List<Sprite>> mainImages = new Dictionary<string, List<Sprite>> ();
	
	// Update is called once per frame
	void Update () {
		if (current == null) {
			description.gameObject.SetActive (false);
			mainText.gameObject.SetActive (true);
			mainImage.sprite = null;
			mainText.text = "None selected";
		} else {
			description.gameObject.SetActive (true);
			description.text = current.name;
			if (!mainImages.ContainsKey (current.name)) {
				mainImages.Add (current.name, new List<Sprite> ());
			}

			List<Sprite> sprites = mainImages [current.name];
			if (sprites.Count == 0) { // no photos taken yet
				mainText.gameObject.SetActive (true);
				mainText.text = "None found";
				mainImage.sprite = null;
			} else {
				mainText.gameObject.SetActive (false);
				mainImage.sprite = sprites [0];
			}
		}
	}

	public void ShowCreature () {
		if (current == null) {
			return;
		}

		if (mainImages.ContainsKey(current.name)) {
			slideShow.ShowSlides(mainImages[current.name]);
		} else {
			return;
		}
	}

	public void SetCurrentCreature(GameObject go) {
		current = go;
	}

	public static void AddCreatureImage(string name, Sprite sprite) {
		if (!mainImages.ContainsKey (name)) {
			mainImages.Add (name, new List<Sprite> ());
		}
		mainImages [name].Add (sprite);
	}
}
