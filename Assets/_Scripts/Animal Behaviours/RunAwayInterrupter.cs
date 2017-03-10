using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayInterrupter : BehaviourInterrupt {

    public GameObject objectToRunAwayFrom;

    public float triggerDistance, safetyDistance;

    bool activated = false;

    public override void trigger()
    {
        animalToTrigger.runAwayFrom(objectToRunAwayFrom, safetyDistance, triggerDistance / 2);
    }
    
    public override bool check()
    {
        return Vector3.Distance(objectToRunAwayFrom.transform.position, animalToTrigger.transform.position) < triggerDistance;
    }

    /*public override bool checkFinished()
    {
        return Vector3.Distance(objectToRunAwayFrom.transform.position, animalToTrigger.transform.position) > safetyDistance;
    }*/
}
