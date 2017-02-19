using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IdleBehaviour : AnimalBehaviour
{
    public override void perform(Animal animal)
    {
        animal.idle();
    }
}
