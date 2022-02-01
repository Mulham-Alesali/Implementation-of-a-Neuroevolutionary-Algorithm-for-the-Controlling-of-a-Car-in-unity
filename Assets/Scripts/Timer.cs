using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Timer class contains information about the roundtime
/// </summary>
public class Timer : MonoBehaviour
{
    public static float Time { get; set; } = 0.0f;
   
    public static int Seconds { get; set; }
   
    // Start is called before the first frame update

    void Start()
    {
        Time = 0f;
        Seconds = 0;
    }

    /// <summary>
    /// Reset the information of the class, for example in case a new simulatin starts
    /// </summary>
    public static void Reset()
    {
        Time = 0;
        Seconds = 0;
    }

    // Update is called once per frame
    void Update()
    {


        if (SimulationController.SimulationState == SimulationController.State.stoped)
       {
           return;
       }    
       
        // seconds in float
        Time += UnityEngine.Time.deltaTime;
        // turn seconds in float to int
        Seconds = (int)(Time);
       
    }
}
