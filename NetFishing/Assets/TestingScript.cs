using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class TestingScript : MonoBehaviour
{
    public ChatLogManager chatLog;

    void Start()
    {
        chatLog.AddMessage("Hello!");
        chatLog.AddMessage("Welcome to the game!");
    }
}
