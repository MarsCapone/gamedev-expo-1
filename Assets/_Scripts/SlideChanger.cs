using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideChanger : MonoBehaviour {


	public KeyCode nextSlide = KeyCode.RightArrow;
	public KeyCode prevSlide = KeyCode.LeftArrow;
	public KeyCode escapeSlide = KeyCode.Escape;

	private Canvas slideShowCanvas;
	private Image currentImage;
	private int selectedImagesIndex = 0;
	private List<Sprite> imagePaths;


	// Use this for initialization
	void Start () {
		slideShowCanvas = GameObject.Find ("SlideShowCanvas").GetComponent<Canvas> ();
		currentImage = GameObject.Find ("CurrentImage").GetComponent<Image> ();
	}
	
	// Hide the slides for the currently activated image.
	void DisableAnimalSlide () {
		slideShowCanvas.gameObject.SetActive (false);
		Journal.activeCreature = null;
		Journal.showSlide = false;
	}

	void Update () {
		if (Journal.showSlide) {
			HandleChangeSlide ();
			imagePaths = Journal.mainImages [Journal.activeCreature.name];
			if (imagePaths.Count == 0) {
				// we should not be trying to show anything
				DisableAnimalSlide();
				return;
			}
			currentImage.sprite = imagePaths [selectedImagesIndex];
		}
	}

	void HandleChangeSlide () {
		if (Input.GetKeyDown (nextSlide)) {
			NextSlide ();
		} else if (Input.GetKeyDown (prevSlide)) {
			PrevSlide ();
		} else if (Input.GetKeyDown (escapeSlide)) {
			DisableAnimalSlide ();
		}
	}

	void NextSlide () {
		selectedImagesIndex += 1;
		selectedImagesIndex = Mathf.Clamp (selectedImagesIndex, 0, imagePaths.Count);
	}

	void PrevSlide () {
		selectedImagesIndex -= 1;
		selectedImagesIndex = Mathf.Clamp (selectedImagesIndex, 0, imagePaths.Count);
	}

}
