using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourInterrupt : MonoBehaviour {

    public Animal animalToTrigger;
    public bool enabled = true;

    public abstract void trigger();

    public abstract bool check();
    //public abstract bool checkFinished();

}
