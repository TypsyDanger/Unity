using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packPurchaseButtonManager : MonoBehaviour {
	private GameManager _gameManagerRef;

	void Start() {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void OnMouseDown() {
		if (Input.GetMouseButtonDown(0)) {
			_gameManagerRef.buyCardPack();
		}
	}
}
