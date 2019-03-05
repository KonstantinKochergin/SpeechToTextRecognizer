using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class ListenerController : MonoBehaviour
{


    float time = 0;
    int a = 0;

    public Listener listener;

    void Start()
    {
        listener.InitAndStart();
    }

    void Update()
    {

        time += Time.deltaTime;
        if (time > 6)
        {
            listener.StopRecording();
            time = 0;
            listener.BeginRecord();
        }
    }




}