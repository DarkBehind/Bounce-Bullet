using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using UnityEngine;
using UnityEngine.UI;

public class DailyFreeCoinUI : PopupScreen
{
    [SerializeField]private GameObject notificationGameObject;
    [SerializeField]private Button claimButton;
    [SerializeField] private RewardItem _currentReward;
    
    public override void StartScreen()
    {
        base.StartScreen();
    }
        
    public override void CloseScreen()
    {
        base.CloseScreen();
    }

    void ChangeNotificationStatus(bool status)
    {
        notificationGameObject.SetActive(status);
        claimButton.interactable = status;
    }
    public void CheckDailyFreeCoin()
    {
        if (PlayerPrefs.HasKey(Extensions.Prefs.FreeCoinLastClaimedDay))
        {
            string lastClaimedDay = PlayerPrefs.GetString(Extensions.Prefs.FreeCoinLastClaimedDay);
            string currentDay = Extensions.GetCurrentDayByDate();
            if (!string.Equals(lastClaimedDay, currentDay))
            {
                ChangeNotificationStatus(true);
            }
            else
            {
                ChangeNotificationStatus(false);
            }
        }
        else
        {
            ChangeNotificationStatus(true);
        }
    }
    
    public void ClaimDailyFreeCoin()
    {
        ChangeNotificationStatus(false);
        UI_System.Instance.SwitchScreens(UI_System.Instance.startScreen);
        InventoryManager.Instance.ShowItemClaimPopup(_currentReward.itemSprite, _currentReward.stackAmount);
        InventoryManager.Instance.AddGold(_currentReward.stackAmount);
        Extensions.SetFreeCoinLastClaimedDay();
    }
}
