using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseStatusBackButtonManager : MonoBehaviour {

	private GameManager _gameManagerRef;
	private Animator _thisAnimator;

	private void Awake() {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_thisAnimator = gameObject.GetComponent<Animator>();
		_thisAnimator.Play("idleHide", -1, 0f);
	}

	private void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.showPurchaseFromStatus();
		}
	}

	public void showButton() {
		_thisAnimator.Play("idleShow", -1, 0f);
	}

	public void hideButton() {
		_thisAnimator.Play("idleHide", -1, 0f);
	}
}
