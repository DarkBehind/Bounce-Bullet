using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace S_Durlanik.UI
{
    public class StageClearUI : PopupScreen
    {
        [SerializeField] TextMeshProUGUI rewardAmountText;
        [SerializeField] InventoryItem rewardItem;
        [SerializeField] Button claimButton;
        [SerializeField] Button doubleClaimButton;
        private void OnEnable()
        {
            LevelManager.OnLevelCompleted += OnLevelCompleted;
            
            rewardAmountText.text ="+" +rewardItem.stackAmount;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelCompleted -= OnLevelCompleted;

        }

        private void OnLevelCompleted()
        {
            StartScreen();
        }

        public void Button_OnClaim()
        {
            ClaimRewardAndNextLevel(rewardItem);
            CloseScreen();
        }

        public void Button_DoubleClaim()
        {
            ButtonsStatus(false);
            ADS.Instance.rewarded.LoadRewardedAd(null, () =>
            {
                InventoryItem doubleReward = rewardItem;
                doubleReward.stackAmount *= 2;
                ClaimRewardAndNextLevel(doubleReward);
                ButtonsStatus(true);
                CloseScreen();
            });
        }
        
        void ClaimRewardAndNextLevel(InventoryItem reward)
        {
            InventoryManager.Instance.PurchaseItem(reward,true);
            LevelManager.Instance.LoadNextLevel();
            CloseScreen();
        }
        
        void ButtonsStatus(bool status)
        {
            claimButton.interactable = status;
            doubleClaimButton.interactable = status;
        }
    }
}