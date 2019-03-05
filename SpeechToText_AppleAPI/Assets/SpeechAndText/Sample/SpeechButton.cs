using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class SpeechButton : MonoBehaviour
{
   
    [SerializeField]
    TextMesh statusText;

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
        if(time > 10)
        {
            listener.StopRecording();
            time = 0;
            listener.StartRecording();
        }
        statusText.text = ""+time;
    }




}
