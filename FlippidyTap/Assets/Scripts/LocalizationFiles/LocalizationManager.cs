using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LocalizationManager : MonoBehaviour {

	/* LocalizationManager 
	 * by tim.perry.wagner@gmail.com
	 * last updated: 05/02/2018
	 * 
	 * Provides interfaces for the game manager to do an initiale localization pass on the game after intialization a language.  
	 * Loads a jSON file to do localization, then parses it and adjusts in-game objects accordingly.  Allows on-the-fly re-localization 
	 * when user selects a new language in-game.
	 * 
	 */

	private LocalizedDataContainer container;
	private UnityEngine.UI.Image[] mainScreenButtons;
	private string _language;
	private GameManager _gameManagerRef;

	public void initAndLocalize(string language) {
		// Called by GameManager
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_language = language;
		container = LocalizedDataContainer.initAndReturn(Resources.Load<TextAsset>("LocalizationJSON/ftap_local_" + language).text);

		mainScreenButtons = new UnityEngine.UI.Image[8];
	}

	public void localizeMainScreen() {
		// Localizes MainScreen Scene, called by GameManager
		if (container != null) {
			processLocalizationArray(container.mainScreenAssets);

			// set random quote
			var ran = Random.Range(0, container.randomQuoteItems.Length);
			GameObject.Find(container.randomQuoteItems[ran].assetName).GetComponent<UnityEngine.UI.Text>().text = container.randomQuoteItems[ran].assetValue;
		}
	}

	public void localizeGamePlay() {
		// Localizes GamePlay, called by GameManager
		processLocalizationArray(container.gamePlayScreenAssets);
		if(_gameManagerRef.returnGameMode() != 0) {
			_gameManagerRef.updateLevelUI();
		}
	}

	private void processLocalizationArray(LocalizedDataItem[] dataArrArg) {
		// Re-localizes all content in current scene.
		UnityEngine.UI.Text tempText;
		UnityEngine.UI.Image tempImage;
		for (var i = 0; i < dataArrArg.Length; i++) {
			if (dataArrArg[i].assetType == "text") {
				// Item is a text asset, find and update its text value.
				tempText = GameObject.Find(dataArrArg[i].assetName).GetComponent<UnityEngine.UI.Text>();
				tempText.text = dataArrArg[i].assetValue;
			} else if (dataArrArg[i].assetType == "image") {
				// Item is an image asset, find its object, then load and update its sprite source
				tempImage = GameObject.Find(dataArrArg[i].assetName).GetComponent<UnityEngine.UI.Image>();
				tempImage.sprite = Resources.Load<Sprite>(dataArrArg[i].assetValue);
			}
		}
	}

	public LocalizedCardPackItem[] returnCardPackArray() {
		// Returns the card pack array.  Called by GameManager
		return container.cardPackDataObjects;
	}

	public LocalizedUILabels returnUILevelLabels() {
		// Returns all UI labels.  Called by GameManager.
		return container.gamePlayUILabels;
	}

	public string returnStatusCode(int codeArg) {
		// Returns all store status codes object.  Called by GameManager.
		return container.statusCodeItems[codeArg].statusString;
	}
}