using Firebase.DynamicLinks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLinksHelper : Singleton<DynamicLinksHelper>
{
    void Awake()
    {
        DynamicLinks.DynamicLinkReceived += OnDynamicLinkReceived;
        DontDestroyOnLoad(this);
    }

    private void OnDynamicLinkReceived(object sender, ReceivedDynamicLinkEventArgs e)
    {
        DebugCustom.Log(e.ReceivedDynamicLink.Url.OriginalString);
    }
}
