using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

	public GameObject cameraIcon, journalIcon, photoCameraInstructionsText;
	public GameObject tutorialAnimal;
	public GameObject player;
	public GameObject mouseIcon, upArrowIcon, leftArrowIcon, rightArrowIcon, downArrowIcon, wasdIcon, cameraKeyIcon, takePhotoIcon, openScrapbookIcon;

	public GameObject cameraLeftArrowIcon, cameraRightArrowIcon, exitCameraIcon;

	private float time;

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
		nothing = 0,
		lookAround,
		walk,
		camera,
		takePhoto,
		exitCamera,
		scrapbook,
		exitScrapbook,
		finished
	}

	private TutorialStage stage = TutorialStage.lookAround;

	// Use this for initialization
	void Start ()
	{
		cameraIcon.SetActive (false);
		journalIcon.SetActive (false);
		photoCameraInstructionsText.SetActive (false);
		mouseIcon.SetActive (true);
		upArrowIcon.SetActive (true);
		downArrowIcon.SetActive (true);
		leftArrowIcon.SetActive (true);
		rightArrowIcon.SetActive (true);
		wasdIcon.SetActive (false);
		cameraKeyIcon.SetActive (false);
		openScrapbookIcon.SetActive (false);

		cameraLeftArrowIcon.SetActive (false);
		cameraRightArrowIcon.SetActive (false);
		takePhotoIcon.SetActive (false);
		exitCameraIcon.SetActive (false);

		Logging.Info ("Initialising tutorial controller.");
	}

	void scheduleTutorialEvent (TutorialStage stage)
	{

	}

	private bool walked, openedCamera, openedScrapbook;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && (stage < TutorialStage.camera || stage > TutorialStage.exitCamera)) {
			finishTutorial ();
		} else if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			walked = true;
		}
		//else if (Input.GetKeyDown(KeyCode.C))
		//{
		//    openedCamera = true;
		//}else if (Input.GetKeyDown(KeyCode.Tab))
		//{
		//    openedScrapbook = true;
		//}
		switch (stage) {
		case TutorialStage.lookAround:
			if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
				mouseIcon.SetActive (false);
				wasdIcon.SetActive (true);
				stage = TutorialStage.walk;
				Debug.Log ("DEBUG: TUTORIAL: Walk with WASD");
			}
			break;
		case TutorialStage.walk:
			if (walked) {
				wasdIcon.SetActive (false);
				upArrowIcon.SetActive (false);
				downArrowIcon.SetActive (false);
				leftArrowIcon.SetActive (false);
				rightArrowIcon.SetActive (false);
				cameraKeyIcon.SetActive (true);
				cameraIcon.SetActive (true);
				stage = TutorialStage.camera;
				Debug.Log ("DEBUG: TUTORIAL: Open the camera");
			}
			break;
		case TutorialStage.camera:
			if (Input.GetKeyDown (KeyCode.C)) {
				cameraKeyIcon.SetActive (false);
				stage = TutorialStage.takePhoto;
				Debug.Log ("DEBUG: TUTORIAL: Look for the tutorial object and take a photo of it");
			}
			break;
		case TutorialStage.takePhoto:
			Transform cameraTransform = player.GetComponent<Transform> ();
			Transform animalTransform = tutorialAnimal.GetComponent<Transform> ();
			Vector2 xCameraTransform = new Vector2 (cameraTransform.position.x, cameraTransform.position.z);
			Vector2 xAnimalTransform = new Vector2 (animalTransform.position.x, animalTransform.position.z);
			Vector2 toVector = xAnimalTransform - xCameraTransform;
			Vector2 fromVector = new Vector2 (cameraTransform.forward.x, cameraTransform.forward.z);

                //magnitude of the angle between camera direction and animal (dot product)
			float angle = Vector2.Angle (fromVector, toVector);
			if (angle < 20.0F) {
				takePhotoIcon.SetActive (true);
				cameraLeftArrowIcon.SetActive (false);
				cameraRightArrowIcon.SetActive (false);
			} else {
				//polarity of the angle (cross product)
				Vector3 cross = Vector3.Cross (fromVector, toVector);
				if (cross.z > 0) {
					takePhotoIcon.SetActive (false);
					cameraLeftArrowIcon.SetActive (true);
					cameraRightArrowIcon.SetActive (false);
				} else {
					takePhotoIcon.SetActive (false);
					cameraLeftArrowIcon.SetActive (false);
					cameraRightArrowIcon.SetActive (true);
				}
			}
                /*!!! TEMPORARY CHECK TO PROGRESS TUTORIAL ON
                 * TODO REPLACE CONDITION WITH ACTUAL 'IS THIS ANIMAL IN THE PHOTO' CHECK !!!
                 */
			if (angle < 20.0F && (Input.GetKeyDown (KeyCode.F) || Input.GetMouseButtonDown (0))) {
				takePhotoIcon.SetActive (false);
				cameraLeftArrowIcon.SetActive (false);
				cameraRightArrowIcon.SetActive (false);
				exitCameraIcon.SetActive (true);
				photoCameraInstructionsText.SetActive (true);
				stage = TutorialStage.exitCamera;
				Debug.Log ("DEBUG: TUTORIAL: Exit the camera");
			}
			break;
		case TutorialStage.exitCamera:
			if (Input.GetKeyDown (KeyCode.C)) {
				exitCameraIcon.SetActive (false);
				openScrapbookIcon.SetActive (true);
				journalIcon.SetActive (true);
				stage = TutorialStage.scrapbook;
				Debug.Log ("DEBUG: TUTORIAL: Open the scrapbook");
			}
			break;
		case TutorialStage.scrapbook:
			if (Input.GetKeyDown (KeyCode.Tab)) {
				openScrapbookIcon.SetActive (false);
				//TODO: close scrapbook icon
				stage = TutorialStage.exitScrapbook;
				Debug.Log ("DEBUG: TUTORIAL: Exit the scrapbook");
			}
			break;
		case TutorialStage.exitScrapbook:
			if (Input.GetKeyDown (KeyCode.Tab)) {
				Debug.Log ("DEBUG: TUTORIAL: Finished.");
				finishTutorial ();
			}
			break;
		case TutorialStage.finished:
			cameraIcon.SetActive (true);
			journalIcon.SetActive (true);
			photoCameraInstructionsText.SetActive (true);
			break;
		}
		//TODO listen for key events and move tutorial on accordingly
		//if (triggerTime > 0)
		//{
		//    if (time > triggerTime && triggerEvent != TutorialStage.nothing)
		//    {
		//        switch (triggerEvent)
		//        {
		//            case TutorialStage.lookAround:
		//                break;
		//            default:
		//                break;
		//        }
		//   }
		//}

	}

	void finishTutorial ()
	{
		stage = TutorialStage.finished;
	}

	//public void updateTime(float t)
	//{
	//    time = t;
	//}

	//public void newDay() { }
}
