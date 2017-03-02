using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeable : MonoBehaviour {

	public ViewCreatures viewCreatures;
	public GameObject creature;

	public void Start () {
		if (creature == null) {
			creature = new GameObject ();
			creature.name = "other";
		}
	}

	public void Select () {
		// activate the creature
		print(creature.name + " button has been pressed.");
		viewCreatures.SetCurrentCreature(creature);
	}
}
