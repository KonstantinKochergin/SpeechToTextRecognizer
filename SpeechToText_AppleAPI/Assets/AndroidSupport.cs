using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidSupport : MonoBehaviour
{
    
    void Start()
    {

    }

    
    void Update()
    {
        ShowToast();
    }

    private void ShowToast()
    {

#if UNITY_ANDROID

        AndroidJavaClass toastClass =
                   new AndroidJavaClass("android.widget.Toast");

        object[] toastParams = new object[3];
        AndroidJavaClass unityActivity =
          new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        toastParams[0] =
                     unityActivity.GetStatic<AndroidJavaObject>
                               ("currentActivity");
        toastParams[1] = "This is a Toast";
        toastParams[2] = toastClass.GetStatic<int>
                               ("LENGTH_LONG");

        AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>
                                      ("makeText", toastParams);
        //toastObject.Call("show");

#endif
    }

    private void MuteRecognitionBeep()
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaClass>("currentActivity");
        AndroidJavaObject audioManager = activity.Call<AndroidJavaObject>("getSystemService", "audio");
        audioManager.Call<int>("requestAudioFocus");
        object[] parms = new object[2];
        
#endif
    }
}
