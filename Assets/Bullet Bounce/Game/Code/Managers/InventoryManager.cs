using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using S_Durlanik.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace S_Durlanik.Game
{
    public class InventoryManager : MonoBehaviour
    {
        public List<InventoryItem> purchasedItems;
        public static InventoryManager Instance { get; private set; }
        
        public ItemClaimUI itemClaimPopup;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LoadInventory();
        }
        
        public InventoryItem UserGold => purchasedItems.Find(x => x.itemCode == "currencyGold");

        public void PurchaseItem(InventoryItem item, bool isFree = false)
        {
            if (isFree)
            {
                if (purchasedItems.Any(x => x.itemCode == item.itemCode))
                {
                    purchasedItems.Find(x => x.itemCode == item.itemCode).stackAmount += item.stackAmount;
                }else
                {
                    purchasedItems.Add(item);
                }
                Debug.Log($"Free item added to purchasedItems: {item.itemCode}");
                SaveInventory();
                return;
            }

            var currency = item.itemType switch
            {
                InventoryItem.ItemType.Currency => UserGold,
                // ihtiyac halinde yeni currency eklenebilir
                _ => throw new ArgumentOutOfRangeException()
            };

            if (currency != null && currency.stackAmount >= item.cost)
            {
                currency.stackAmount -= item.cost;
                purchasedItems.Add(item);
                Debug.Log($"Item purchased: {item.itemCode}");
            }
            else
            {
                Debug.LogWarning($"Not enough Gold {item.cost}");
                // Not enough currency popup
            }

            SaveInventory();
        }
        
        public void AddGold(int amount)
        {
            UserGold.stackAmount += amount;
            SaveInventory();
        }

        public void RemoveGold(int amount)
        {
            UserGold.stackAmount -= amount;
            SaveInventory();
        }
        
        int GetCurrencyGoldAmount() => UserGold.stackAmount;
        
        public void CheckGoldAndRemove(int amount,Action successCallback = null,Action failCallback = null)
        {
            if (GetCurrencyGoldAmount() >= amount)
            {
                RemoveGold(amount);
                successCallback?.Invoke();
            }
            else
            {
                failCallback?.Invoke();
            }
        }

        public void UpdateUIGoldAmount()
        {
            foreach (TextMeshProUGUI goldAmountText in UI_System.Instance.goldAmountTexts)
            {
                goldAmountText.text = GetCurrencyGoldAmount().ToString();
            }
        }
        public void ShowItemClaimPopup(Sprite _itemImage, int _amount)
        {
            itemClaimPopup.gameObject.SetActive(true);
            itemClaimPopup.Show(_itemImage,_amount);
        }

        // Envanter kayit islemi => purchasedItems listesini json stringine cevirip PlayerPrefs'a kaydeder
        public void SaveInventory()
        {
            PlayerPrefs.SetString("jsonInventory", JsonConvert.SerializeObject(purchasedItems));
            UpdateUIGoldAmount();
        }


        // Envanter yukleme islemi => PlayerPrefs'ten json stringini alip purchasedItems listesine cevirir
        public void LoadInventory()
        {
            var jsonString = PlayerPrefs.GetString("jsonInventory");
            
            if (!string.IsNullOrEmpty(jsonString))
            {
                purchasedItems = JsonConvert.DeserializeObject<List<InventoryItem>>(jsonString);
                Debug.Log("Inventory Loaded");
            }
            else
            {
                Debug.LogWarning("Inventory Empty");
                purchasedItems = new List<InventoryItem>();
                
                // ilk acilista kullaniciya 100 altin ver
                
                var gold = new InventoryItem
                {
                    itemCode = "currencyGold",
                    name = "Gold",
                    stackAmount = 1,
                    cost = 0,
                    itemType = InventoryItem.ItemType.Currency
                };
                
                PurchaseItem(gold, true);
                SaveInventory();
            }
            
            UpdateUIGoldAmount();
        }
        
    }
    
    [Serializable]
    public class InventoryItem
    {
        public string itemCode;
        public string name;
        public int stackAmount;
        public int cost;
        public ItemType itemType;

        public enum ItemType
        {
            Currency
        }
    }

    [Serializable]
    public class SpinItem : InventoryItem
    {
        public Sprite itemSprite;
    }
    [Serializable]
    public class RewardItem : InventoryItem
    {
        public Sprite itemSprite;
    }
    
    
}