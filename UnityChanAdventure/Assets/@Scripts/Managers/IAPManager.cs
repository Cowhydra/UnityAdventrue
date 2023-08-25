using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : IStoreListener
{
    #region ��ǰ

    public const string _Android_limit49000 = "limit49000";
    public const string _Android_nonlimit49000 = "nonlimit49000";
    public const string _Android_nonlimit11900 = "nonlimit11900";
    public const string _Android_limit11900 = "limit11900";

    public const string _Ios_limit49000 = "limit49000";
    public const string _Ios_nonlimit49000 = "nonlimit49000";
    public const string _Ios_nonlimit11900 = "nonlimit11900";
    public const string _Ios_limit11900 = "limit11900";
    #endregion
    //IStoreController�� ���� ������ �����ϴ� �Լ��� �����ϸ�
    //IExtensionProvider�� ���� �÷����� ���� Ȯ�� ó���� �����Ѵ�.

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;
    Product CurrBuyProduct = null;
    public void Init()
    {
        if (IsInitialized()) return;
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        InitUnityIAP(builder);
        UnityPurchasing.Initialize(this, builder);
    }
    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }
  


    private void InitUnityIAP(ConfigurationBuilder builder)
    {

        builder.AddProduct(_Android_limit49000, ProductType.NonConsumable, new IDs(){
            {_Android_limit49000, GooglePlay.Name },
                        {_Ios_limit49000, AppleAppStore.Name}
        }
        );

        builder.AddProduct(_Android_limit11900, ProductType.NonConsumable, new IDs(){
            {_Android_limit11900, GooglePlay.Name },
                        {_Ios_limit11900, AppleAppStore.Name}
       }
       );

        builder.AddProduct(_Android_nonlimit49000, ProductType.Consumable, new IDs(){
            {_Android_nonlimit49000, GooglePlay.Name },
                        {_Ios_nonlimit49000, AppleAppStore.Name}
       }
       );

        builder.AddProduct(_Android_nonlimit11900, ProductType.Consumable, new IDs(){
            {_Android_nonlimit11900, GooglePlay.Name },
                        {_Ios_nonlimit11900, AppleAppStore.Name}
        });
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        storeController = controller;
        storeExtensionProvider = extension;
    }

    public void OnConfirmIAP()
    {
        if (false == IsInitialized())
            return;

        if (null != CurrBuyProduct)
        {
            storeController.ConfirmPendingPurchase(CurrBuyProduct);


        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("�ʱ�ȭ ����");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning($"���� ���� - {product.definition.id}, {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        bool validPurchase = true;
        CurrBuyProduct = null;

#if UNITY_ANDROID || UNITY_IOS
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),null, Application.identifier);
        try
        {
            //������ �˻�
            /*���� ������ ���� ������ ��ȿ���� �˻��մϴ�.
            �������� ���ø����̼� ���� �ĺ��ڸ� ���ø����̼��� �ĺ��ڿ� ���մϴ�. 
            �� ���� ��ġ���� ������ InvalidBundleId ���� ������ �߻��մϴ�.*/
            var result = validator.Validate(e.purchasedProduct.receipt);
            Debug.Log("������ ���� ��� ��");

            //������ ���� ���
            foreach (IPurchaseReceipt purchaseReceipt in result)
            {
                Debug.Log("������ ����");
                Debug.Log(purchaseReceipt.productID);
                Debug.Log(purchaseReceipt.purchaseDate);
                Debug.Log(purchaseReceipt.transactionID);
            }
        }
        catch (IAPSecurityException)
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("Invalid receipt", Color.red);
            Debug.Log($"���� �� �� ");
            validPurchase = false;
        }
#endif
        if (validPurchase) //����Ȯ��
        {
            int Dia = 0;
            if (e.purchasedProduct.definition.id.Equals(_Android_limit49000))
            {
                Dia = 49000 * 2;
            }
            else if (e.purchasedProduct.definition.id.Equals(_Android_nonlimit11900))
            {
                Dia = 119000*2;
            }
            else if (e.purchasedProduct.definition.id.Equals(_Android_limit11900))
                Dia = 49000 + 119000;
            else if (e.purchasedProduct.definition.id.Equals(_Android_nonlimit49000))
                Dia = 49000 + 24000;

            CurrBuyProduct = e.purchasedProduct;
           Debug.Log($"{Dia} ������̾� ȹ�� �Ϸ�");
            Managers.Game.RedDiamondChange(Dia);
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText($"���� ���� {Dia} RedDia", Color.black);
            
        }
        else
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("���� ���� ������ ����", Color.red);
            Debug.Log("���� ���� ������ ����");
        }
      
        return PurchaseProcessingResult.Complete;


    }

  


   public void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = storeController.products.WithID(productId);
            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

                //Debug.Log( " IAP MANAGER 111111111111111111111111111111111111111111 " + productId );

                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                storeController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }
  
}
