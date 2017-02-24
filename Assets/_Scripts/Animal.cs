using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour, ITimeBasedObject {
    
    public BehaviourState initialState;
    private BehaviourState _state = BehaviourState.idle;
    private Animation animationController;
    private bool scheduleInterrupted = false;
    public BehaviourState state
    {
        get
        {
            return _state;
        }
        set {
            //sleep animation is used extensively throughout the setter
            string sleepAnimation = animationDict.ContainsKey(BehaviourState.asleep) ? animationDict[BehaviourState.asleep] : null;
            if (sleepAnimation != null && _state == BehaviourState.asleep)
            {
                //wake up by playing the asleep animation backwards
                AnimationState wakeState = animationController.PlayQueued(sleepAnimation, QueueMode.PlayNow);
                wakeState.speed = scheduleInterrupted ? -1F : -0.25F; //if interrupted from sleep, wake up fast, otherwise really groggily
                wakeState.time = animationController[sleepAnimation].length - 0.01F; //fix from http://answers.unity3d.com/questions/156869/reverse-animation-with-quotplayqueuedquot.html
                //then play the desired animation
                _state = value;
                if (animationDict.ContainsKey(value)) animationController.PlayQueued(animationDict[value], QueueMode.CompleteOthers);
            }
            else
            {
                _state = value;
                //set things up before playing
                if (value == BehaviourState.asleep)
                {
                    animationController[sleepAnimation].speed = 1;
                    animationController[sleepAnimation].time = -1;
                }
                if(animationDict.ContainsKey(value)) animationController.Play(animationDict[value]);
            }
        }
    }
    private Action continuation; //used for chains of multiple actions

    [Serializable]
    public class BehaviourAnimation
    {
        public BehaviourState state;
        public string animation;
    }

    public BehaviourAnimation[] animations;

    private Dictionary<BehaviourState, string> animationDict = new Dictionary<BehaviourState, string>();

    private AnimalBehaviour currentBehaviour;

    private AnimalBehaviour[] schedule;
    private BehaviourInterrupt[] interrupts;

    private BehaviourAction currentAction;

    private NavMeshAgent navigation;
    //private bool navigating = false;

    void Start()
    {
        schedule = GetComponentsInChildren<AnimalBehaviour>();
        Array.Sort(schedule, (x, y) => x.startTime.CompareTo(y.startTime));
        foreach(BehaviourAnimation a in animations){
            animationDict.Add(a.state, a.animation);
        }
        animationController = GetComponent<Animation>();
        interrupts = GetComponentsInChildren<BehaviourInterrupt>();
        navigation = GetComponent<NavMeshAgent>();
    }

    public void updateTime(float time)
    {
        if (!scheduleInterrupted)
        {
            foreach (AnimalBehaviour b in schedule)
            {
                if (b.startTime <= time)
                {
                    if (!currentBehaviour || (currentBehaviour != b && currentBehaviour.startTime < b.startTime))
                    {
                        Debug.Log(b);
                        currentBehaviour = b;
                        b.activate(this);
                        break;
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (currentAction == BehaviourAction.walkTowards || currentAction == BehaviourAction.runAway) {
            float distanceToTarget = navigation.remainingDistance;
            if((!navigation.pathPending) && 
                (navigation.remainingDistance <= navigation.stoppingDistance) &&
                (!navigation.hasPath || navigation.velocity.sqrMagnitude == 0f))
            {
                Debug.Log("STOPPED");
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
        if (!scheduleInterrupted)
        {
            foreach (BehaviourInterrupt interrupt in interrupts)
            {
                if (interrupt.check())
                {
                    interruptSchedule();
                    interrupt.trigger();
                }
            }
        }
    }

    public void newDay()
    {
        currentBehaviour = null;
    }

    //method stubs that currently just do animation

    public void sleep()
    {
        state = BehaviourState.asleep;
    }

    public void walkTo(Vector3 location, float tolerance=0F)
    {
        state = BehaviourState.walking;
        navigation.SetDestination(location);
        navigation.stoppingDistance = tolerance;
        currentAction = BehaviourAction.walkTowards;
    }

    public void wander()
    {
        state = BehaviourState.walking;
    }

    public void runAwayFrom(GameObject obj, float distanceToRun, float tolerance=0F)
    {
        state = BehaviourState.running;
        //code adapted from http://answers.unity3d.com/questions/868003/navmesh-flee-ai-flee-from-player.html
        Quaternion newRotation = Quaternion.LookRotation(transform.position - obj.GetComponent<Transform>().position);

        newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, 0, newRotation.eulerAngles.z);

        Vector3 runTo = transform.position + newRotation.eulerAngles * (distanceToRun + tolerance);
        Debug.Log(runTo);

        //NavMeshHit hit;
        //NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));

        //navigation.SetDestination(hit.position);
        //Debug.Log(hit.position);
        navigation.SetDestination(runTo);
        navigation.stoppingDistance = tolerance;
        currentAction = BehaviourAction.runAway;
    }

    public void interruptSchedule()
    {
        scheduleInterrupted = true;
        currentBehaviour = null;
    }

    public void resumeSchedule()
    {
        scheduleInterrupted = false;
    }

    public void idle()
    {
        state = BehaviourState.idle;
        currentAction = BehaviourAction.idle;
    }

    public void setContinuation(Action continuation)
    {
        this.continuation = continuation;
    }
}

