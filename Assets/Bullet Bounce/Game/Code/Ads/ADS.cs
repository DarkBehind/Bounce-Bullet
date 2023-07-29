using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
public class ADS : MonoBehaviour
{
    public Rewarded rewarded;
    public InterstitialAds interstitialAds;
    public static ADS Instance;
    
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    public void RemoveAds()
    {
        Debug.Log("RemoveAds");
    }
}
