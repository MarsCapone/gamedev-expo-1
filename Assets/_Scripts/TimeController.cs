using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    //time speed variables
    public float minutesPerFrame;
    public float initialTime;
    float time;

    //object references
    public Sunlight sun;


    // Use this for initialization
    void Start()
    {
        time = initialTime;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        float minute = 1.0F / 60.0F;
        time += minute * minutesPerFrame;
        time = time % 24;
        sun.updateTime(time);
    }
}
