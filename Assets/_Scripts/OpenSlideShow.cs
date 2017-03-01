using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenSlideShow : MonoBehaviour, IPointerClickHandler {

	public int position;
	public Journal journal;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick (PointerEventData data) {
		GameObject creature = journal.GetImageGameObject (position);
		journal.EnableAnimalSlide (creature);
	}
}
