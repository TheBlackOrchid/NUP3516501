using UnityEngine;
using System.Collections.Generic;
using OnePF;

public class IABController : MonoBehaviour
{
    const string SKU_NO_ADS = "no_ads";

    public StateMachine stateMachine;

    string _label = "";
    bool _isInitialized = false;
    Inventory _inventory = null;

    // Use this for initialization
    void Start()
    {
        // Map skus for different stores       
        OpenIAB.mapSku(SKU_NO_ADS, OpenIAB_Android.STORE_GOOGLE, "no_ads");
    }

    private void OnEnable()
    {
        // Listen to all events for illustration purposes
        OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }

    private void OnDisable()
    {
        // Remove all event handlers
        OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }

    public void Init()
    {
        // Application public key
        var googlePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAng8aclsJp0G+vRhgKUQ69ZQYv7w6KPGSWNcj4mIaiU3o3FFofBmF7vYgaGiAaV485TDZIAgdzb99otvIOHJIcSFV1Iu5hbLqNMhy23W4DI0ld6kH+iTJvk2w0zDeTNcGjsu/h/TRJb25HUSgBTIW+PsnvnlP25uuTC0/AnS+338BPQWjrqDivbWcQUsOFmxHCcpGr+7j7bBHdwzo1Dvuom6ZHT+i1UcYU69gjGssSkRNqO4NDLgnCv0kRGPJMBJE6YeHUJyaFFIc92eAbAuXNznMoB7Y71Z8J+HgEN1Fkjb4Vco3RiFgzkllz4KxdDCD+BvgnN4WT39d2JQt4vKGIwIDAQAB";

        var options = new Options();
        options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
        options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
        options.checkInventory = false;
        options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
        options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_GOOGLE, googlePublicKey } };
        options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;

        // Transmit options and start the service
        OpenIAB.init(options);
    }

    public void QueryInventory()
    {
        OpenIAB.queryInventory(new string[] { SKU_NO_ADS });
    }

    public bool CheckNoAds()
    {
        if (_inventory != null)
        {
            return _inventory.HasPurchase(SKU_NO_ADS);
        }
        else
        {
            return false;
        }
        //return _inventory.HasPurchase(SKU_NO_ADS);
    }

    public void PurchaseProduct()
    {
        OpenIAB.purchaseProduct(SKU_NO_ADS);
    }

    public void ConsumeProduct()
    {
        if (_inventory != null && _inventory.HasPurchase(SKU_NO_ADS))
            OpenIAB.consumeProduct(_inventory.GetPurchase(SKU_NO_ADS));
    }

    public void PurchaseTestProduct()
    {
        OpenIAB.purchaseProduct("android.test.purchased");
    }

    public void ConsumeTestProduct()
    {
        if (_inventory != null && _inventory.HasPurchase("android.test.purchased"))
            OpenIAB.consumeProduct(_inventory.GetPurchase("android.test.purchased"));
    }

    public void PurchaseUnavailableProduct()
    {
        OpenIAB.purchaseProduct("android.test.item_unavailable");
    }

    public void PurchaseCanceledProduct()
    {
        OpenIAB.purchaseProduct("android.test.canceled");
    }

    //---------------------------------------------------------

    private void billingSupportedEvent()
    {
        _isInitialized = true;
        Debug.Log("billingSupportedEvent");
    }

    private void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }

    private void queryInventorySucceededEvent(Inventory inventory)
    {
        Debug.Log("queryInventorySucceededEvent: " + inventory);
        if (inventory != null)
        {
            _label = inventory.ToString();
            _inventory = inventory;
        }
    }

    private void queryInventoryFailedEvent(string error)
    {
        Debug.Log("queryInventoryFailedEvent: " + error);
        _label = error;
    }

    private void purchaseSucceededEvent(Purchase purchase)
    {
        //stateMachine.DisableAds();
        Debug.Log("purchaseSucceededEvent: " + purchase);
        _label = "PURCHASED:" + purchase.ToString();
    }

    private void purchaseFailedEvent(int errorCode, string errorMessage)
    {
        Debug.Log("purchaseFailedEvent: " + errorMessage);
        _label = "Purchase Failed: " + errorMessage;
    }

    private void consumePurchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
        _label = "CONSUMED: " + purchase.ToString();
    }

    private void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
        _label = "Consume Failed: " + error;
    }
}
