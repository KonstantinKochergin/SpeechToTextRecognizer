using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField]
    private Listener listener;

    float time = 0;

    void Start()
    {
        listener.StartRecording();
    }

    
    void Update()
    {
        time += Time.deltaTime;

        if(time > 10)
        {
            listener.StopRecording();
        }
    }
}
