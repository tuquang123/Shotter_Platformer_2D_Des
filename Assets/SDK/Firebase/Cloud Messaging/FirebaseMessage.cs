using Firebase.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseMessage : MonoBehaviour
{
    void Awake()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;

        DontDestroyOnLoad(this);
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message: " + e.Message);
    }
}
