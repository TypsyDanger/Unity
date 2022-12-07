using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
	private GameManager _gameManagerRef;

	private IEnumerator _restoreCoroutine;

	public static string prodIDAndroidFullGame = "ftap_iap_fullgame";
	public static string prodIDiOSFullGame = "ftap_iap_fullgame_ios";

	private string prodIDFullGame;

	private static int _STATUS_ERROR_STORE_UNAVAIL = 0;
	private static int _STATUS_ERROR_RESTORE_NOT_NEEDED = 1;
	private static int _STATUS_ERROR_PROD_ID_FAILED = 2;
	private static int _STATUS_ERROR_IOS_ONLY = 3;
	private static int _STATUS_ERROR_PROD_LOOKUP_FAILED_OR_UNKNOWN = 4;
	private static int _STATUS_ERROR_PROD_PURCHASE_ERROR = 5;
	private static int _STATUS_ERROR_STORE_INIT_FAILED = 6;
	private static int _STATUS_ERROR_PROD_UNKNOWN_PURCHASED = 7;
	private static int _STATUS_STORE_COMMUNICATING = 8;  // likely never used here, but important to indicate so 8 is never overwritten.

	void Start() {
		//print("IAP Manager is here!");
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();

		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			prodIDFullGame = prodIDiOSFullGame;
		} else {
			prodIDFullGame = prodIDAndroidFullGame;
		}

		if (m_StoreController == null) {
			InitializePurchasing();
		}
	}

	private IEnumerator restoreTimeout(float delay) {
		yield return new WaitForSeconds(delay);
		cancelRestore();
	}

	private void cancelRestore() {
		StopCoroutine(_restoreCoroutine);
		_gameManagerRef.purchaseError(_STATUS_ERROR_RESTORE_NOT_NEEDED, "", "");
	}

	public void InitializePurchasing() {
		// If we have already connected to Purchasing ...
		if (IsInitialized()) {
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.

		builder.AddProduct(prodIDFullGame, ProductType.NonConsumable);

		//builder.AddProduct(prodIDErrorID, ProductType.NonConsumable);


		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}

	private bool IsInitialized() {
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public IEnumerator BuyFullGame(float delay) {
		// Buy the non-consumable product using its general identifier. Expect a response either 
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		yield return new WaitForSeconds(delay);
		//print("IAP Manager is trying to buy full game");
		BuyProductID(prodIDFullGame);
	}

	public IEnumerator BuyError(int statusArg, float delay) {
		// Buy the non-consumable product using its general identifier. Expect a response either 
		// through ProcessPurchase or OnPurchaseFailed asynchronously.
		yield return new WaitForSeconds(delay);
		_gameManagerRef.purchaseError(statusArg, "argOneThing", "argTwoThingy");
		//print("IAP Manager is trying to buy with error ID");
		//BuyProductID(prodIDErrorID);
	}

	void BuyProductID(string productId) {
		// If Purchasing has been initialized ...
		//print("IAP Manager is trying to buy productID: " + productId);
		if (IsInitialized()) {
			//print("it's initialized, so buy stuff");
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase) {
				//print("not only is product available, but it's not null!");
				//print(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else {
				// ... report the product look-up failure situation  
				//print("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				_gameManagerRef.purchaseError(_STATUS_ERROR_PROD_LOOKUP_FAILED_OR_UNKNOWN, productId, "");

			}
		}
		// Otherwise ...
		else {
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			//print("BuyProductID FAIL. Not initialized.");
			initializeError();
		}
	}

	public IEnumerator attemptRestore(float delay) {
		yield return new WaitForSeconds(delay);
		RestorePurchases();
	}

	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases() {
		
		// If Purchasing has not yet been set up ...
		//print("RestorePurchases()");
		if (!IsInitialized()) {
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			//print("RestorePurchases FAIL. Not initialized.");
			initializeError();
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer ||
			Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			// ... begin restoring purchases
			//print("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
				// The first phase of restoration. If no more responses are received on ProcessPurchase then 
				// no purchases are available to be restored.
				_restoreCoroutine = restoreTimeout(10f);
				StartCoroutine(_restoreCoroutine);
			});
		}
		// Otherwise ...
		else {
			// We are not running on an Apple device. No work is necessary to restore purchases.
			//print("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);

			_gameManagerRef.purchaseError(_STATUS_ERROR_IOS_ONLY, "" + Application.platform, "");

		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		//print("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;

		//print("setting full game price from IAP Manager");
		_gameManagerRef.setFulLGamePrice(m_StoreController.products.WithID(prodIDFullGame).metadata.localizedPriceString);
	}


	public void OnInitializeFailed(InitializationFailureReason error) {
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		//print("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
		//print("purchaseProcessResult fired");
	
		// Or ... a non-consumable product has been purchased by this user.
		if (String.Equals(args.purchasedProduct.definition.id, prodIDFullGame, StringComparison.Ordinal)) {
			//print(string.Format("FullGameProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			_gameManagerRef.setFullGamePurchased();
			// TODO: The non-consumable item has been successfully purchased, grant this item to the player.
		}

		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else {
			//print(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
			_gameManagerRef.purchaseError(_STATUS_ERROR_PROD_UNKNOWN_PURCHASED, "" + args.purchasedProduct.definition.id, "");
		}
	
		// Return a flag indicating whether this product has completely been received, or if the application needs 
		// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
		// saving purchased products to the cloud, and when that save is delayed. 
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		//print(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

		_gameManagerRef.purchaseError(_STATUS_ERROR_PROD_PURCHASE_ERROR, "" + product.definition.storeSpecificId, "" + failureReason);

	}

	private void initializeError() {
		//print("iap manager reported a fail on initialization");
		_gameManagerRef.purchaseError(_STATUS_ERROR_STORE_INIT_FAILED, "", ""); 

	}
	/*
	public IEnumerator testError(float delay) {
		//print("running testError from IAPManager");
		yield return new WaitForSeconds(delay);
		_gameManagerRef.purchaseError("This is a test error");
	}*/

}
