using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechComponent : MonoBehaviour
{
    public string[] sentences;

    private float lastTimeSpoken;

    public void RandomSpeak(float rate)
    {
        if(Time.time > lastTimeSpoken + rate)
        {
            int randomInt = Random.Range(0, sentences.Length);
            lastTimeSpoken = Time.time;

            Debug.LogFormat("{0}:\"{1}\"", name, sentences[randomInt]);
        }
    }
}
