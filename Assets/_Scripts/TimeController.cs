using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    //time speed variables
    public float minutesPerFrame;
    public float initialTime;
    private float time;

    //object references
    public Sunlight sun;
    public GameObject animals;

    public List<ITimeBasedObject> timeBasedObjects = new List<ITimeBasedObject>();

    // Use this for initialization
    void Start()
    {
        time = initialTime;
        Update();
        timeBasedObjects.Add(sun);
        timeBasedObjects.AddRange(animals.GetComponentsInChildren<Animal>());
    }

    // Update is called once per frame
    void Update()
    {
        float minute = 1.0F / 60.0F;
        time += minute * minutesPerFrame;
        bool newDay = false;
        if (time > 24) {
            time = time % 24;
            newDay = true;
        }
        foreach(ITimeBasedObject itbo in timeBasedObjects) {
            if (newDay) itbo.newDay();
            itbo.updateTime(time);
        }
    }

    public void registerTimeBasedObject(ITimeBasedObject obj)
    {
        timeBasedObjects.Add(obj);
    }

    public void unregisterTimeBasedObject(ITimeBasedObject obj)
    {
        timeBasedObjects.Remove(obj);
    }
}

public interface ITimeBasedObject
{
    void updateTime(float t);
    void newDay();
}