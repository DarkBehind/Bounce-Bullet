using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace S_Durlanik.UI
{
    public class DailyLoginUI : PopupScreen
    {
        public List<DailyLoginReward> dailyLoginRewards;
        
        public DailyLoginItemUI exampleDailyLoginItemUI;
        public DailyLoginItemUI exampleSpecialDailyLoginItemUI; // 5. gun = special item
        
        public GameObject claimButton;
        public GameObject claim2XButton;
        public GameObject todayClaimedWarning;
        
        private DailyLoginReward _currentReward;
        public override void StartScreen()
        {
            base.StartScreen();
            SetDailyLoginUI();  
        }
        
        public override void CloseScreen()
        {
            base.CloseScreen();
        }
        
        private void SetDailyLoginUI()
        {
            exampleDailyLoginItemUI.gameObject.SetActive(false);
            exampleSpecialDailyLoginItemUI.gameObject.SetActive(false);
            
            foreach (Transform child in exampleDailyLoginItemUI.transform.parent)
            {
                if (child.gameObject.activeSelf)
                {
                    Destroy(child.gameObject);
                }
            }
            
            foreach (Transform child in exampleSpecialDailyLoginItemUI.transform.parent)
            {
                if (child.gameObject.activeSelf && child.GetComponent<DailyLoginItemUI>())
                {
                    Destroy(child.gameObject);
                }
            }
            
            foreach (var reward in dailyLoginRewards)
            {
                var instantiatedItem = reward.day == 5 ? exampleSpecialDailyLoginItemUI : exampleDailyLoginItemUI;
                Transform parent = reward.day == 5 ? exampleSpecialDailyLoginItemUI.transform.parent : exampleDailyLoginItemUI.transform.parent;
                
                var newDailyLoginItemUI = Instantiate(instantiatedItem, parent);

                if (reward.day == 5)
                {
                    newDailyLoginItemUI.transform.SetSiblingIndex(newDailyLoginItemUI.transform.parent.childCount - 2);
                }
                
                newDailyLoginItemUI.gameObject.SetActive(true);
                
                reward.isCurrentReward = reward.day == Extensions.GetCurrentDay();

                if (reward.day < Extensions.GetCurrentDay())
                {
                    reward.hasRewardClaimed = true;
                }
                else if (reward.day == Extensions.GetCurrentDay())
                {
                    reward.hasRewardClaimed = Extensions.HasCurrentDayRewardTaken() == 1;
                }
                else
                {
                    reward.hasRewardClaimed = false;
                }
                
                if (reward.isCurrentReward)
                {
                    _currentReward = reward;
                }
                
                newDailyLoginItemUI.SetDailyLoginItemUI(reward);
            }
            
            if (_currentReward == null || _currentReward.hasRewardClaimed)
            {
                claimButton.SetActive(false);
                claim2XButton.SetActive(false);
                todayClaimedWarning.SetActive(true);
                return;
            }
            
            claimButton.SetActive(true);
            claim2XButton.SetActive(true);
            todayClaimedWarning.SetActive(false);
            
        }

        public void ClaimReward()
        {
            InventoryManager.Instance.ShowItemClaimPopup(_currentReward.itemIcon, _currentReward.amount);
            InventoryManager.Instance.AddGold(_currentReward.amount);
            Extensions.SetCurrentDayRewardTaken();
        }
        
        public void Claim2XReward()
        {
            // reklam goster => basarili ise odul popupi ac
        }
    }
    
    [Serializable]
    public class DailyLoginReward
    {
        public int day;
        public int amount;
        public Sprite itemIcon;
        public bool hasRewardClaimed;
        public bool isCurrentReward;
    }
}