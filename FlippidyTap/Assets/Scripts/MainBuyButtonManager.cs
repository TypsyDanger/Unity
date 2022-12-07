using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuyButtonManager : MonoExtended {
	private void OnMouseDown() {
		print("mouse over");
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showPurchaseOverlay();
		}
	}
}
