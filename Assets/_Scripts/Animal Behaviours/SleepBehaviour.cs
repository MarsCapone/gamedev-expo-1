using System;
using UnityEngine;

[System.Serializable]
public class SleepBehaviour : AnimalBehaviour
{
    public GameObject home;
    
    public override void perform(Animal animal)
    {
        Action goToSleep = delegate { animal.sleep(); };
        Action walkHome = delegate {
            if (home) animal.walkTo(home.GetComponent<Transform>().position);
            goToSleep();
        };
        walkHome();
    }
    
}