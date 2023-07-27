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
        private bool clicked = false;
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
            if(clicked) return;
            clicked = true;
            StartScreen(() =>
            {
                clicked = false;
            });
        }

        public void Button_OnClaim()
        {
            if(clicked) return;
            clicked = true;
            ClaimRewardAndNextLevel(rewardItem);
        }

        public void Button_DoubleClaim()
        {
            if(clicked) return;
            clicked = true;
            ButtonsStatus(false);
            ADS.Instance.rewarded.LoadRewardedAd(null, () =>
            {
                InventoryItem doubleReward = rewardItem;
                doubleReward.stackAmount *= 2;
                ClaimRewardAndNextLevel(doubleReward);
                ButtonsStatus(true);
            });
        }
        
        void ClaimRewardAndNextLevel(InventoryItem reward)
        {
            InventoryManager.Instance.PurchaseItem(reward,true);
            LevelManager.Instance.LoadNextLevel();
            CloseScreen(() =>
            {
                clicked = false;
            });
        }
        
        void ButtonsStatus(bool status)
        {
            claimButton.interactable = status;
            doubleClaimButton.interactable = status;
        }
    }
}