using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//modified by JPML to work with ingame time
//(doesn't work properly)
public class IwasiMove : MonoBehaviour, ITimeBasedObject {
	public float speed=1.5f;
	public float rotateSpeed=100f;
	private Transform target;
    public GameObject swimLocations;
    private Transform[] swimLocationArray;
	Vector3 targetRelPos;

    System.Random rnd = new System.Random();

    private float time = 0f;
    private int daySegment = 0;
    public int numberOfSwimsPerDay = 4;

    private void Start()
    {
        swimLocationArray = swimLocations.GetComponentsInChildren<Transform>();
        Debug.Log(swimLocationArray);
        target = swimLocationArray[0];
    }

	void Update () {
		targetRelPos = target.position - transform.position;
		Rigidbody iwasirigid = GetComponent<Rigidbody> ();

		float dottigawa = Vector3.Dot (targetRelPos, transform.right);
		if (dottigawa < 0) {
			iwasirigid.AddTorque(-Vector3.up  * rotateSpeed);
		} else if (dottigawa > 0) {
			iwasirigid.AddTorque(Vector3.up  * rotateSpeed);
		}

		iwasirigid.velocity= transform.forward * speed;
	}
    
    public void updateTime(float t)
    {
        if (enabled)
        {
            time = t;
            int newSegment = (int)((t / 24) * numberOfSwimsPerDay);
            if (newSegment != daySegment)
            {
                int newLocation = (int)(rnd.NextDouble() * swimLocationArray.Length);
                target = swimLocationArray[newLocation];
                Debug.Log(target);
                daySegment = newSegment;
            }
        }
    }

    public void newDay()
    {
        time = 0;
    }
}
