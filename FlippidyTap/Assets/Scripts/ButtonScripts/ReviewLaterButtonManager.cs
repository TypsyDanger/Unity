using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewLaterButtonManager : MonoExtended {
	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.hideReviewWindow();
		}
	}
}
