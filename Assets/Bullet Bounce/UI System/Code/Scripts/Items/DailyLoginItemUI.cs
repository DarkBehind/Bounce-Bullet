using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace S_Durlanik.UI
{
    public class DailyLoginItemUI : MonoBehaviour
    {
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI amountText;
        public Image itemImage;
        public Image clearRewardImage;
        public GameObject currentRewardImage;
        
        public void SetDailyLoginItemUI(DailyLoginReward reward)
        {
            dayText.text = $"Day {reward.day}";
            amountText.text = reward.amount.ToString();
            itemImage.sprite = reward.itemIcon;
            clearRewardImage.gameObject.SetActive(reward.hasRewardClaimed);
            currentRewardImage.SetActive(reward.isCurrentReward);
        }
    }
}
