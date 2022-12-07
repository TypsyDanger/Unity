using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageScreenManager : MonoExtended {
	public void goToMain() {
		_gameManagerRef.loadScene("main");
	}
}
