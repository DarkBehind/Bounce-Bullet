using System.Collections;
using System.Collections.Generic;
using S_Durlanik.UI;
using UnityEngine;

namespace S_Durlanik.UI
{
    public class PauseUI : PopupScreen
    {
        public GameObject settingsUI;
        
        public void OnSettings()
        {
            settingsUI.SetActive(true);
        }
        
        public void OnContinue()
        {
            
        }

        public void OnRestart()
        {
            
        }

        public void OnMenu()
        {

        }

    }
}
