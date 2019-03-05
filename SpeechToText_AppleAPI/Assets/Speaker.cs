using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {
    public AudioClip[] sleeps;
    public List<AudioClip> hello;

    public void AddMessHello(AudioClip audio)
    {
        hello.Add(audio);
    }
    public void GoodNightMess()
    {
        AudioSource aud = GetComponent<AudioSource>();
        aud.clip = sleeps[Random.Range(0, sleeps.Length-1)];
        aud.pitch = 1.6F;
        aud.Play();
        aud.pitch = 1;
    }
    public void HelloMess()
    {
        AudioSource aud = GetComponent<AudioSource>();
        aud.clip = hello[Random.Range(0, hello.Count)];
        aud.pitch = 1.6F;
        aud.Play();
        aud.pitch = 1;
    }
}
