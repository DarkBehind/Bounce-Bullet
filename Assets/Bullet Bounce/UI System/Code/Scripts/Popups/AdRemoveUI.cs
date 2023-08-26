using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace S_Durlanik.UI
{

    public class AdRemoveUI : PopupScreen
    {
        public override void StartScreen(Action onStarted = null)
        {
            base.StartScreen(onStarted);
        }
        
        public override void CloseScreen(Action onClosed = null)
        {
            base.CloseScreen(onClosed);
        }
        public void ShowScreen()
        {
            StartScreen();
        }
        public void LaterButton_OnClick()
        {
            CloseScreen();
        }
        public void OnRemoveAdsComplete()
        {
            Debug.Log("Ads Removed!");
            CloseScreen();
        }
        
        public void OnRemoveAdsFailed()
        {
            Debug.Log("Ads Remove Failed!");
        }
    }
}