using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : AnimalBehaviour
{
    public override void perform(Animal animal)
    {
        animal.wander();
    }
}
