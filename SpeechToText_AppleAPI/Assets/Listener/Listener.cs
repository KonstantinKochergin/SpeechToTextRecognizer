using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;


public class Listener : MonoBehaviour
{

    private Speaker speaker;
    private Recorder recorder;

    public Text debText;

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        
             
       /* SpeechToText.instance.onResultCallback = onResultCallback;
#if UNITY_ANDROID
        SpeechToText.instance.onReadyForSpeechCallback = onReadyForSpeechCallback;
        SpeechToText.instance.onEndOfSpeechCallback = onEndOfSpeechCallback;
        SpeechToText.instance.onRmsChangedCallback = onRmsChangedCallback;
        SpeechToText.instance.onBeginningOfSpeechCallback = onBeginningOfSpeechCallback;
        SpeechToText.instance.onErrorCallback = onErrorCallback;
        SpeechToText.instance.onPartialResultsCallback = onPartialResultsCallback;
#else
        gameObject.SetActive(false);
#endif*/
    }

    public void InitAndStart()
    {
        if(audioSrc == null)
        {
            audioSrc = GetComponent<AudioSource>();
        }
        Setting("ru");
        SpeechToText.instance.onResultCallback = OnResultSpeech;
        speaker = this.gameObject.GetComponent<Speaker>();
        recorder = this.gameObject.GetComponent<Recorder>();

        BeginRecord();
    }

    void onResultCallback(string _data)
    {
        
    }

    void onReadyForSpeechCallback(string _params)
    {
        
    }
    void onEndOfSpeechCallback()
    {
        //listener.StartRecording();      //начало слушание пользователя 
        
    }
    void onRmsChangedCallback(float _value)
    {
        
        
    }
    void onBeginningOfSpeechCallback()
    {
       
    }
    void onErrorCallback(string _params)
    {
        //StartRecording();     //начинаем слушать при ошибке
        
    }
    void onPartialResultsCallback(string _params)
    {
        
    }

    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        SpeechToText.instance.StartRecording("Speak any");
#endif
    }

    public void StopRecording()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        SpeechToText.instance.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(true);
#endif
    }

    public void Setting(string code)
    {
        SpeechToText.instance.Setting(code);
    }

    void OnResultSpeech(string data)
    {
        data = data.ToLower();
        debText.text = data;
        if (data.Contains("желаю") || data.Contains("хочу") || data.Contains("прошу"))
        {
            debText.text = "желание";
            recorder.StartRecordChild();
        }
        if (data.Contains("спать"))
        {
            debText.text = "спокойной ночи";
            speaker.GoodNightMess();
        }
        if (data.Contains("привет"))
        {
            debText.text = "приветствие";
            speaker.HelloMess();
        }
        BeginRecord();
    }

    public void BeginRecord()
    {
        if (!audioSrc.isPlaying)
        {
            StartRecording();
        }
    }
}
