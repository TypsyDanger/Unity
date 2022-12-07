using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLanguageButtonManager : MonoExtended {
	void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showLanguageFromInfo();
		}
	}
}
