using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButtonManager : MonoBehaviour {

	private GameManager _gameManagerRef;

	// Use this for initialization
	void Start () {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.buyFullGame();
		}
	}
}
