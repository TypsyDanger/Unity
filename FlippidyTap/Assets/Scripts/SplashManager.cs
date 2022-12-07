using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoExtended {
    void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
			evaluateLanguage();
        }
    }

	public void playSwishOne() {
		_gameManagerRef.playSound("play_swish_1");
	}

	public void playSwishTwo() {
		_gameManagerRef.playSound("play_swish_2");
	}

	public void evaluateLanguage() {
		if (_gameManagerRef.isLanguageSet()) {
			_gameManagerRef.loadScene("Main");

		} else {
			_gameManagerRef.loadScene("LanguageSet");
		}
	}
}

