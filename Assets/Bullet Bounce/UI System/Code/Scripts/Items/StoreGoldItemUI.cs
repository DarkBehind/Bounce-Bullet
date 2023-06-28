using System.Collections;
using System.Collections.Generic;
using System.Linq;
using S_Durlanik.Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace S_Durlanik.UI
{
    public class StoreGoldItemUI : MonoBehaviour
    {
        public TextMeshProUGUI goldAmountText;
        public TextMeshProUGUI priceText;
        public Image icon;
        public Image bestOfferIcon;
        public GameObject watchAdsButton;

        private string _itemCode;
        int _goldAmount;
        public void SetStoreGoldItemUI(GoldOffer offer)
        {
            _itemCode = offer.itemCode;
            goldAmountText.text = offer.goldAmount.ToString();
            _goldAmount = offer.goldAmount;
            icon.sprite = offer.icon;
            bestOfferIcon.gameObject.SetActive(offer.isBestOffer);

            if (offer.canEarnByWatchingAds)
            {
                priceText.gameObject.SetActive(false);
                watchAdsButton.SetActive(true);
            }
            else
            {
                priceText.gameObject.SetActive(true);
                watchAdsButton.SetActive(false);
                priceText.text = offer.priceString;
            }
        }

        public void OnOfferClicked()
        {
            if (_itemCode == StoreScreen.Instance.allGoldOffers.First().itemCode)
            {
                // allGoldOffers listesinin ilk elemani daima reklam izlenebilir olacak
                // bu yuzden ilk elemanin satin alinmasi durumunda reklam izleme ekrani acilacak
                OnWatchAds();
                return;
            }
            
            Debug.Log("Offer bought: " + _itemCode);
            // market satin alma ekrani acilacak, basarili olursa satin alindi popupini gosterecegiz
        }

        public void OnWatchAds()
        {
            Debug.Log("Watch ads for: " + _itemCode);
            ADS.Instance.rewarded.LoadRewardedAd(null, () =>
            {
                InventoryManager.Instance.AddGold(_goldAmount);
                InventoryManager.Instance.ShowItemClaimPopup(icon.sprite, _goldAmount);
                Debug.Log("Reward earned: " + _itemCode);
            });
        }
    }

}