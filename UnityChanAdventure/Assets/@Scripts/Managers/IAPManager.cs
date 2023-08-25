using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : IStoreListener
{
    #region 상품

    public const string _Android_limit49000 = "limit49000";
    public const string _Android_nonlimit49000 = "nonlimit49000";
    public const string _Android_nonlimit11900 = "nonlimit11900";
    public const string _Android_limit11900 = "limit11900";

    public const string _Ios_limit49000 = "limit49000";
    public const string _Ios_nonlimit49000 = "nonlimit49000";
    public const string _Ios_nonlimit11900 = "nonlimit11900";
    public const string _Ios_limit11900 = "limit11900";
    #endregion
    //IStoreController는 구매 과정을 제어하는 함수를 제공하며
    //IExtensionProvider는 여러 플랫폼을 위한 확장 처리를 제공한다.

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
        Debug.LogError($"유니티 IAP 초기화 실패 {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("초기화 에러");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        bool validPurchase = true;
        CurrBuyProduct = null;

#if UNITY_ANDROID || UNITY_IOS
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),null, Application.identifier);
        try
        {
            //영수증 검사
            /*서명 검증을 통해 영수증 유효성을 검사합니다.
            영수증의 애플리케이션 번들 식별자를 애플리케이션의 식별자와 비교합니다. 
            이 둘이 일치하지 않으면 InvalidBundleId 예외 오류가 발생합니다.*/
            var result = validator.Validate(e.purchasedProduct.receipt);
            Debug.Log("영수증 내용 출력 전");

            //영수증 내용 출력
            foreach (IPurchaseReceipt purchaseReceipt in result)
            {
                Debug.Log("영수증 내용");
                Debug.Log(purchaseReceipt.productID);
                Debug.Log(purchaseReceipt.purchaseDate);
                Debug.Log(purchaseReceipt.transactionID);
            }
        }
        catch (IAPSecurityException)
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("Invalid receipt", Color.red);
            Debug.Log($"오류 발 생 ");
            validPurchase = false;
        }
#endif
        if (validPurchase) //구매확정
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
           Debug.Log($"{Dia} 레드다이아 획득 완료");
            Managers.Game.RedDiamondChange(Dia);
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText($"구매 성공 {Dia} RedDia", Color.black);
            
        }
        else
        {
            Managers.UI.ShowPopupUI<WarningText>().Set_WarningText("구매 실패 비정상 결제", Color.red);
            Debug.Log("구매 실패 비정상 결제");
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
