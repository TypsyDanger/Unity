using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOptionsBackButtonManager : MonoExtended {
	
	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showInfoFromOptions();
		}
	}
}
