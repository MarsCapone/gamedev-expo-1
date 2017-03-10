using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour, ITimeBasedObject {
    
    public BehaviourState initialState;
    protected bool scheduleInterrupted = false;
    public abstract BehaviourState state
    {
        get;
        set;
    }
    protected Action continuation; //used for chains of multiple actions
    
    protected AnimalBehaviour currentBehaviour;

    protected AnimalBehaviour[] schedule;
    protected BehaviourInterrupt[] interrupts;
    protected BehaviourInterrupt currentInterrupt = null;

    protected BehaviourAction currentAction;

    public void Start()
    {
        schedule = GetComponentsInChildren<AnimalBehaviour>();
        Array.Sort(schedule, (x, y) => x.startTime.CompareTo(y.startTime));
        interrupts = GetComponentsInChildren<BehaviourInterrupt>();
        init();
    }

    public abstract void init();

    protected float lastTime;
    public void updateTime(float time)
    {
        lastTime = time;
        if (!scheduleInterrupted)
        {
            foreach (AnimalBehaviour b in schedule)
            {
                if (b.startTime <= time || !currentBehaviour)
                {
                    if (!currentBehaviour || (currentBehaviour != b && currentBehaviour.startTime < b.startTime))
                    {
                        Debug.Log(b.ToString());
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
        UpdateAndBehave();
        if (!scheduleInterrupted)
        {
            foreach (BehaviourInterrupt interrupt in interrupts)
            {
                if (interrupt.check())
                {
                    interruptSchedule(interrupt);
                    interrupt.trigger();
                }
            }
        }
    }


    public void newDay()
    {
        currentBehaviour = null;
    }

    public void interruptSchedule(BehaviourInterrupt bi)
    {
        currentInterrupt = bi;
        scheduleInterrupted = true;
        currentBehaviour = null;
    }

    public void resumeSchedule()
    {
        currentInterrupt = null;
        scheduleInterrupted = false;
    }

    //each animal should act out actions in a frame-by-frame manner within this method
    public abstract void UpdateAndBehave();


    public abstract void sleep();

    public abstract void walkTo(Vector3 location, float tolerance = 0F);

    public abstract void wander();

    public abstract void runAwayFrom(GameObject obj, float distanceToRun, float tolerance = 0F);

    public void idle()
    {
        state = BehaviourState.idle;
        currentAction = BehaviourAction.idle;
    }

    public virtual void eat(float hoursToEatFor)
    {
        state = BehaviourState.eating;
        float eatEndTime = lastTime + hoursToEatFor;
        while(eatEndTime > 24)
        {
            eatEndTime -= 24;
        }
    }

    public void setContinuation(Action continuation)
    {
        this.continuation = continuation;
    }
}

