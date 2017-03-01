using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Journal : MonoBehaviour {

	Canvas slideShowCanvas;
	Canvas journalCanvas;

	public GameObject[] seeableCreatures;
	public int creatureCount { get { return seeableCreatures.Length; } }

	public GameObject photoSpaces = null;

	public Image leftPageLeftImage;
	public Image leftPageRightImage;
	public Image rightPageLeftImage;
	public Image rightPageRightImage;

	private Text lpliText, lpriText, rpliText, rpriText;
	private int creaturesPerPage = 2;
	private int pageCount;
	private int currentPage = 0;

	public static GameObject activeCreature = null;
	public static bool showSlide = false;
	public static Dictionary<string, List<Sprite>> mainImages = new Dictionary<string, List<Sprite>> ();
	public static Dictionary<string, List<Sprite>> thumbnailImages = new Dictionary<string, List<Sprite>> ();

	// Use this for initialization
	void Start () {
		pageCount = (int) 1 + (creatureCount / creaturesPerPage);
		slideShowCanvas = GameObject.Find ("SlideShowCanvas").GetComponent<Canvas> ();
		journalCanvas = GameObject.Find ("JournalCanvas").GetComponent<Canvas> ();

		if (photoSpaces == null) {
			photoSpaces = GameObject.Find ("PhotoSpaces");
		}

		slideShowCanvas.gameObject.SetActive (false);
		journalCanvas.gameObject.SetActive (true);

		// get text descriptions for images
		lpliText = leftPageLeftImage.GetComponent<Text> ();
		lpriText = leftPageRightImage.GetComponent<Text> ();
		rpliText = rightPageLeftImage.GetComponent<Text> ();
		rpriText = rightPageRightImage.GetComponent<Text> ();


		// add "other" to seeable creatures for scenic photos
		GameObject[] tmpCreatures = new GameObject[seeableCreatures.Length + 1];
		seeableCreatures.CopyTo (tmpCreatures, 0);
		GameObject otherPhotos = new GameObject ();
		otherPhotos.name = "other";
		tmpCreatures [tmpCreatures.Length] = otherPhotos;
		seeableCreatures = tmpCreatures;

		foreach (GameObject go in seeableCreatures) {
			mainImages.Add (go.name, new List<Sprite> ());
			thumbnailImages.Add (go.name, new List<Sprite> ());
		}
	}

	// Update is called once per frame
	void Update () {
		LoadCurrentImages ();
	}

	void LoadCurrentImages () {
		// 4 images per spread, so images available are the seeableCreatures[pageIndex*4:pageIndex*4 + 4] 
		int index = currentPage * 4;
		LoadCurrentImage (leftPageLeftImage, lpliText, index + 0);
		LoadCurrentImage (leftPageRightImage, lpriText, index + 1);
		LoadCurrentImage (rightPageLeftImage, lpliText, index + 2);
		LoadCurrentImage (rightPageRightImage, lpliText, index + 3);
	}


	// When a specific animal image is shown, enable a slide show 
	// for that animal with the previously taken photos.
	public void EnableAnimalSlide (GameObject creature) {
		slideShowCanvas.gameObject.SetActive (true);
		showSlide = true;
		activeCreature = creature;
	}

	private void LoadCurrentImage (Image im, Text text, int seeableIndex) {
		if (seeableIndex < seeableCreatures.Length && seeableIndex >= 0) {
			// get the images for that creature from the index in what is supposed o be seeable
			string c = seeableCreatures [seeableIndex].name;
			if (thumbnailImages.ContainsKey (c)) {
				List<Sprite> creatureImages = thumbnailImages [c];
				if (creatureImages.Count > 0) {
					text.gameObject.SetActive (false);
					im.sprite = creatureImages [0];
				} else {
					text.gameObject.SetActive (true);
					text.text = seeableCreatures [seeableIndex].name;
				}
			}
		} 
	}

	public GameObject GetImageGameObject(int index) {
		return seeableCreatures [currentPage * 4 + index];
	}
}
