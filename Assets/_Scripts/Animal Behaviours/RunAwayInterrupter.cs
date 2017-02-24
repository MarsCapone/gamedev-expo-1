using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayInterrupter : BehaviourInterrupt {

    public GameObject objectToRunAwayFrom;
    public float distanceToRunAway;

    public float safeDistanceFromDanger;

    bool activated = false;

    public override void trigger()
    {
        animalToTrigger.runAwayFrom(objectToRunAwayFrom, distanceToRunAway, safeDistanceFromDanger);//walkTo(new Vector3(0, 0, 0), 10);//runAwayFrom(objectToRunAwayFrom, distanceToRunAway * 100F);
    }

//    public override void reset()
 //   {
  //      activated = false;
   // }

    /*public void OnCollisionEnter(Collision collision)
    {
        //   if (!activated)
        // {
        if (collision.gameObject.name != "Terrain (0,0)")
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name == "Player")
            {
                activated = true;
                Debug.Log("COLLISION ENTER");
                trigger();
            }
        }
        //}
    }*/

    public override bool check()
    {
        //Debug.Log(Vector3.Distance(objectToRunAwayFrom.transform.position, animalToTrigger.transform.position) < safeDistanceFromDanger);
        return Vector3.Distance(objectToRunAwayFrom.transform.position, animalToTrigger.transform.position) < safeDistanceFromDanger;
    }
}
