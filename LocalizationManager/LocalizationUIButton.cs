using UnityEngine;
using System.Collections;

public class LocalizationUIButton : MonoExtended {

	/* LocalizationUIButton
	 * by tim.perry.wagner@gmail.com
	 * last updated: 05/19/2018
	 * 
	 * This is inherited by in-game language buttons as they all require some base functionality and access to common constants.
	 */

	protected const string _LOCAL_LANG_ENGLISH = "english";
	protected const string _LOCAL_LANG_FRENCH = "francais";
	protected const string _LOCAL_LANG_SA_SPANISH = "sa_spanish";
	protected const string _LOCAL_LANG_PIRATE = "pirate";
	protected const string _LOCAL_LANG_CANUCK = "canada";

	protected string _selectedLanguage;


	protected virtual void Awake() {
		// Runs as soon as the button is created.
		base.Awake();
		_selectedLanguage = _LOCAL_LANG_ENGLISH; // The default... subclasses should update this in their awake.
	}

	protected virtual void OnMouseDown() {
		// Fires when the button is tapped.
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.selectLanguage(_selectedLanguage);
		}
	}
}
