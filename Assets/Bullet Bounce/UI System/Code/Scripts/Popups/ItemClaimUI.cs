using System.Collections;
using System.Collections.Generic;
using S_Durlanik.UI;
using UnityEngine;

namespace S_Durlanik.UI
{
    public class ItemClaimUI: MonoBehaviour
    {
        public PopupClaimItemUI examplePopupClaimItemUI;
        public void Show(DailyLoginReward reward)
        {
            examplePopupClaimItemUI.gameObject.SetActive(false);
            foreach (Transform child in examplePopupClaimItemUI.transform.parent)
            {
                if (child.gameObject.activeSelf)
                {
                    Destroy(child.gameObject);
                }
            }
            
            StartCoroutine(ShowItemClaimUI(reward));
        }
        
        private IEnumerator ShowItemClaimUI(DailyLoginReward reward)
        {
            yield return new WaitForSeconds(.5f);
            examplePopupClaimItemUI.gameObject.SetActive(true);
            examplePopupClaimItemUI.SetPopupClaimItemUI(reward);
        }
    }
}
