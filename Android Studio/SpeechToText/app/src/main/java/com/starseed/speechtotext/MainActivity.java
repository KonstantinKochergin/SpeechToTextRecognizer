package com.starseed.speechtotext;

import android.content.Context;
import android.content.Intent;
import android.media.AudioManager;
import android.os.Bundle;
import android.provider.MediaStore;
import android.speech.RecognitionListener;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Locale;


public class MainActivity extends UnityPlayerActivity
{
    private TextToSpeech tts;
    private SpeechRecognizer speech;
    private Intent intent;

    private int startVolume = 0;
    private AudioManager audioManager;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        tts = new TextToSpeech(this, initListener);
        audioManager = (AudioManager) getSystemService(Context.AUDIO_SERVICE);

        speech = SpeechRecognizer.createSpeechRecognizer(this);
        speech.setRecognitionListener(recognitionListener);


    }
    @Override
    public void onDestroy() {
        // Don't forget to shutdown tts!
        if (tts != null) {
            tts.stop();
            tts.shutdown();
        }
        if (speech != null) {
            speech.destroy();
        }
        super.onDestroy();
    }
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK && null != data) {
            ArrayList<String> text = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
            UnityPlayer.UnitySendMessage("SpeechToText", "onResults", text.get(0));
        }
    }

    // speech to text
    public void OnStartRecording() {

        //отключение звука, при пиликанье
        startVolume = audioManager.getStreamVolume(AudioManager.STREAM_MUSIC);
        audioManager.adjustStreamVolume(AudioManager.STREAM_MUSIC, AudioManager.ADJUST_MUTE, 0);
        //Toast.makeText(this, "Выключаем музыкальный поток " + audioManager.isMicrophoneMute(), Toast.LENGTH_SHORT).show();

        intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_PREFERENCE, Bridge.languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, Bridge.languageSpeech);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE, Bridge.languageSpeech);
        //intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_MINIMUM_LENGTH_MILLIS, 2000);
        intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_COMPLETE_SILENCE_LENGTH_MILLIS, 2000);
        //intent.putExtra(RecognizerIntent.EXTRA_SPEECH_INPUT_POSSIBLY_COMPLETE_SILENCE_LENGTH_MILLIS, 2000);
        intent.putExtra(RecognizerIntent.EXTRA_CALLING_PACKAGE, this.getPackageName());
        intent.putExtra(RecognizerIntent.EXTRA_MAX_RESULTS, 3);

        this.runOnUiThread(new Runnable() {

            @Override
            public void run() {
                speech.startListening(intent);
            }
        });
        UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "CallStart, Language:" + Bridge.languageSpeech);

        new BeepMuteThread(audioManager, this, startVolume, "BeepMuteThread").start();
        Log.i("Unity", "строка после старта потока");
        //Toast.makeText(this, "До потока", Toast.LENGTH_SHORT).show();
        /*try{
            Thread.sleep(300);
            audioManager.setStreamVolume(AudioManager.STREAM_MUSIC, startVolume, 0);
            Toast.makeText(this, "Включаем музакальный поток " + audioManager.isMicrophoneMute(), Toast.LENGTH_SHORT).show();
        }
        catch (Exception e){
            Log.e("Unity problem :D", e.toString());
        }*/
    }
    public void OnStopRecording() {
        this.runOnUiThread(new Runnable() {

            @Override
            public void run() {
                speech.stopListening();
            }
        });
        UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "CallStop");
        //Toast.makeText(this, "OnStopRecording", Toast.LENGTH_SHORT).show();
        Log.i("Unity", "OnStopRecording");
        //включаем музыкальный поток
        //audioManager.setStreamVolume(AudioManager.STREAM_MUSIC, startVolume, 0);
    }

    RecognitionListener recognitionListener = new RecognitionListener() {
        @Override
        public void onReadyForSpeech(Bundle params) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onReadyForSpeech", params.toString());
        }
        @Override
        public void onBeginningOfSpeech() {
            UnityPlayer.UnitySendMessage("SpeechToText", "onBeginningOfSpeech", "");
        }
        @Override
        public void onRmsChanged(float rmsdB) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onRmsChanged", "" + rmsdB);
        }
        @Override
        public void onBufferReceived(byte[] buffer) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "onBufferReceived: " + buffer.length);
        }
        @Override
        public void onEndOfSpeech() {
            UnityPlayer.UnitySendMessage("SpeechToText", "onEndOfSpeech", "");
        }
        @Override
        public void onError(int error) {
            UnityPlayer.UnitySendMessage("SpeechToText", "onError", "" + error);
        }
        @Override
        public void onResults(Bundle results) {
            //oast.makeText(getApplicationContext(), "OnResults", Toast.LENGTH_LONG).show();
            ArrayList<String> text = results.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
            UnityPlayer.UnitySendMessage("SpeechToText", "onResults", text.get(0));
        }
        @Override
        public void onPartialResults(Bundle partialResults) {
            ArrayList<String> text = partialResults.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
            UnityPlayer.UnitySendMessage("SpeechToText", "onPartialResults", text.get(0));
        }
        @Override
        public void onEvent(int eventType, Bundle params) {

            UnityPlayer.UnitySendMessage("SpeechToText", "onMessage", "onEvent");
        }
    };


    ////
    public  void OnStartSpeak(String valueText)
    {
        HashMap<String, String> map = new HashMap<String, String>();
        map.put(TextToSpeech.Engine.KEY_PARAM_UTTERANCE_ID, valueText);
        tts.speak(valueText, TextToSpeech.QUEUE_FLUSH, map);
    }
    public void OnSettingSpeak(String language, float pitch, float rate) {
        tts.setPitch(pitch);
        tts.setSpeechRate(rate);
        int result = tts.setLanguage(getLocaleFromString(language));
        UnityPlayer.UnitySendMessage("TextToSpeech", "onSettingResult", "" + result);
    }
    public void OnStopSpeak()
    {
        tts.stop();
        //Toast.makeText(this, "OnStopSpeak", Toast.LENGTH_SHORT).show();
        Log.i("Unity", "OnStopSpeak");
    }

    TextToSpeech.OnInitListener initListener = new TextToSpeech.OnInitListener()
    {
        @Override
        public void onInit(int status) {
            if (status == TextToSpeech.SUCCESS)
            {
                OnSettingSpeak(Locale.KOREA.toString(), 1.0f, 1.0f);
                tts.setOnUtteranceProgressListener(utteranceProgressListener);
            }
        }
    };
    UtteranceProgressListener utteranceProgressListener=new UtteranceProgressListener() {
        @Override
        public void onStart(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "onStart", utteranceId);
        }
        @Override
        public void onError(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "onError", utteranceId);
        }
        @Override
        public void onDone(String utteranceId) {
            UnityPlayer.UnitySendMessage("TextToSpeech", "onDone", utteranceId);
        }
    };

    /**
     * Convert a string based locale into a Locale Object.
     * Assumes the string has form "{language}_{country}_{variant}".
     * Examples: "en", "de_DE", "_GB", "en_US_WIN", "de__POSIX", "fr_MAC"
     *
     * @param localeString The String
     * @return the Locale
     */
    public static Locale getLocaleFromString(String localeString)
    {
        if (localeString == null)
        {
            return null;
        }
        localeString = localeString.trim();
        if (localeString.toLowerCase().equals("default"))
        {
            return Locale.getDefault();
        }

        // Extract language
        int languageIndex = localeString.indexOf('_');
        String language = null;
        if (languageIndex == -1)
        {
            // No further "_" so is "{language}" only
            return new Locale(localeString, "");
        }
        else
        {
            language = localeString.substring(0, languageIndex);
        }

        // Extract country
        int countryIndex = localeString.indexOf('_', languageIndex + 1);
        String country = null;
        if (countryIndex == -1)
        {
            // No further "_" so is "{language}_{country}"
            country = localeString.substring(languageIndex+1);
            return new Locale(language, country);
        }
        else
        {
            // Assume all remaining is the variant so is "{language}_{country}_{variant}"
            country = localeString.substring(languageIndex+1, countryIndex);
            String variant = localeString.substring(countryIndex+1);
            return new Locale(language, country, variant);
        }
    }
}

class BeepMuteThread extends Thread
{

    private AudioManager audioManager;
    private Context context;
    private int startVolume;

    public BeepMuteThread(AudioManager audioManager, Context context, int startVolume, String name){
        super(name);
        this.audioManager = audioManager;
        this.context = context;
        this.startVolume = startVolume;
    }

    @Override
    public void run() {
        super.run();
        try{
            Thread.sleep(300);
            audioManager.setStreamVolume(AudioManager.STREAM_MUSIC, startVolume, 0);
            Log.i("Unity", "startVolume = " + startVolume);
        }
        catch (Exception e){
            Log.e("Unity problem :D", e.toString());
        }
    }
}

