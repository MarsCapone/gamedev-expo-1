using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Animal
{
    BehaviourState _state;
    public override BehaviourState state
    {
        get
        {
            return _state;
        }

        set
        {
            if (_state == BehaviourState.asleep && value != BehaviourState.asleep)
            {
                enabled = true;
            }
            _state = value;
        }
    }

    Rigidbody rb;
    Animation animationController;

    public override void init()
    {
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<Animation>();
    }

    public override void runAwayFrom(GameObject obj, float distanceToRun, float tolerance = 0)
    {
        
    }

    public override void sleep()
    {
        state = BehaviourState.asleep;
        enabled = false;
    }


    Vector3 flyLocation;
    public override void UpdateAndBehave()
    {
        
    }

    public override void walkTo(Vector3 location, float tolerance = 0)
    {
        state = BehaviourState.walking;
        //flyLocation = location
    }

    public override void wander()
    {

    }
}
