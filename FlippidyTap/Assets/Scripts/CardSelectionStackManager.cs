using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionStackManager : MonoBehaviour {

	private GameManager _gameManagerRef;
	private Animator[] _cardPackAnimators;
	private Animator _packLockOverlayAnimator;
	private Animator _cardButtonStackAnimator;
	private int _activeCardPack;
	private bool _lockButton;
	private UnityEngine.UI.Text _cardPackLabel;
	private UnityEngine.UI.Text _cardPackText;
	private bool _lockIconActive;
	private int _packSelected;
	private Animator _storeCoinTextAnimator;
	private Animator _lockAnimator;
	private Animator _packSelectCheckAnimator;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update() {

	}

	private void Awake() {
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_cardButtonStackAnimator = GameObject.Find("packButtonStack").GetComponent<Animator>();
		_lockIconActive = false;
	}

	public void initStack(int packSelectedArg) {
		_activeCardPack = packSelectedArg; // 0 = regular, 1 = infinite s, 2 = infnite m, 3 = infinite l
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_lockButton = false;

		_cardPackAnimators = new Animator[3];
		_cardPackAnimators[0] = GameObject.Find("cardPackOneButton").GetComponent<Animator>();
		_cardPackAnimators[1] = GameObject.Find("cardPackTwoButton").GetComponent<Animator>();
		_cardPackAnimators[2] = GameObject.Find("cardPackThreeButton").GetComponent<Animator>();

		_packLockOverlayAnimator = GameObject.Find("packLockOverlay").GetComponent<Animator>();
		_packSelectCheckAnimator = GameObject.Find("packSelectCheck").GetComponent<Animator>();

		_lockAnimator = GameObject.Find("packLock").GetComponent<Animator>();

		_storeCoinTextAnimator = GameObject.Find("playerStoreCoins").GetComponent<Animator>();

		_cardPackLabel = GameObject.Find("cardPackLabel").GetComponent<UnityEngine.UI.Text>();
		_cardPackText = GameObject.Find("cardPackText").GetComponent<UnityEngine.UI.Text>();

		setActivePack(packSelectedArg);

	}

	private void setActivePack(int packSelectedArg) {
		for (var i = 0; i < _cardPackAnimators.Length; i++) {
			_cardPackAnimators[i].Play("outIdle");
		}

		_cardButtonStackAnimator.Play("idleInUse", -1, 0f);

		_cardPackAnimators[packSelectedArg].Play("inIdle");
		_packSelected = packSelectedArg;
		_activeCardPack = packSelectedArg;

		_gameManagerRef.setCardPackTexts(_packSelected);

		_packSelectCheckAnimator.Play("idleBounce", -1, 0f);
		_packLockOverlayAnimator.Play("outIdle", -1, 0f);
	}

    private void setStartButton(int buttonArg) {
		
	}

	public void switchButton(string direction) {
		if (!_lockButton) {
			_lockButton = true;
			_gameManagerRef.playSound("play_flipped");
			_cardPackAnimators[_activeCardPack].Play("flipOut", -1, 0f);
			if(_lockIconActive == true) {
				_packLockOverlayAnimator.Play("flipOut");
				_lockAnimator.Play("idleBounce");
			}

			if (direction == "next") {
				if (_activeCardPack == 2) {
					_activeCardPack = 0;
				} else {
					_activeCardPack++;
				}
			} else if (direction == "prev") {
				if (_activeCardPack == 0) {
					_activeCardPack = 2;
				} else {
					_activeCardPack--;
				}
			}

			_gameManagerRef.setCardPackTexts(_activeCardPack);

			//print("activeCardPack: " + _activeCardPack + " activeCardPack ownership: " + _gameManagerRef.returnPackOwned(_activeCardPack) + " and lockIconActive: " + _lockIconActive);

			_cardPackAnimators[_activeCardPack].Play("flipIn", -1, 0f);
			if (_gameManagerRef.returnPackOwned(_activeCardPack) == 0 && !_lockIconActive) {
				_packLockOverlayAnimator.Play("flipIn", -1, 0f);
				_packSelectCheckAnimator.Play("idleHide", -1, 0f);
				_cardButtonStackAnimator.Play("idleBuy", -1, 0f);
				_lockIconActive = true;
			} else if(_gameManagerRef.returnPackOwned(_activeCardPack) == 0 && _lockIconActive){
				_packSelectCheckAnimator.Play("idleHide", -1, 0f);
				_cardButtonStackAnimator.Play("idleBuy", -1, 0f);
				_packLockOverlayAnimator.Play("flipOnceFull", -1, 0f);
			} else {
				if(_activeCardPack != _packSelected) {
					_packSelectCheckAnimator.Play("idleHide", -1, 0f);
					_cardButtonStackAnimator.Play("idleUse", -1, 0f);
				} else {
					_packSelectCheckAnimator.Play("idleBounce", -1, 0f);
					_cardButtonStackAnimator.Play("idleInUse", -1, 0f);
				}
				_lockIconActive = false;
			}

			StartCoroutine(unlockButtons(0.25f));
		}
	}

	private IEnumerator unlockButtons(float delayArg) {
		yield return new WaitForSeconds(delayArg);
		_lockButton = false;
	}

	public int returnActivePack() {
		return _activeCardPack;
	}

	public void useActivePack() {
		_cardButtonStackAnimator.Play("idleInUse", -1, 0f);
	}

	public void playPurchaseFail() {
		_storeCoinTextAnimator.Play("buyFail", -1, 0f);
		_gameManagerRef.playSound("play_buyFail");
	}

	public void playPurchaseSuccess() {
		_lockAnimator.Play("unLock", -1, 0f);
		_gameManagerRef.playSound("play_purchase_1");
		_cardButtonStackAnimator.Play("idleInUse", -1, 0f);
		StartCoroutine(setActiveCardUnlocked(1f));
	}

	private IEnumerator setActiveCardUnlocked(float delay) {
		yield return new WaitForSeconds(delay);
		_packLockOverlayAnimator.Play("hide", -1, 0f);
		_gameManagerRef.selectCardPack();
	}

	public void selectCurrentPack() {
		_gameManagerRef.playSound("play_uiSelect");
		_packSelected = _activeCardPack;
		//print("packSelected: " + _packSelected);
		_cardButtonStackAnimator.Play("idleInUse", -1, 0f);
		_packSelectCheckAnimator.Play("show", -1, 0f);
	}
}
