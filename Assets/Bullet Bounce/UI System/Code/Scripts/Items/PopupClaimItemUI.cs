using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace S_Durlanik.UI
{
    public class PopupClaimItemUI : MonoBehaviour
    {
        public Image itemImage;
        public TextMeshProUGUI amountText;
        
        public void SetPopupClaimItemUI(Sprite _itemImage, int _amount)
        {
            itemImage.sprite = _itemImage;
            amountText.text =$"x{_amount}";
        }
    }
}
