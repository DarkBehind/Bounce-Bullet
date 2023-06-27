using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelListItemUI : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public Image lockImage;

    [HideInInspector]
    public bool isLocked;
    [HideInInspector]
    public int levelNumber;
    
    public void SetLevelListItemUI(int level, bool locked)
    {
        this.isLocked = locked;
        this.levelNumber = level;
        
        levelNumberText.text = level.ToString();
        lockImage.gameObject.SetActive(locked);
    }
}
