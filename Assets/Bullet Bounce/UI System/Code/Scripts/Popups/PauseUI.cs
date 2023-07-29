using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
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

        public void OnRestart()
        {
            LevelManager.Instance.RestartLevel();
        }

        public void OnMenu()
        {
            LevelManager.Instance.ReturnToMenu();
        }

    }
}
