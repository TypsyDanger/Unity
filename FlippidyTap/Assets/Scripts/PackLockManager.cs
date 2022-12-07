using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackLockManager : MonoBehaviour {

	public bool isStart;
	private GameManager _gameManagerRef;

	// Use this for initialization
	void Start() {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseOver() {
		//print("packlockcollider");
		if(Input.GetMouseButtonDown(0)) {
			//print("down");
			//print("packLockTap");
			if(isStart) {
				_gameManagerRef.showPurchaseOverlay();
			} else if(!isStart){
				_gameManagerRef.buyCardPack();	
			}
		}
	}
}
