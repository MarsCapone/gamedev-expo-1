using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTiler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Terrain terrain = GetComponent<Terrain> ();
		terrain.SetNeighbors (terrain, terrain, terrain, terrain);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
