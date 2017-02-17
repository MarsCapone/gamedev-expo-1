using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	Camera cam;

	public float speedH = 2f;
	public float speedV = 2f;

	private float yaw = 0f;
	private float pitch = 0f;

	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("Camera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		yaw += speedH * Input.GetAxis ("Mouse X");
		pitch -= speedV * Input.GetAxis ("Mouse Y");


	}
}
