using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using UnityEngine;
using UnityEngine.Purchasing;
public class IAPManager : MonoBehaviour, IStoreListener
{
    #region Singleton

    public static IAPManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    public List<ConsumableItem> consumableItems;
    public List<NonConsumableItem> nonConsumableItems;
    public List<SubscriptionItem> subscriptionItems;
    
    
     IStoreController _storeController;
    private void Start()
    {
        SetupBuilder();
    }
    
    void SetupBuilder()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        builder.AddProducts(consumableItems.ConvertAll<ProductDefinition>(x => new ProductDefinition(x.Id, ProductType.Consumable)));
        builder.AddProducts(nonConsumableItems.ConvertAll<ProductDefinition>(x => new ProductDefinition(x.Id, ProductType.NonConsumable)));
        builder.AddProducts(subscriptionItems.ConvertAll<ProductDefinition>(x => new ProductDefinition(x.Id, ProductType.Subscription)));
        UnityPurchasing.Initialize(this, builder);
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _storeController = controller;
        CheckRemoveAdsBought();
        Debug.Log("Store initialized");
    }
    Action _onPurchaseComplete;
    Action _onPurchaseFailed;
    public void Consumable_Btn_Pressed(string productID,Action onPurchaseComplete,Action onPurchaseFailed)
    {
        _onPurchaseComplete = onPurchaseComplete;
        _onPurchaseFailed = onPurchaseFailed;
        _storeController.InitiatePurchase(productID);
    }
    
    public void NonConsumable_Btn_Pressed(int nonConsumableProductIndex)
    {
        _storeController.InitiatePurchase(nonConsumableItems[nonConsumableProductIndex].Id);
    }

    public void Subscription_Btn_Pressed(int subscriptionProductIndex)
    {
        _storeController.InitiatePurchase(subscriptionItems[subscriptionProductIndex].Id);
    }
    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    { 
        _onPurchaseComplete?.Invoke();
        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {

    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Initialization Failed " + error + " " + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        _onPurchaseFailed?.Invoke();
        Debug.Log("Purchase Failed " + failureReason + " Product: " + product.definition.id);
    }
    
    void CheckRemoveAdsBought()
    {
        if(_storeController == null) return;
        Product product = _storeController.products.WithID(nonConsumableItems[0].Id);
        if (product != null && product.hasReceipt)
        {
            if(product.definition.id == nonConsumableItems[0].Id)
            {
                ADS.Instance.isRemoveAds = true;
                Debug.Log("Remove ADS purchased");
            }
        }
    }
    
}
[Serializable]
public class ConsumableItem
{
    public string Name;
    public string Id;
    public string Desc;
    public float Price;
    public int Amount;
}
[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string Desc;
    public float Price;
    public int Amount;
}
[Serializable]
public class SubscriptionItem
{
    public string Name;
    public string Id;
    public string Desc;
    public float Price;
    public int Amount;
    public int Duration; // in Days
}
