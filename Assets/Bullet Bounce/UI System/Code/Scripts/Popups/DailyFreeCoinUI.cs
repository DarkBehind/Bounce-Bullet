using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyFreeCoinUI : PopupScreen
{
    [SerializeField]private GameObject notificationGameObject;
    [SerializeField]private Button claimButton;
    [SerializeField] private RewardItem _currentReward;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private int[] _maxAndMinRewardAmount;
    public override void StartScreen(Action onStarted = null)
    {
        base.StartScreen(onStarted);
    }
        
    public override void CloseScreen(Action onClosed = null)
    {
        base.CloseScreen(onClosed);
    }

    void ChangeNotificationStatus(bool status)
    {
        notificationGameObject.SetActive(status);
        claimButton.interactable = status;
        if (!status)
        {
            rewardText.text = "Claimed";
            rewardText.color = Color.gray;
        }
        else
            rewardText.text = _currentReward.stackAmount.ToString();
        
    }
    public void CheckDailyFreeCoin()
    {
        if (PlayerPrefs.HasKey(Extensions.Prefs.FreeCoinLastClaimedDay))
        {
            string lastClaimedDay = PlayerPrefs.GetString(Extensions.Prefs.FreeCoinLastClaimedDay);
            string currentDay = Extensions.GetCurrentDayByDate();
            if (!string.Equals(lastClaimedDay, currentDay))
            {
                _currentReward.stackAmount = UnityEngine.Random.Range(_maxAndMinRewardAmount[0], _maxAndMinRewardAmount[1]);
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
