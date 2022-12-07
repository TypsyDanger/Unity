using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonStackManager : MonoBehaviour {

    private GameManager _gameManagerRef;
    private int _activeButton;
    private Animator[] _buttonAnimators;
    private bool _lockFromFlipping;
	private Animator _lockOverlayAnimator;
	private Animator _lockAnimator;
	private int _fullGameOwned;
	private bool _lockActive;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initStack(int gameMode) {
        _activeButton = 0; // 0 = regular, 1 = infinite s, 2 = infnite m, 3 = infinite l
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_fullGameOwned = _gameManagerRef.returnFullGameOwned();
		_lockOverlayAnimator = GameObject.Find("startLockOverlay").GetComponent<Animator>();
		_lockActive = false;

		_lockAnimator = GameObject.Find("startLock").GetComponent<Animator>();

		if(_fullGameOwned == 1 || gameMode == 0) {
			_lockOverlayAnimator.Play("outIdle", -1, 0f);
		}

        _lockFromFlipping = false;
		_buttonAnimators = new Animator[4];
		_buttonAnimators[0] = GameObject.Find("playEnduro").GetComponent<Animator>();
        _buttonAnimators[1] = GameObject.Find("playInfiniteEasy").GetComponent<Animator>();
        _buttonAnimators[2] = GameObject.Find("playInfiniteMedium").GetComponent<Animator>();
        _buttonAnimators[3] = GameObject.Find("playInfiniteHard").GetComponent<Animator>();

        setStartButton(gameMode);

    }

    private void setStartButton(int buttonArg) {
        for (var i = 0; i < _buttonAnimators.Length; i++) {
            _buttonAnimators[i].Play("outIdle");
        }

		if(buttonArg != 0 && _gameManagerRef.returnFullGameOwned() == 0) {
			_lockOverlayAnimator.Play("inIdle");
			_lockActive = true;
		}

        _buttonAnimators[buttonArg].Play("inIdle");
        _activeButton = buttonArg;
    }

    public void switchButton(string direction) {
		//print("flip!");
        if(!_lockFromFlipping) {
			_lockFromFlipping = true;
			_gameManagerRef.playSound("play_flipped");
            _buttonAnimators[_activeButton].Play("flipOut", -1, 0f);

            if(direction == "next") {
                if(_activeButton == 3) {
                    _activeButton = 0;
                } else {
                    _activeButton++;
                }
            } else if(direction == "prev") {
                if(_activeButton == 0) {
                    _activeButton = 3;
                } else {
                    _activeButton--;
                }
            }

            _buttonAnimators[_activeButton].Play("flipIn", -1, 0f);

			_gameManagerRef.setGameMode(_activeButton);

			if (_gameManagerRef.returnFullGameOwned() == 0) {
				if (_activeButton != 0) {
					//print("game mode not 0");
					if(!_lockActive) {
						//print("lock not active so set");
						_lockOverlayAnimator.Play("inIdle", -1, 0f);
						_lockActive = true;
					}
				} else {
					//print("game mode is 0");
					if(_lockActive) {
						//print("lockactive so hide");
						_lockOverlayAnimator.Play("outIdle", -1, 0f);
						_lockActive = false;
					}
				}
			}

            Invoke("unlockButtons", 0.25f);
        }
    }

    private void unlockButtons() {
        _lockFromFlipping = false;
    }

	public IEnumerator playPurchaseSuccess(float delay) {
		yield return new WaitForSeconds(delay);
		_gameManagerRef.playSound("play_purchase_1");
		_lockAnimator.Play("unLock", -1, 0f);
	}

	public void playStartUnlock() {
		_lockOverlayAnimator.Play("hide", -1, 0f);
	}
}
