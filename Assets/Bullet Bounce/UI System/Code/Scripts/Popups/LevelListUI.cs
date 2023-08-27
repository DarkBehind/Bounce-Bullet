using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using UnityEngine;

public class LevelListUI : PopupScreen
{
    public LevelListItemUI exampleLevelListItemUI;

    public override void StartScreen(Action onStarted = null)
    {
        base.StartScreen(onStarted);
        SetLevelListUI();
    }

    private void SetLevelListUI()
    {
        exampleLevelListItemUI.gameObject.SetActive(false);
        
        foreach (Transform child in exampleLevelListItemUI.transform.parent)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
        
        for (int i = 1; i <= 25; i++) // 25 level oldugunu ifade ediyor
        {
            var newLevelListItemUI = Instantiate(exampleLevelListItemUI, exampleLevelListItemUI.transform.parent);
            newLevelListItemUI.SetLevelListItemUI(i, i > LevelManager.GetLastPlayedLevel());
            newLevelListItemUI.gameObject.SetActive(true);
        }
    }

    public void OnLevelClicked(LevelListItemUI itemUI)
    {
        if (itemUI.isLocked)
        {
            return;
        }
        
        Debug.Log("Level Loaded: " + itemUI.levelNumber);
        UI_System.Instance.SwitchScreens(UI_System.Instance.playScreen);
        LevelManager.Instance.LoadLevel(itemUI.levelNumber);

    }
}
