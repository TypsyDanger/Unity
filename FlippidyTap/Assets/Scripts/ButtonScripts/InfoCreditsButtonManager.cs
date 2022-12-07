using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCreditsButtonManager : MonoExtended {

	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showCreditsFromInfo();
		}
	}
}
