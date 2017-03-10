using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBehaviour : AnimalBehaviour
{
    public GameObject[] eatLocations;
    private System.Random rnd;
    public float hoursToEatFor = 5f;

    public EatBehaviour()
    {
        rnd = new System.Random();
    }

    /*public override void perform(Animal animal)
    {
        Action walkAction = null;
        Action eatAction = delegate
        {
            animal.eat(secondsToEatFor);
            animal.setContinuation(walkAction);
        };
        walkAction = delegate
        {
            Debug.Log(eatLocations[0]);
            GameObject eatLocation = eatLocations[(int)rnd.NextDouble() * eatLocations.Length];
            animal.walkTo(eatLocation.transform.position, 5);
            animal.setContinuation(eatAction);
        };
        walkAction();
    }*/

    public override void perform(Animal animal)
    {
        animal.eat(hoursToEatFor);
    }
}
