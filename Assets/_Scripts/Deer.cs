using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deer : Animal {
    
    private Animation animationController;


    [Serializable]
    public class BehaviourAnimation
    {
        public BehaviourState state;
        public string animation;
    }

    public BehaviourAnimation[] animations;
    private Dictionary<BehaviourState, string> animationDict = new Dictionary<BehaviourState, string>();

    private BehaviourState _state = BehaviourState.idle;
    public override BehaviourState state
    {
        get
        {
            return _state;
        }

        set
        {
            if (animationController != null)
            {
                //sleep animation is used extensively throughout the setter
                string sleepAnimation = animationDict.ContainsKey(BehaviourState.asleep) ? animationDict[BehaviourState.asleep] : null;
                if (sleepAnimation != null && _state == BehaviourState.asleep)
                {
                    //wake up by playing the asleep animation backwards
                    AnimationState wakeState = animationController.PlayQueued(sleepAnimation, QueueMode.PlayNow);
                    wakeState.speed = scheduleInterrupted ? -2F : -0.25F; //if interrupted from sleep, wake up fast, otherwise really groggily
                    wakeState.time = animationController[sleepAnimation].length - 0.01F; //fix from http://answers.unity3d.com/questions/156869/reverse-animation-with-quotplayqueuedquot.html
                                                                                         //then play the desired animation
                    _state = value;
                    if (animationDict.ContainsKey(value)) animationController.PlayQueued(animationDict[value], QueueMode.CompleteOthers);
                }
                else
                {
                    _state = value;
                    //set things up before playing
                    if (value == BehaviourState.asleep && _state != BehaviourState.asleep)
                    {
                        animationController[sleepAnimation].speed = 1;
                        animationController[sleepAnimation].time = -1;
                    }
                    if (animationDict.ContainsKey(value)) animationController.Play(animationDict[value]);
                }
            }
            else
            {
                _state = value;
            }
        }
    }

    private BehaviourState lastState = BehaviourState.idle;
    private void OnDisable()
    {
        lastState = _state;
    }
    private void OnEnable()
    {
        state = lastState;
    }


    protected NavMeshAgent navigation;

    // Use this for initialization
    public override void init ()
    {
        foreach (BehaviourAnimation a in animations)
        {
            animationDict.Add(a.state, a.animation);
        }
        animationController = GetComponent<Animation>();
        navigation = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	public override void UpdateAndBehave () {
        if (currentAction == BehaviourAction.walkTowards || currentAction == BehaviourAction.runAway)
        {
            float distanceToTarget = navigation.remainingDistance;
            if ((!navigation.pathPending) &&
                (navigation.remainingDistance <= navigation.stoppingDistance) &&
                (!navigation.hasPath || navigation.velocity.sqrMagnitude == 0f))
            {
                navigation.Stop();
                switch (currentAction)
                {
                    case BehaviourAction.none:
                        break;
                    case BehaviourAction.sleep:
                        break;
                    case BehaviourAction.walkTowards:
                        if (continuation != null)
                        {
                            Action currentContinuation = continuation;
                            continuation = null;
                            currentContinuation();
                        }
                        else
                        {
                            idle();
                        }
                        break;
                    case BehaviourAction.runAway:
                        resumeSchedule();
                        break;
                    case BehaviourAction.wander:
                        wander();
                        break;
                    case BehaviourAction.idle:
                    default:
                        idle();
                        break;
                }
                currentAction = BehaviourAction.none;
            }
        }
    }

    public override void sleep()
    {
        state = BehaviourState.asleep;
    }

    public override void wander()
    {
        //TODO (maybe) actually wander around
        state = BehaviourState.walking;
    }

    public override void walkTo(Vector3 location, float tolerance = 0)
    {
        state = BehaviourState.walking;
        navigation.SetDestination(location);
        navigation.stoppingDistance = tolerance;
        currentAction = BehaviourAction.walkTowards;
    }

    public override void runAwayFrom(GameObject obj, float distanceToRun, float tolerance = 0)
    {
        navigation.ResetPath();
        state = BehaviourState.running;
        //code adapted from http://answers.unity3d.com/questions/868003/navmesh-flee-ai-flee-from-player.html
        Quaternion forwardRotation = Quaternion.LookRotation(transform.position - obj.GetComponent<Transform>().position);
        Quaternion leftRotation = forwardRotation * Quaternion.Euler(0, 90, 0);
        Quaternion rightRotation = forwardRotation * Quaternion.Euler(0, -90, 0);

        //run in a random direction: either directly away from, or at a right angle to, the player
        Quaternion[] rotations = { forwardRotation, leftRotation, rightRotation };

        for (int i = 0; i < rotations.Length; i++)
        {
            Quaternion newRotation = rotations[i];
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, 0, newRotation.eulerAngles.z);

            Vector3 runTo = transform.position + newRotation.eulerAngles * (distanceToRun + tolerance);
            if (runTo.x < -650 || runTo.x > 650 || runTo.z < -650 || runTo.z > 650)
            {
                //if this rotation is off the edge of the map, don't run there
                continue;
            }

            Debug.Log(runTo);

            navigation.SetDestination(runTo);
            navigation.stoppingDistance = tolerance;
            currentAction = BehaviourAction.runAway;
            break;
        }
    }
}
