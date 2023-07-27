using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace S_Durlanik.UI
{
    public class StoreScreen : UI_Screen
    {
        public List<GoldOffer> allGoldOffers;
    
        public StoreGoldItemUI exampleGoldItemUI;
        public GameObject removeAdsItem;

        public static StoreScreen Instance;
        private void Awake()
        {
            Instance = this;
        }
        public override void StartScreen(Action onStarted = null)
        {
            base.StartScreen(onStarted);

            StartCoroutine(SetStoreUI());
        }

        private IEnumerator SetStoreUI()
        {
            yield return null;
            
            removeAdsItem.SetActive(!Extensions.IsAdsRemoved());

            yield return null;
            
            SetGoldOffers();
        }

        private void SetGoldOffers()
        {
            exampleGoldItemUI.gameObject.SetActive(false);

            foreach (Transform child in exampleGoldItemUI.transform.parent)
            {
                if (child.gameObject.activeSelf)
                {
                    Destroy(child.gameObject);
                }
            }
            
            foreach (var offer in allGoldOffers)
            {
                var newGoldItemUI = Instantiate(exampleGoldItemUI, exampleGoldItemUI.transform.parent);
                newGoldItemUI.gameObject.SetActive(true);
                newGoldItemUI.SetStoreGoldItemUI(offer);
            }
        
        }

        public void OnRemoveAds()
        {
            ADS.Instance.RemoveAds();
        }
    }

    [Serializable]

    public class GoldOffer
    {
        public string itemCode;
        public int goldAmount;
        public string priceString;
        public bool canEarnByWatchingAds;
        public bool isBestOffer;
        public Sprite icon;
    }
}