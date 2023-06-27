using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.UI;
using Unity.Plastic.Newtonsoft.Json;
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
                purchasedItems.Add(item);
                Debug.Log($"Free item added to purchasedItems: {item.itemCode}");
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
        
        public void ShowItemClaimPopup(DailyLoginReward reward)
        {
            itemClaimPopup.gameObject.SetActive(true);
            itemClaimPopup.Show(reward);
        }

        // Envanter kayit islemi => purchasedItems listesini json stringine cevirip PlayerPrefs'a kaydeder
        public void SaveInventory() => PlayerPrefs.SetString("jsonInventory", JsonConvert.SerializeObject(purchasedItems));



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
    
    
}