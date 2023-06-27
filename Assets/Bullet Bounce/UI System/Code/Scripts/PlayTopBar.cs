using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using TMPro;
using UnityEngine;

public class PlayTopBar : MonoBehaviour
{
   
    public TextMeshProUGUI bulletCountText;
    public TextMeshProUGUI goldText;

    public void SetPlayTopBar()
    {
        bulletCountText.text = LevelManager.Instance.maxShotCount.ToString();
        goldText.text = InventoryManager.Instance.UserGold.stackAmount.ToString();
    }

    public void UpdateBulletCountText()
    {
        bulletCountText.text = LevelManager.Instance.maxShotCount.ToString();
    }
    
    public void UpdateGoldText()
    {
        goldText.text = InventoryManager.Instance.UserGold.stackAmount.ToString();
    }
}
