using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideChanger : MonoBehaviour
{


	public KeyCode nextSlide = KeyCode.RightArrow;
	public KeyCode prevSlide = KeyCode.LeftArrow;
	public KeyCode escapeSlide = KeyCode.Escape;

	public GameObject hideWhile;
	public Image currentImage;
	public Text photoNumberText;

	private int selectedImagesIndex = 0;
	private List<Sprite> images;
	public static bool showSlides;

	// Use this for initialization
	void Start ()
	{
		showSlides = false;
		gameObject.SetActive (false);
		Logging.Info ("Initialising Slide Changer.");
	}
	
	// Hide the slides for the currently activated image.
	void DisableAnimalSlide ()
	{
		Logging.Info ("Leaving slide show.");
		hideWhile.gameObject.SetActive (true);
		gameObject.SetActive (false);
		showSlides = false;
		photoNumberText.text = "";
	}

	void Update ()
	{
		if (showSlides) {
			HandleChangeSlide ();
			currentImage.sprite = images [selectedImagesIndex];
			photoNumberText.text = string.Format ("{0}/{1}", selectedImagesIndex + 1, images.Count);
		}
	}

	public void ShowSlides (List<Sprite> images)
	{
		if (images.Count == 0) {
			DisableAnimalSlide ();
			return;
		}
		this.images = images;
		this.images.Reverse ();
		showSlides = true;
		hideWhile.gameObject.SetActive (false);
		gameObject.SetActive (true);
		photoNumberText.text = string.Format ("{0}/{1}", selectedImagesIndex + 1, images.Count);
		Logging.Info ("Opening slide show.");
	}

	void HandleChangeSlide ()
	{
		if (Input.GetKeyDown (nextSlide)) {
			NextSlide ();
		} else if (Input.GetKeyDown (prevSlide)) {
			PrevSlide ();
		} else if (Input.GetKeyDown (escapeSlide)) {
			Escape ();
		}
	}

	public void NextSlide ()
	{
		Logging.Info ("Moving to next photo in slide show.");
		selectedImagesIndex += 1;
		selectedImagesIndex = selectedImagesIndex % (images.Count);
	}

	public void PrevSlide ()
	{
		Logging.Info ("Moving to previous photo in slide show.");
		selectedImagesIndex -= 1;
		selectedImagesIndex = selectedImagesIndex % (images.Count);
	}

	public void Escape ()
	{
		DisableAnimalSlide ();
	}

}
