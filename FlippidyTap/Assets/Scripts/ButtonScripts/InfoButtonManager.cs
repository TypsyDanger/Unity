using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButtonManager : MonoExtended {

	new void Awake() {
		base.Awake();
	}

	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showInfoFromMain();
		}
	}
}
