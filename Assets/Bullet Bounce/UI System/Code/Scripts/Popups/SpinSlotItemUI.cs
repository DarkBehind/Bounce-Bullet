using S_Durlanik.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinSlotItemUI : MonoBehaviour
{
    public int slotID;
    public Image itemImage;
    public TextMeshProUGUI amountText;
    
    public void SetSpinSlotUI(SpinItem spinItem)
    {
        itemImage.sprite = spinItem.itemSprite;
        amountText.text =$"x{spinItem.stackAmount}";
    }
}
