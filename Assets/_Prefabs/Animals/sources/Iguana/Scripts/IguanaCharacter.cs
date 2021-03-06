﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;

//modified by JPML to fit the day/night cycle etc
public class IguanaCharacter : Animal {
	
	public void Attack(){
		iguanaAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		iguanaAnimator.SetTrigger("Hit");
	}
	
	public void Eat(){
		iguanaAnimator.SetTrigger("Eat");
	}

	public void Death(){
		iguanaAnimator.SetTrigger("Death");

	}

	public void Rebirth(){
		iguanaAnimator.SetTrigger("Rebirth");
	}


	
	public void Move(float v,float h){
		iguanaAnimator.SetFloat ("Forward", v);
		iguanaAnimator.SetFloat ("Turn", h);
	}

    //JPML:
    NavMeshAgent nma;
    Animator iguanaAnimator;
    Rigidbody rb;
    float velocity, turn;
    System.Random rnd = new System.Random();

    private BehaviourState _state = BehaviourState.idle;
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
                Rebirth();
                Debug.Log("iguana awakens");
                //rb.constraints = RigidbodyConstraints.None;
            }
            _state = value;
        }
    }
   

    public override void init()
    {
        iguanaAnimator = GetComponent<Animator>();
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public override void UpdateAndBehave()
    {
        if (currentAction == BehaviourAction.walkTowards)
        {
            Move(nma.velocity.magnitude, 0);
            float distanceToTarget = nma.remainingDistance;
            if ((!nma.pathPending) &&
                (nma.remainingDistance <= nma.stoppingDistance) &&
                (!nma.hasPath || nma.velocity.sqrMagnitude == 0f))
            {
                Debug.Log("IGUANA STOPPED");
                nma.Stop();
                if(continuation != null)
                {
                    Action currentContinuation = continuation;
                    continuation = null;
                    currentContinuation();
                }
                else
                {
                    idle();
                }
            }
        }else if(state == BehaviourState.eating)
        {
            if (rnd.NextDouble() > 0.99)
            {
                Eat();
            }
        }
    }

    public override void sleep()
    {
        Move(0, 0);
        //nma.ResetPath();
        Death();
        state = BehaviourState.asleep;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    
    public override void walkTo(Vector3 location, float tolerance = 0)
    {
        state = BehaviourState.walking;
        nma.ResetPath();
        nma.SetDestination(location);
        nma.stoppingDistance = tolerance;
        currentAction = BehaviourAction.walkTowards;
    }

    public override void wander()
    {
        velocity = Mathf.Abs(velocity + (float)(rnd.NextDouble()));
        turn = turn + (float)(rnd.NextDouble()) / 10;
        Move(velocity, turn);
        state = BehaviourState.walking;
    }

    
    public override void runAwayFrom(GameObject obj, float distanceToRun, float tolerance = 0)
    {
        throw new NotImplementedException();
    }
}
