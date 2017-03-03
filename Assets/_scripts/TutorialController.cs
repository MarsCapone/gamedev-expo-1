using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    public GameObject cameraIcon, journalIcon, photoCameraInstructionsText, completionText;
    public GameObject tutorialAnimal;
    public GameObject player;
    public GameObject mouseIcon, upArrowIcon, leftArrowIcon, rightArrowIcon, downArrowIcon, wasdIcon, cameraKeyIcon, takePhotoIcon, openScrapbookIcon;

    public GameObject cameraLeftArrowIcon, cameraRightArrowIcon, exitCameraIcon;

    private List<GameObject> visibleUIElements = new List<GameObject>();
    private List<GameObject> invisibleUIElements = new List<GameObject>();

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
        nothing=0,
        lookAround,
        walk,
        camera,
        takePhoto,
        exitCamera,
        scrapbook,
        exitScrapbook,
        finished
    }

    private TutorialStage stage = TutorialStage.nothing;

	// Use this for initialization
	void Start () {
        invisibleEverything();

        photoCameraInstructionsText.SetActive(false);
        cameraIcon.SetActive(false);
        journalIcon.SetActive(false);
        completionText.SetActive(false);

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

    void scheduleTutorialEvent(TutorialStage stage)
    {

    }

    private bool walked;

    float fadeInIncrement = 0.05f;
    // Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && (stage < TutorialStage.camera || stage > TutorialStage.exitCamera))
        {
            finishTutorial();
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            walked = true;
        }
        //else if (Input.GetKeyDown(KeyCode.C))
        //{
        //    openedCamera = true;
        //}else if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    openedScrapbook = true;
        //}
        switch (stage)
        {
            case TutorialStage.nothing:
                makeVisible(mouseIcon);
                makeVisible(upArrowIcon);
                makeVisible(downArrowIcon);
                makeVisible(leftArrowIcon);
                makeVisible(rightArrowIcon);
                stage = TutorialStage.lookAround;
                break;
            case TutorialStage.lookAround:
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    makeInvisible(mouseIcon);
                    makeVisible(wasdIcon);
                    stage = TutorialStage.walk;
                    Debug.Log("DEBUG: TUTORIAL: Walk with WASD");
                }
                break;
            case TutorialStage.walk:
                if (walked)
                {
                    makeInvisible(wasdIcon);
                    makeInvisible(upArrowIcon);
                    makeInvisible(downArrowIcon);
                    makeInvisible(leftArrowIcon);
                    makeInvisible(rightArrowIcon);
                    makeVisible(cameraKeyIcon);
                    cameraIcon.SetActive(true);
                    makeVisible(cameraIcon);
                    stage = TutorialStage.camera;
                    Debug.Log("DEBUG: TUTORIAL: Open the camera");
                }
                break;
            case TutorialStage.camera:
                if (Input.GetKeyDown(KeyCode.C))
                {
                    cameraKeyIcon.SetActive(false);
                    stage = TutorialStage.takePhoto;
                    Debug.Log("DEBUG: TUTORIAL: Look for the tutorial object and take a photo of it");
                }
                break;
            case TutorialStage.takePhoto:
                Transform cameraTransform = player.GetComponent<Transform>();
                Transform animalTransform = tutorialAnimal.GetComponent<Transform>();
                Vector2 xCameraTransform = new Vector2(cameraTransform.position.x, cameraTransform.position.z);
                Vector2 xAnimalTransform = new Vector2(animalTransform.position.x, animalTransform.position.z);
                Vector2 toVector = xAnimalTransform - xCameraTransform;
                Vector2 fromVector = new Vector2(cameraTransform.forward.x, cameraTransform.forward.z);

                //magnitude of the angle between camera direction and animal (dot product)
                float angle = Vector2.Angle(fromVector, toVector);
                if(angle < 20.0F)
                {
                    takePhotoIcon.SetActive(true);
                    cameraLeftArrowIcon.SetActive(false);
                    cameraRightArrowIcon.SetActive(false);
                }else
                {
                    //polarity of the angle (cross product)
                    Vector3 cross = Vector3.Cross(fromVector, toVector);
                    if(cross.z > 0)
                    {
                        takePhotoIcon.SetActive(false);
                        cameraLeftArrowIcon.SetActive(true);
                        cameraRightArrowIcon.SetActive(false);
                    }else
                    {
                        takePhotoIcon.SetActive(false);
                        cameraLeftArrowIcon.SetActive(false);
                        cameraRightArrowIcon.SetActive(true);
                    }
                }
                /*!!! TEMPORARY CHECK TO PROGRESS TUTORIAL ON
                 * TODO REPLACE CONDITION WITH ACTUAL 'IS THIS ANIMAL IN THE PHOTO' CHECK !!!
                 */
                if(angle < 20.0F && (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0)))
                {
                    takePhotoIcon.SetActive(false);
                    cameraLeftArrowIcon.SetActive(false);
                    cameraRightArrowIcon.SetActive(false);
                    photoCameraInstructionsText.SetActive(true);
                    makeVisible(exitCameraIcon);
                    stage = TutorialStage.exitCamera;
                    Debug.Log("DEBUG: TUTORIAL: Exit the camera");
                }
                break;
            case TutorialStage.exitCamera:
                if (Input.GetKeyDown(KeyCode.C))
                {
                    makeInvisible(exitCameraIcon);
                    makeVisible(openScrapbookIcon);
                    journalIcon.SetActive(true);
                    makeVisible(journalIcon);
                    stage = TutorialStage.scrapbook;
                    Debug.Log("DEBUG: TUTORIAL: Open the scrapbook");
                }
                break;
            case TutorialStage.scrapbook:
                if (OpenJournal.journalIsOpen)
                {
                    makeInvisible(openScrapbookIcon);
                    stage = TutorialStage.exitScrapbook;
                    Debug.Log("DEBUG: TUTORIAL: Exit the scrapbook");
                }
                break;
            case TutorialStage.exitScrapbook:
                if (!OpenJournal.journalIsOpen)
                {
                    makeInvisible(openScrapbookIcon);
                    Debug.Log("DEBUG: TUTORIAL: Finished.");
                    finishTutorial();
                }
                break;
            case TutorialStage.finished:
                cameraIcon.SetActive(true);
                journalIcon.SetActive(true);
                photoCameraInstructionsText.SetActive(true);
                completionText.SetActive(true);
                break;
        }
        
        for(int i = 0; i < visibleUIElements.Count; i++)
        {
            GameObject icon = visibleUIElements[i];
            bool removeThisObject = incrementColor(icon, fadeInIncrement, 1);
            if (removeThisObject)
            {
                visibleUIElements.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < invisibleUIElements.Count; i++)
        {
            GameObject icon = invisibleUIElements[i];
            bool removeThisObject = incrementColor(icon, -fadeInIncrement, 0);
            if (removeThisObject)
            {
                invisibleUIElements.RemoveAt(i);
                i--;
            }
        }
        
    }

    void finishTutorial()
    {
        stage = TutorialStage.finished;
    }


    void makeVisible(GameObject o)
    {
        if (invisibleUIElements.Contains(o)) invisibleUIElements.Remove(o);
        visibleUIElements.Add(o);
    }

    void makeInvisible(GameObject o)
    {
        if(visibleUIElements.Contains(o)) visibleUIElements.Remove(o);
        invisibleUIElements.Add(o);
    }

    void invisibleEverything()
    {
        invisibleUIElements.AddRange(new GameObject[] { cameraIcon, journalIcon , mouseIcon,
        upArrowIcon, downArrowIcon, leftArrowIcon, rightArrowIcon, wasdIcon, cameraKeyIcon, openScrapbookIcon,
         exitCameraIcon}); //cameraLeftArrowIcon, cameraRightArrowIcon, takePhotoIcon,

        foreach (GameObject o in invisibleUIElements)
        {
            try
            {
                SpriteRenderer renderer = o.GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0f);
            }catch(MissingComponentException mce)
            {
                RawImage image = o.GetComponent<RawImage>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            }
        }

    }

    bool incrementColor(GameObject icon, float amount, float target)
    {
        try{
            SpriteRenderer renderer = icon.GetComponent<SpriteRenderer>();
            if ((amount < 0 && renderer.color.a > target) || (amount > 0 && renderer.color.a < target))
            {
                renderer.color = new Color(1, 1, 1, renderer.color.a + amount);
                return false;
            }else
            {
                return true;
            }
        }catch (MissingComponentException mce)
        {
            RawImage image = icon.GetComponent<RawImage>();
            if ((amount < 0 && image.color.a > target) || (amount > 0 && image.color.a < target))
            {
                image.color = new Color(1, 1, 1, image.color.a + amount);
                return false;
            }else
            {
                return true;
            }
        }
    }
}
