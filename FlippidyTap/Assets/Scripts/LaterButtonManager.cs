﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaterButtonManager : MonoBehaviour {

	private GameManager _gameManagerRef;

	void Start() {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void OnMouseDown() {
		if (Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showMainFromPurchase();
		}
	}
}
