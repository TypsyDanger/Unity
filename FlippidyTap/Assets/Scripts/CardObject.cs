using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour {

	public Animator cardAnimator;
	public GridManager gridRef;
	public GameObject cardObject;
	public bool isFlipped;
	public bool isMatched;
	public int cardNum; // assigned when updateFaceAndInit is called by GridPrefab.  This card and it's pair will have the same value.  Used in checking matches.
	public int cardId;
	private bool _lockCard;
	private CardObject _scriptRef;
	private GameObject _gameObjRef;
    private GameManager _gameManagerRef;
    private GameObject _flipParticlePrefab;
	private GameObject _matchPopPrefab;
    private GameObject _matchParticle;
    private BoxCollider2D _colliderRef;
	private bool _playAudio;

	private bool _isHintRoutineRunning;
	private int _hintClockDuration;
	private int _hintClockTick;
	private bool _hintRoutineInProgress;

	private bool _showDebugHintInUpdate;

	private bool _isPaused;

	void Awake () {
		// does not create the face of the card.  Handled by "initCard" method which is called by GridPrefab.cs
        _lockCard = true;
		isMatched = false;
	}

	// Update is called once per frame
	void Update () {
		if(!_isPaused && _isHintRoutineRunning && !_hintRoutineInProgress) {
			//print("starting hint routine");
			_showDebugHintInUpdate = true;
			startHintRoutine();
		} else if(_showDebugHintInUpdate)  {
			//print("isPaused: " + _isPaused + ", isHintRoutineRunning: " + _isHintRoutineRunning + ", hintRoutineInProgress: " + _hintRoutineInProgress);
			_showDebugHintInUpdate = false;
		}
	}

	void initCard() {
		// called by updateFaceAndInit()
		// print("init card...");

		cardAnimator = this.GetComponent<Animator> ();
        _colliderRef = this.GetComponent<BoxCollider2D>();

		isFlipped = false;

        _gameManagerRef.playSound("play_cardAppear");

		_flipParticlePrefab = Resources.Load("Prefabs/FlipParticle") as GameObject;
		_matchParticle = Resources.Load("Prefabs/MatchCoinParticle") as GameObject;
		_matchPopPrefab = Resources.Load("Prefabs/matchPopPrefab") as GameObject;

		_playAudio = true;

		_isPaused = false;

		_hintClockDuration = 5;
		_hintClockTick = 0;
		_hintRoutineInProgress = false;

		_showDebugHintInUpdate = false;

	}

	public void updateFaceAndInit(GridManager gridRefArg, GameManager gameManagerRefArg, Sprite faceSpriteArg, int cardNumArg, int cardIdArg, CardObject cardScriptArg, GameObject objRefArg) {
		// print ("updateFace: " + faceSprite);
		gridRef = gridRefArg;
		_scriptRef = cardScriptArg;
        _gameManagerRef = gameManagerRefArg;
		cardObject.GetComponent<SpriteRenderer> ().sprite = faceSpriteArg; 
		_gameObjRef = objRefArg;
		cardNum = cardNumArg;
		cardId = cardIdArg;
		//print("cardID = " + cardId);
		initCard ();
	}

	public void lockCard() {
        _colliderRef.enabled = false;
		_lockCard = true;
	}

	public void triggerDestroyCard() {
		cardAnimator.Play ("blazeh-outro", -1, 0f);
		stopHintRoutine();
        _gameManagerRef.playSound("play_cardDestroy");
		StartCoroutine (destroyCard (0.2f));
		
	}

	private IEnumerator destroyCard(float delayTimeArg) {
		yield return new WaitForSeconds (delayTimeArg);
		stopHintRoutine();
		Destroy(_gameObjRef);
	}

	void OnMouseOver() {
		if (Input.GetMouseButtonDown (0)) {
			//print("card mouse down");
			if (!_lockCard) {
				//print("card not locked");
				if (!isFlipped && gridRef.approveFlip()) {
					stopHintRoutine();
					//print("tapped and flipping!");
					cardAnimator.Play ("blazeh-flip", -1, 0f);
                    var particleSystem = GameObject.Instantiate(_flipParticlePrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity) as GameObject;
                    Destroy(particleSystem, 0.25f);
                    _gameManagerRef.playSound("play_flipped_two");
					_gameManagerRef.pauseHintCounter(true);

                    isFlipped = true;
					gridRef.setCards (cardNum, _scriptRef);
				}  else {
					print("something else went wrong.. isFlipped: " + isFlipped + " approved: " + gridRef.approveFlip());
				}
			}
		}
	}


    public void playMatched() {
		var coinCount = (_gameManagerRef.returnCurrentCoinValue() + (_gameManagerRef.getConsecutiveMatches() * _gameManagerRef.returnCurrentCoinValue()));
		//var coinCount = 64;

		for (var i = 0; i < coinCount; i++) {
			if (i % 2 == 0) {
				var matchParticle = GameObject.Instantiate(_matchParticle, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity) as GameObject;
				Destroy(matchParticle, 0.75f);
			}

			if (_playAudio && i < 17) {
				// i < 17 prevents horrendous number of coin sounds from playing
				StartCoroutine(playCoinMatchSound(0.015f * i, i));
			}
		}

		if(_gameManagerRef.getConsecutiveMatches() > 0) {
			var coinDelay = 0.0f;
			if (_gameManagerRef.returnIsEnduroMode()) {
				StartCoroutine(createMatchPop("clock", "plus", "float", 5, 0.0f));
				coinDelay = 0.3f;
			}
			StartCoroutine(createMatchPop("coin", "multiply", "float", _gameManagerRef.getConsecutiveMatches() + 1, coinDelay));
		}

		_gameManagerRef.playSound("play_match");
				
        cardAnimator.Play("blazeh-match", -1, 0f);
	}

	private IEnumerator playCoinMatchSound(float delay, int iteration) {
		yield return new WaitForSeconds(delay);
		switch(iteration % 2) {
			case 0:
				_gameManagerRef.playSound("play_coinGet");
				break;
			case 1:
				_gameManagerRef.playSound("play_coinGet_2");
				break;
		}

	}

	private IEnumerator createMatchPop(string iconArg, string signArg, string animArg, int numArg, float delay) {
		yield return new WaitForSeconds(delay);

		var tempPop = GameObject.Instantiate(_matchPopPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 10f), Quaternion.identity) as GameObject;
		var tempAnimator = tempPop.transform.GetChild(0).GetComponent<Animator>();
		//print("temp anim: " + tempAnimator);
		tempPop.transform.parent = GameObject.Find("ScoreCanvas").transform;
		tempAnimator.Play(animArg, -1, 0f);
		tempPop.GetComponent<MatchPopPrefab>().initNumberSequence(iconArg, signArg, numArg);
		Destroy(tempPop, 1f);
	}

	public void unFlipCard() {
		//print("unFlipCard");
		cardAnimator.Play ("blazeh-unflip-2", -1, 0f);

		isFlipped = false;
	}

    public void unlockCard() {
        _colliderRef.enabled = true;
        _lockCard = false;
    }

    public void pauseCard() {
        cardAnimator.enabled = false;
		_isPaused = true;
    }

    public void unPauseCard() {
        cardAnimator.enabled = true;
		_isPaused = false;
    }

	public void restrictAudio() {
		_playAudio = false;
	}

	public void playTapHint() {
		cardAnimator.Play("WINK_2_card", -1, 0f);
		startHintRoutine();
	}

	public void playHiddenHint() {
		if (!isFlipped && !cardAnimator.GetCurrentAnimatorStateInfo(0).IsName("blazeh-unflip-2")) {
			cardAnimator.Play("HIDDENHINT_card", -1, 0f);
		}
	}

	private void startHintRoutine() {
		_isHintRoutineRunning = true;
		if (!_isHintRoutineRunning) {
			//print("starting hint routine");
		}
		StartCoroutine(initHintRoutine(1f));
	}

	public IEnumerator initHintRoutine(float delay) {
		//print("hint routine tick: " + _hintClockTick);
		_hintRoutineInProgress = true;
		yield return new WaitForSeconds(delay);

		if(_isPaused) {
			//print("isPaused... blank tick");
		} else if (_hintClockTick < _hintClockDuration && _isHintRoutineRunning) {
			//print("tick...");
			_hintClockTick++;
		} else if(_hintClockTick >= _hintClockDuration && _isHintRoutineRunning) {
			//print("routine finished, going impatient");
			cardAnimator.Play("Impatient", -1, 0f);
			resetHintRoutineData();
		} else {
			//print("routine killed during ether between clock ticks, or something else happened.");
		}
		_hintRoutineInProgress = false;
	}

	private void stopHintRoutine() {
		if(_isHintRoutineRunning) {
			resetHintRoutineData();
		}
	}

	private void resetHintRoutineData() {
		_hintClockTick = 0;
		_isHintRoutineRunning = false;
	}

	public int returnCardNum() {
		return cardNum;
	}

	public bool checkUnflippedCounterpart(int cardNumArg) {
		//print("checking counter part on card: " + cardNum + " for cardNumArg: " + cardNumArg);
		if(cardNum == cardNumArg && !isFlipped) {
			return true;
		} else {
			return false;
		}
	}

	public int returnIDifCounterPartAndUnmatched(int cardNumArg) {
		//print(string.Format("returnIDifyaddayadda:: cardNum: {0}, cardNumArg{1}", cardNum, cardNumArg));
		if(cardNum == cardNumArg && !isMatched) {
			return cardId;
		} else {
			return -99;
		}
	}

	public int returnID() {
		return cardId;
	}

	public bool returnMatched() {
		return isMatched;
	}

	public void setMatched() {
		isMatched = true;
		lockCard();
	}

	public bool returnFlipped() {
		return isFlipped;
	}

	public void playTapHintWinkedSound() {
		_gameManagerRef.playSound("play_wink");
	}
}