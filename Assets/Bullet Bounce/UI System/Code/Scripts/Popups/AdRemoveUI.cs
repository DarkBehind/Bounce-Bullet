using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace S_Durlanik.UI
{

    public class AdRemoveUI : PopupScreen
    {
        public override void StartScreen()
        {
            base.StartScreen();
        }
        
        public override void CloseScreen()
        {
            base.CloseScreen();
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