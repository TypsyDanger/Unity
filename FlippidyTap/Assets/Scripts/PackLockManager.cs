using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackLockManager : MonoBehaviour {
	public bool isStart;
	private GameManager _gameManagerRef;
	void Start() {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	private void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)) {
			if(isStart) {
				_gameManagerRef.showPurchaseOverlay();
			} else if(!isStart){
				_gameManagerRef.buyCardPack();	
			}
		}
	}
}
