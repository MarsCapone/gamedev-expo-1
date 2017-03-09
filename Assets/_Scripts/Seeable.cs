using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeable : MonoBehaviour {

	public ViewCreatures viewCreatures;
	public string creature;

	public static HashSet<string> seeableCreatures = new HashSet<string> ();

	public void Start () {
		if (creature == null) {
			creature = "Other";
		}
		seeableCreatures.Add (creature);
	}

	public void Select () {
		// activate the creature
		print(creature + " button has been pressed.");
		viewCreatures.SetCurrentCreature(creature);
	}
}
