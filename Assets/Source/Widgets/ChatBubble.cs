using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Chat bubble appears above soldiers heads when they talk.
/// </summary>
public class ChatBubble : Widget
{
    public Text textMessage;

    public void SetText(string message)
    {
        textMessage.text = message;
    }
}
