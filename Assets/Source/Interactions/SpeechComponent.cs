using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechComponent : MonoBehaviour
{
    public float height;

    [Multiline]
    public string[] randomSentences;
    [Multiline]
    public string[] instructions;

    private float lastTimeSpoken;
    private ChatBubble chatBubble;
    private int currentInstruction;


    private void Start()
    {
        chatBubble = GameInstance.GameMode.HUD.SpawnWidget<ChatBubble>();
        chatBubble.gameObject.SetActive(false);

        // Reduce initial delay for speach by 2 seconds.
        // This is to make the first message appear faster
        // than it normally would if it used the talk rate
        lastTimeSpoken = -2;
    }


    private void Update()
    {
        chatBubble.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, height, transform.position.z), Camera.MonoOrStereoscopicEye.Mono);
    }


    public void InstructionsSpeak(float rate)
    {
        if(currentInstruction >= instructions.Length)
        {
            return;
        }

        if (Time.time > lastTimeSpoken + rate)
        {
            lastTimeSpoken = Time.time;

            chatBubble.SetText(instructions[currentInstruction++]);
            chatBubble.gameObject.SetActive(true);

            if(currentInstruction == instructions.Length)
            {
                Invoke("DisableChatBubble", rate);
            }
        }
    }

    public void RandomSpeak(float rate)
    {
        if (Time.time > lastTimeSpoken + rate)
        {
            int randomInt = Random.Range(0, randomSentences.Length);
            lastTimeSpoken = Time.time;

            chatBubble.SetText(randomSentences[randomInt]);
            chatBubble.gameObject.SetActive(true);

            Invoke("DisableChatBubble", 4.5f);
        }
    }

    private void DisableChatBubble()
    {
        chatBubble.gameObject.SetActive(false);
    }
}
