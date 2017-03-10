using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeable : MonoBehaviour
{

	public ViewCreatures viewCreatures;
	public string creatureName;

	public static HashSet<string> seeableCreatures = new HashSet<string> ();

	public void Start ()
	{
		seeableCreatures.Add (creatureName);
		Logging.Info (string.Format ("Initialising button referencing {0}", creatureName));
	}

	public void Select ()
	{
		// activate the creature
		Logging.Info (string.Format ("Button referencing {0} has been pressed", creatureName));
		viewCreatures.SetCurrentCreature (creatureName);
	}
}
