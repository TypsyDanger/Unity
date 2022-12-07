using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackSelectionButtonManager : MonoBehaviour {

	private GameManager _gameManagerRef;

	// Use this for initialization
	void Start () {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();	
	}

	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.selectCardPack();
		}
	}
}
