using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Recorder : MonoBehaviour
{
    AudioClip reco1, reco2, reco3, recoChild, recoHello;
    float currentTime;
    int index = 1;
    bool isRecord = false;

    public void StartRecord1()
    {
        if (!Microphone.IsRecording(null))
        {
            reco1 = Microphone.Start(null, false, 15, 44100);
            isRecord = true;
            index = 1;
        }
    }
    public void Play1() 
    {
        Microphone.End(null);
        Load("record1.wav");
    }
    public void StartRecord2()
    {
        if (!Microphone.IsRecording(null))
        {
            reco2 = Microphone.Start(null, false, 15, 44100);
            isRecord = true;
            index = 2;
        }
    }
    public void Play2()
    {
        Microphone.End(null);
        Load("record2.wav");
    }
    public void StartRecord3()
    {
        if (!Microphone.IsRecording(null))
        {
            reco3 = Microphone.Start(null, false, 15, 44100);
            isRecord = true;
            index = 3;
        }
    }
    public void Play3()
    {
        Microphone.End(null);
        Load("record3.wav");
    }
    public void StartRecordHello()
    {
        if (!Microphone.IsRecording(null))
        {
            recoHello = Microphone.Start(null, false, 15, 44100);
            isRecord = true;
            index = 4;
        }
        GameObject.Find("Canvas").GetComponent<Speaker>().AddMessHello(recoHello);
    }
    public void PlayHello()
    {
        Microphone.End(null);
        Load("recordHello.wav");
    }
    public void StartRecordChild()
    {
        if (!Microphone.IsRecording(null))
        {
            recoChild = Microphone.Start(null, false, 15, 44100);
            isRecord = true;
            index = 5;
        }
    }
    public void PlayChild()
    {
        Microphone.End(null);
        Load("recordChild.wav");
        AudioSource aud = GetComponent<AudioSource>();
        aud.pitch = 1;
    }
    public void End1()
    {
        Microphone.End(null);
        SavWav.Save("record1", reco1);
    }
    public void End2()
    {
        Microphone.End(null);
        SavWav.Save("record2", reco2);
    }
    public void End3()
    {
        Microphone.End(null);
        SavWav.Save("record3", reco3);
    }
    public void EndHello()
    {
        Microphone.End(null);
        SavWav.Save("recordHello", recoHello);
    }
    public void Load(string s)
    {
        AudioSource aud;
        var filepath = Path.Combine(Application.persistentDataPath, s);
        WWW audioLoader = new WWW("file://" + filepath);
        Debug.Log(filepath);
        while (!audioLoader.isDone)
            Debug.Log("upload");
        aud = GetComponent<AudioSource>();
        aud.clip = audioLoader.GetAudioClip(false, false, AudioType.WAV);
        aud.pitch = 1.6f;
        aud.Play();
    }
    public void Delete1()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "record1.wav"));
    }
    public void Delete2()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "record2.wav"));
    }
    public void Delete3()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "record3.wav"));
    }
    public void DeleteHello()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "recordHello.wav"));
    }
    public void DeleteChild()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "recordChild.wav"));
    }
    private void Update()
    {
        if (isRecord)
            currentTime += Time.deltaTime;
        if (currentTime > 15)
        {
            switch (index)
            {
                case 1:
                    SavWav.Save("record1", reco1);
                    currentTime = 0;
                    isRecord = false;
                    break;
                case 2:
                    SavWav.Save("record2", reco1);
                    currentTime = 0;
                    isRecord = false;
                    break;
                case 3:
                    SavWav.Save("record3", reco1);
                    currentTime = 0;
                    isRecord = false;
                    break;
                case 4:
                    SavWav.Save("recordHello", reco1);
                    currentTime = 0;
                    isRecord = false;
                    break;
                case 5:
                    SavWav.Save("recordChild", reco1);
                    currentTime = 0;
                    isRecord = false;
                    break;
            }
        }
    }
}
