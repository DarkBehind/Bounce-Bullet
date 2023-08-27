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
        public void OnContinue()
        {
            Time.timeScale = 1;
            screenSwitching = false;
            UI_System.Instance.SwitchScreens(UI_System.Instance.playScreen);
        }
        public void OnRestart()
        {
            Time.timeScale = 1;
            CloseScreen();
            LevelManager.Instance.RestartLevel();
            UI_System.Instance.SwitchScreens(UI_System.Instance.playScreen);
        }

        public void OnMenu()
        {
            Time.timeScale = 1;
            LevelManager.Instance.ReturnToMenu();
        }

    }
}
