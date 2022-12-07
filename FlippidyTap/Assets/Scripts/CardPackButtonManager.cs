using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPackButtonManager : MonoBehaviour {
	private GameManager _gameManagerRef;
	private string _buttonMode;
	private Animator _buttonAnimator;

	// Use this for initialization
	void Start () {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_buttonAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		if(Input.GetMouseButtonDown(0)) {
			switch(_buttonMode) {
				case "select":
					_gameManagerRef.selectCardPack();
					break;
				case "buy":
					_gameManagerRef.buyCardPack();
					break;
				case "inactive":
					break;
			}
		}
	}

	public void setButtonMode(string buttonModeArg) {
		if(_buttonAnimator == null) {
			_buttonAnimator = gameObject.GetComponent<Animator>();
		}

		_buttonMode = buttonModeArg;
		switch(_buttonMode) {
			case "select":
				_buttonAnimator.Play("useIdle", -1, 0f);
				break;
			case "buy":
				_buttonAnimator.Play("buyIdle", -1, 0f);
				break;
			case "inactive":
				_buttonAnimator.Play("inUseIdle", -1, 0f);
				break;
		}
	}

}
