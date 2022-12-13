using UnityEngine;
using System.Collections;
using UnityEngine.Purchasing;
using System;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;
using UnityEngine.Events;

public class InAppPurchaseController : MonoBehaviour
{
    public static InAppPurchaseController Instance { get; private set; }


    private CrossPlatformValidator validator;

    private UnityAction<Product> buyProductCallback;
    private UnityAction<Product> buyProductCallbackDefault;
    private UnityAction initializedCallback;

    private bool isRestoringPurchases;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }
}
    