using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{

	public GameObject cameraIcon, journalIcon, photoCameraInstructionsText, completionText;
	public GameObject tutorialAnimal;
	public GameObject player;
    
	//reason for loads of public variables like this: so that the objects can be dragged and dropped in unity
	public GameObject mouseIcon, upArrowIcon, leftArrowIcon, rightArrowIcon, downArrowIcon, wasdIcon, cameraKeyIcon, takePhotoIcon, openScrapbookIcon;
    
	public GameObject cameraLeftArrowIcon, cameraRightArrowIcon, cameraUpArrowIcon, cameraDownArrowIcon, exitCameraIcon;

	public GameObject titleScreen;

	private List<GameObject> visibleUIElements = new List<GameObject> ();
	private List<GameObject> invisibleUIElements = new List<GameObject> ();

	private float time;

	/*
	private float triggerTime = 0F;
	private TutorialStage triggerEvent = TutorialStage.nothing;

	/*
     * tutorial schedule:
     * -move the mouse to look around
     *      mouse icon and arrows UDLR
     * -wasd to walk
     *      W-S, A-D in that order (keys onscreen)
     * -c for camera
     *      bring camera icon onscreen
     * -take photo
     *      click mouse / press F icon
     *      look at the tutorial object through the viewfinder
     *      move mouse icon
     *      arrows on the screen directing the player to look at the tutorial object (hard bit!)
     * -exit camera
     *      c again to exit camera
     * -show scrapbook
     *      scrapbook key to see scrapbook
     * -exit scrapbook
     *      this finishes the tutorial
     * 
     */
     
	private enum TutorialStage
	{
		pre = 0,
		nothing,
		lookAround,
		walk,
		camera,
		takePhoto,
		exitCamera,
		scrapbook,
		exitScrapbook,
		finished
	}

	private TutorialStage stage = TutorialStage.pre;

	// Use this for initialization
	void Start ()
	{
		invisibleEverything ();

		titleScreen.SetActive (true);

		photoCameraInstructionsText.SetActive (false);
		cameraIcon.SetActive (false);
		journalIcon.SetActive (false);
		completionText.SetActive (false);

		/*
        photoCameraInstructionsText.SetActive(false);


        mouseIcon.SetActive(true);
        upArrowIcon.SetActive(true);
        downArrowIcon.SetActive(true);
        leftArrowIcon.SetActive(true);
        rightArrowIcon.SetActive(true);
        wasdIcon.SetActive(false);
        cameraKeyIcon.SetActive(false);
        openScrapbookIcon.SetActive(false);

        cameraLeftArrowIcon.SetActive(false);
        cameraRightArrowIcon.SetActive(false);
        takePhotoIcon.SetActive(false);
        exitCameraIcon.SetActive(false);
        */
	}

	void scheduleTutorialEvent (TutorialStage stage)
	{

	}

	private bool walked;

	float fadeInIncrement = 0.05f;

	private IEnumerator ShortDelay ()
	{
		yield return new WaitForSecondsRealtime (2);
	}

	// Update is called once per frame
	void Update ()
	{
		//if (Input.GetKeyDown(KeyCode.Escape) && (stage < TutorialStage.camera || stage > TutorialStage.exitCamera))
		//{
		//    finishTutorial();
		//}
		/*else*/
		switch (stage) {
		case TutorialStage.pre:
			titleScreen.SetActive (true);
			if (Input.anyKey) {
				StartCoroutine (ShortDelay ());
				stage = TutorialStage.nothing;
				titleScreen.SetActive (false);
			}
			break;
		case TutorialStage.nothing:
			makeVisible (mouseIcon);
			makeVisible (upArrowIcon);
			makeVisible (downArrowIcon);
			makeVisible (leftArrowIcon);
			makeVisible (rightArrowIcon);
			stage = TutorialStage.lookAround;
			break;
		case TutorialStage.lookAround:
			if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
				makeInvisible (mouseIcon);
				makeVisible (wasdIcon);
				stage = TutorialStage.walk;
			}
			break;
		case TutorialStage.walk:
			if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
				walked = true;
			}
			if (walked) {
				makeInvisible (wasdIcon);
				makeInvisible (upArrowIcon);
				makeInvisible (downArrowIcon);
				makeInvisible (leftArrowIcon);
				makeInvisible (rightArrowIcon);
				makeVisible (cameraKeyIcon);
				cameraIcon.SetActive (true);
				makeVisible (cameraIcon);
				stage = TutorialStage.camera;
			}
			break;
		case TutorialStage.camera:
			if (Input.GetKeyDown (KeyCode.C)) {
				cameraKeyIcon.SetActive (false);
				stage = TutorialStage.takePhoto;
			}
			break;
		case TutorialStage.takePhoto:
			Transform cameraTransform = player.GetComponent<Transform> ();
			Transform animalTransform = tutorialAnimal.GetComponent<Transform> ();
			Vector2 camera2DpointH = new Vector2 (cameraTransform.position.x, cameraTransform.position.z);
			Vector2 animal2DpointH = new Vector2 (animalTransform.position.x, animalTransform.position.z);
			Vector2 toVectorH = animal2DpointH - camera2DpointH;
			Vector2 fromVectorH = new Vector2 (cameraTransform.forward.x, cameraTransform.forward.z);
                
                /*
                Vector2 camera2DpointV = new Vector2(cameraTransform.position.y, cameraTransform.position.z);
                Vector2 animal2DpointV = new Vector2(animalTransform.position.y, animalTransform.position.z);
                Vector2 toVectorV = animal2DpointV - camera2DpointV;
                Vector2 fromVectorV = new Vector2(cameraTransform.forward.y, cameraTransform.forward.z);
                */

                //magnitude of the angle between camera direction and animal (dot product)
			float angleH = Vector2.Angle (fromVectorH, toVectorH);
                //float angleV = Vector2.Angle(fromVectorV, toVectorV);
			if (angleH < 20.0F) {// && angleV < 20.0F)
				takePhotoIcon.SetActive (true);
				cameraLeftArrowIcon.SetActive (false);
				cameraRightArrowIcon.SetActive (false);
				//cameraUpArrowIcon.SetActive(false);
				//cameraDownArrowIcon.SetActive(false);
			} else {
				takePhotoIcon.SetActive (false);
				if (angleH >= 20.0F) {
					//polarity of the angle (cross product)
					Vector3 crossH = Vector3.Cross (fromVectorH, toVectorH);
					if (crossH.z > 0) {
						cameraLeftArrowIcon.SetActive (true);
						cameraRightArrowIcon.SetActive (false);
					} else {
						cameraLeftArrowIcon.SetActive (false);
						cameraRightArrowIcon.SetActive (true);
					}
				}
				/*if(angleV >= 20.0F)
                    {
                        Vector3 crossV = Vector3.Cross(fromVectorV, toVectorV);
                        Debug.Log(crossV);
                        if (crossV.x > 0)
                        {
                            cameraUpArrowIcon.SetActive(true);
                            cameraDownArrowIcon.SetActive(false);
                        }
                        else
                        {
                            cameraUpArrowIcon.SetActive(false);
                            cameraDownArrowIcon.SetActive(true);
                        }
                    }*/
			}
			if (/*tutorialAnimal.GetComponent<IsSeen>().IsOnScreen() && */(Input.GetKeyDown (KeyCode.F) || Input.GetMouseButtonDown (0))) {
				takePhotoIcon.SetActive (false);
				cameraLeftArrowIcon.SetActive (false);
				cameraRightArrowIcon.SetActive (false);
				photoCameraInstructionsText.SetActive (true);
				makeVisible (exitCameraIcon);
				stage = TutorialStage.exitCamera;
			}
			break;
		case TutorialStage.exitCamera:
			if (Input.GetKeyDown (KeyCode.C)) {
				makeInvisible (exitCameraIcon);
				makeVisible (openScrapbookIcon);
				journalIcon.SetActive (true);
				makeVisible (journalIcon);
				stage = TutorialStage.scrapbook;
			}
			break;
		case TutorialStage.scrapbook:
			if (OpenJournal.journalIsOpen) {
				makeInvisible (openScrapbookIcon);
				stage = TutorialStage.exitScrapbook;
			}
			break;
		case TutorialStage.exitScrapbook:
			if (!OpenJournal.journalIsOpen) {
				makeInvisible (openScrapbookIcon);
				finishTutorial ();
			}
			break;
		case TutorialStage.finished:
			cameraIcon.SetActive (true);
			journalIcon.SetActive (true);
			photoCameraInstructionsText.SetActive (true);
			completionText.SetActive (true);
			break;
		}
        
		for (int i = 0; i < visibleUIElements.Count; i++) {
			GameObject icon = visibleUIElements [i];
			bool removeThisObject = incrementColor (icon, fadeInIncrement, 1);
			if (removeThisObject) {
				visibleUIElements.RemoveAt (i);
				i--;
			}
		}

		for (int i = 0; i < invisibleUIElements.Count; i++) {
			GameObject icon = invisibleUIElements [i];
			bool removeThisObject = incrementColor (icon, -fadeInIncrement, 0);
			if (removeThisObject) {
				invisibleUIElements.RemoveAt (i);
				i--;
			}
		}
        
	}

	void finishTutorial ()
	{
		stage = TutorialStage.finished;
	}


	void makeVisible (GameObject o)
	{
		if (invisibleUIElements.Contains (o))
			invisibleUIElements.Remove (o);
		visibleUIElements.Add (o);
	}

	void makeInvisible (GameObject o)
	{
		if (visibleUIElements.Contains (o))
			visibleUIElements.Remove (o);
		invisibleUIElements.Add (o);
	}

	void invisibleEverything ()
	{
		invisibleUIElements.AddRange (new GameObject[] { cameraIcon, journalIcon, mouseIcon,
			upArrowIcon, downArrowIcon, leftArrowIcon, rightArrowIcon, wasdIcon, cameraKeyIcon, openScrapbookIcon,
			exitCameraIcon
		}); //cameraLeftArrowIcon, cameraRightArrowIcon, takePhotoIcon,

		foreach (GameObject o in invisibleUIElements) {
			try {
				SpriteRenderer renderer = o.GetComponent<SpriteRenderer> ();
				renderer.color = new Color (renderer.color.r, renderer.color.g, renderer.color.b, 0f);
			} catch (MissingComponentException mce) {
				RawImage image = o.GetComponent<RawImage> ();
				image.color = new Color (image.color.r, image.color.g, image.color.b, 0f);
			}
		}

	}

	bool incrementColor (GameObject icon, float amount, float target)
	{
		try {
			SpriteRenderer renderer = icon.GetComponent<SpriteRenderer> ();
			if ((amount < 0 && renderer.color.a > target) || (amount > 0 && renderer.color.a < target)) {
				renderer.color = new Color (1, 1, 1, renderer.color.a + amount);
				return false;
			} else {
				return true;
			}
		} catch (MissingComponentException mce) {
			RawImage image = icon.GetComponent<RawImage> ();
			if ((amount < 0 && image.color.a > target) || (amount > 0 && image.color.a < target)) {
				image.color = new Color (1, 1, 1, image.color.a + amount);
				return false;
			} else {
				return true;
			}
		}
	}

}
