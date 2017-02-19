using System;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        schedule = GetComponentsInChildren<AnimalBehaviour>();
        Array.Sort(schedule, (x, y) => x.startTime.CompareTo(y.startTime));
        //DEBUG
        foreach(AnimalBehaviour b in schedule)
        {
            Debug.Log(b.GetType().ToString());
        }
        foreach(BehaviourAnimation a in animations){
            animationDict.Add(a.state, a.animation);
        }
        animationController = GetComponent<Animation>();
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
                        currentBehaviour = b;
                        b.perform(this);
                        break;
                    }
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

    public void walkTo(Vector3 point)
    {
        state = BehaviourState.walking;
    }

    public void wander()
    {
        state = BehaviourState.walking;
    }

    public void runAwayFrom(GameObject obj)
    {
        state = BehaviourState.running;
    }

    public void idle()
    {
        state = BehaviourState.idle;
    }
}

