using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
	private GameObject _cardPrefab;
	private int _numPairs;
	private int _numMatches;
	private int[] _cardSequence;
	private Sprite[] _cardCandidates;
	private CardObject[] _allCardScriptsRefs;
	private int[] _flippedCards;
	private CardObject[] _flippedCardsScripts;
	private bool _okToGo;
	private int _numRows;
	private int _numCols;
	private bool _scoreSet;
	private int _theScore;
	private GameManager _gameManagerRef;
	private float _scaleAdjust;
	private Animator _gamePlayBGAnimator;
	private Animator _winPopAnimator;
	private Text _winPopLevelText;
	private Sprite[] _allCards;

	private CardObject[] _currentHintPair;

	private GameObject _cardContainerRef;

	private float _unPauseBackGroundSpeed;  // used for unpausing at the right speed.
	
	void Start() {
		_flippedCards = new int[2];
		_flippedCards[0] = -1;
		_flippedCards[1] = -1;
		_okToGo = true;
		_flippedCardsScripts = new CardObject[2];
		_currentHintPair = new CardObject[2];
		_cardPrefab = Resources.Load<GameObject>("Prefabs/gameCard");
		_scoreSet = false;

		_cardContainerRef = GameObject.Find("cardContainer");

		_scaleAdjust = 1f;

		_winPopAnimator = GameObject.Find("winPop").GetComponent<Animator>();

		_gamePlayBGAnimator = GameObject.Find("gamePlayBackground").GetComponent<Animator>();

		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();

		_numRows = 1;
		_numCols = 2;

		initGrid();
	}

	public void initGrid() {
		switch (_gameManagerRef.returnActivePack()) {

			case 0:
				_allCards = Resources.LoadAll<Sprite>("TileFaces/pack-1-random");
				break;
			case 1:
				_allCards = Resources.LoadAll<Sprite>("TileFaces/pack-4-food");
				break;
			case 2:
				_allCards = Resources.LoadAll<Sprite>("TileFaces/pack-2-flags-1");
				break;
		}

		if (_gameManagerRef.returnCurrentLevel() > 1 && _gameManagerRef.returnIsEnduroMode()) {
			_gameManagerRef.updateLevelUI();
		}

		setLevel();

		_numMatches = 0;
		_numPairs = (_numRows * _numCols) / 2;

		_cardCandidates = new Sprite[_numPairs];
		_allCardScriptsRefs = new CardObject[_numPairs * 2];

		// prevents game from crashing if number of pairs is lower than total number of cards in library.
		if (_numPairs > _allCards.Length) {
			_numRows--;
		}

		var i = 0;
		while (i < _numPairs) {
			var ran = Random.Range(0, _allCards.Length); 

			if (_allCards[ran] != null) {
				// check if card was already used in source array
				_cardCandidates[i] = _allCards[ran];
				_allCards[ran] = null;
				i++;    // incrementing here
			} else {
				//print("found a null value in _allCards[" + ran + "]");
			}
		}

		_cardSequence = populateSequenceOfPairs(_numPairs * 2);     // K, so this will end up being the array that controls the sequence of indexes that will be used in creating the actual grid.  This DOES ensure that pairs are used and the proper number.
		_cardSequence = shuffleArray(_cardSequence);    // The above line created an array with sequential pairs (1,1,2,2, etc.) and shuffles it.
		_cardSequence = shuffleArray(_cardSequence); // Bah, shuffle it again, just for fun

		Invoke("invokedCreateGrid", .05f);

	}

	private void setLevel() {

		int gameMode = _gameManagerRef.returnGameMode();
		if (_gameManagerRef.returnIsEnduroMode()) {
			switch (_gameManagerRef.returnCurrentLevel()) {
				case 1:
					_numRows = 1;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 2:
					_numRows = 2;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 3:
					_numRows = 3;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 4:
					_numRows = 2;
					_numCols = 3;
					_scaleAdjust = .95f;
					break;
				case 5:
					_numRows = 2;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 6:
					_numRows = 4;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 7:
					_numRows = 3;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 8:
					_numRows = 4;
					_numCols = 3;
					_scaleAdjust = 0.75f;
					break;
				case 9:
					_numRows = 4;
					_numCols = 3;
					_scaleAdjust = 1f;
					break;
				case 10:
					_numRows = 6;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 11:
					_numRows = 4;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 12:
					_numRows = 3;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 13:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 14:
					_numRows = 3;
					_numCols = 6;
					_scaleAdjust = 0.485f;
					break;
				case 15:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 16:
					_numRows = 4;
					_numCols = 5;
					_scaleAdjust = 0.55f;
					break;
				case 17:
					_numRows = 5;
					_numCols = 4;
					_scaleAdjust = 0.485f;
					break;
				case 18:
					_numRows = 4;
					_numCols = 5;
					_scaleAdjust = 0.58f;
					break;
				case 19:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 20:
					_numRows = 6;
					_numCols = 6;
					_scaleAdjust = 0.485f;
					break;
				case 21:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 22:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 23:
					_numRows = 4;
					_numCols = 5;
					_scaleAdjust = 0.58f;
					break;
				case 24:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 25:
					_numRows = 3;
					_numCols = 6;
					_scaleAdjust = 0.485f;
					break;
				case 26:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 27:
					_numRows = 3;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 28:
					_numRows = 4;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 29:
					_numRows = 2;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 30:
					_numRows = 6;
					_numCols = 6;
					_scaleAdjust = 0.485f;
					break;
				case 31:
					_numRows = 4;
					_numCols = 5;
					_scaleAdjust = 0.58f;
					break;
				case 32:
					_numRows = 6;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 33:
					_numRows = 6;
					_numCols = 4;
					_scaleAdjust = 0.72f;
					break;
				case 34:
					_numRows = 4;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 35:
					_numRows = 4;
					_numCols = 5;
					_scaleAdjust = .58f;
					break;
				case 36:
					_numRows = 6;
					_numCols = 3;
					_scaleAdjust = .72f;
					break;
				case 37:
					_numRows = 6;
					_numCols = 2;
					_scaleAdjust = .72f;
					break;
				case 38:
					_numRows = 6;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 39:
					_numRows = 6;
					_numCols = 3;
					_scaleAdjust = .72f;
					break;
				case 40:
					_numRows = 7;
					_numCols = 6;
					_scaleAdjust = .49f;
					break;
				case 41:
					_numRows = 2;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 42:
					_numRows = 4;
					_numCols = 2;
					_scaleAdjust = 1f;
					break;
				case 43:
					_numRows = 4;
					_numCols = 3;
					_scaleAdjust = 1f;
					break;
				case 44:
					_numRows = 5;
					_numCols = 4;
					_scaleAdjust = .72f;
					break;
				case 45:
					_numRows = 6;
					_numCols = 5;
					_scaleAdjust = .58f;
					break;
				case 46:
					_numRows = 6;
					_numCols = 6;
					_scaleAdjust = .485f;
					break;
				case 47:
					_numRows = 6;
					_numCols = 6;
					_scaleAdjust = .485f;
					break;
				case 48:
					_numRows = 7;
					_numCols = 6;
					_scaleAdjust = .485f;
					break;
				case 49:
					_numRows = 7;
					_numCols = 6;
					_scaleAdjust = .485f;
					break;
				default:
					_numRows = 8;
					_numCols = 6;
					_scaleAdjust = 0.485f;
					break;
			}
		} else {
			switch (_gameManagerRef.returnGameMode()) {
				case 1:
					_numRows = 4;
					_numCols = 3;
					_scaleAdjust = 1f;
					break;
				case 2:
					_numRows = 5;
					_numCols = 4;
					_scaleAdjust = 0.75f;
					break;
				case 3:
					_numRows = 8;
					_numCols = 6;
					_scaleAdjust = 0.485f;
					break;
			}
		}
	}

	private void invokedCreateGrid() {
		createGrid(_cardSequence);
	}

	private IEnumerator updateScore(float delay) {
		yield return new WaitForSeconds(delay);

		_gameManagerRef.updateScore();

		if (_gameManagerRef.returnIsEnduroMode()) {

			if (_gameManagerRef.getConsecutiveMatches() > 1) {
				_gameManagerRef.checkBonusAndAddTimeToClock(5);
				setBackgroundSpeed(2f);
			}
		}
	}

	int[] populateSequenceOfPairs(int length) {
		// This guy takes a length arg, creates an arr of that length, then populates that arr with pairs of sequential ints and returns it.
		int[] tempArr = new int[length];

		var i = 0;
		var j = 0;

		while (i < length) {
			if (i > 1 && i % 2 == 0)
				j += 1;

			tempArr[i] = j;

			i++;
		}

		return tempArr;
	}

	int[] shuffleArray(int[] sourceArray) {
		// this guy takes an array and creates a new shuffled array based on the argument array's contents.

		int[] tempArr = new int[sourceArray.Length];
		int[] sentinelArr = sourceArray;

		var i = 0;

		while (i < sourceArray.Length) {
			var ran = Random.Range(0, sourceArray.Length);
			if (sentinelArr[ran] != -1) {
				tempArr[i] = sentinelArr[ran];
				sentinelArr[ran] = -1;

				i++;
			}

		}

		return tempArr;
	}

	void createGrid(int[] cardSequenceArg) {
		// called by initGrid()

		float width = 105.6f * _scaleAdjust;
		float height = 105.6f * _scaleAdjust;
		float yStart = ((((_numRows * height) - height) / 2) + 1.56f);
		float xPos = ((((_numCols * width) - width) / 2) * -1);
		float yPos = yStart;

		int totalIterations = 0;

		for (var i = 0; i < _numCols; i++) {
			for (var j = 0; j < _numRows; j++) {
				StartCoroutine(createCard(totalIterations, cardSequenceArg[totalIterations], xPos, yPos));

				yPos -= height;
				totalIterations++;

			}
			xPos += width;
			yPos = yStart;
		}

		Resources.UnloadUnusedAssets();

		if (!_scoreSet) {
			_gameManagerRef.initScore();
			_scoreSet = true;
		}

		float delay = (float)totalIterations * 0.055f;

		if (totalIterations < 16) {
			delay += 0.02f * (16f - (float)totalIterations);
		}

		_gameManagerRef.startClockAfterDelay(delay);

		if ((_gameManagerRef.returnCurrentLevel() > 1 && _gameManagerRef.returnIsEnduroMode()) || !_gameManagerRef.returnIsEnduroMode()) {
			StartCoroutine(unlockAllCards(false, delay));
			StartCoroutine(enablePlayMode(delay));
		} else {
			StartCoroutine(unlockAllCards(false, 0.31f));
			StartCoroutine(enablePlayMode(0.31f));
		}
	}

	public IEnumerator createCard(int spriteNum, int cardFaceNum, float xPos, float yPos) {
		yield return new WaitForSeconds((float)spriteNum * 0.05f);

		GameObject tempObj = GameObject.Instantiate(_cardPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
		tempObj.transform.parent = _cardContainerRef.transform;
		tempObj.transform.localScale = new Vector3(_scaleAdjust, _scaleAdjust, 0);

		var scriptRef = tempObj.GetComponentInChildren<CardObject>();

		scriptRef.updateFaceAndInit(this, _gameManagerRef, _cardCandidates[cardFaceNum], cardFaceNum, spriteNum, scriptRef, tempObj);
		_allCardScriptsRefs[spriteNum] = scriptRef;
	}

	public void setCards(int cardNumArg, CardObject cardScriptArg) {
		if (_flippedCards[0] == -1) {
			_flippedCards[0] = cardNumArg;

			evaluateTapHint(cardNumArg);
			_flippedCardsScripts[0] = cardScriptArg;
		} else if (_flippedCards[1] == -1) {
			_flippedCards[1] = cardNumArg;
			_flippedCardsScripts[1] = cardScriptArg;
			_okToGo = false;
			checkMatch();   // put this here to allow mismatched cards to freeze for a moment before flipping back over, but it also prevents matched cards from playing their success anim soon enough.
		}
	}

	private void getNewCounterPartPair() {
		var rangeMax = (_numPairs * 2) -1;
		var found = false;
		var ran = 0;
		var counterPart = 0;

		while(!found) {
			ran = Random.Range(0, rangeMax);  // SOMETHING WRONG HERE, CAUSING GAME TO CRASH AT LEVEL 13
			if (_allCardScriptsRefs[ran].returnMatched()) {
				continue;
			}

			counterPart = getCounterPartID(ran);
			if(counterPart == -99) {
				continue;
			}

			var firstMatchedCheck = _allCardScriptsRefs[ran].returnMatched();
			var secondMatchedCheck = _allCardScriptsRefs[counterPart].returnMatched();

			if(!firstMatchedCheck && !secondMatchedCheck) {
				_currentHintPair[0] = _allCardScriptsRefs[ran];
				_currentHintPair[1] = _allCardScriptsRefs[counterPart];
				found = true;
			} else {
				//print("didn't find a suitable pair");
			}
		}

	}

	private void evaluateTapHint(int cardNumArg) {
		var ran = Random.Range(0, 6);
		//var ran = 10;
		if (ran == 5 && _gameManagerRef.returnCurrentLevel() > 5) {
			//print("play tap match: " + ran);
			var counterPart = getCounterPartByCardNum(cardNumArg);
			if (counterPart > -99) {
				_allCardScriptsRefs[counterPart].playTapHint();
				_gameManagerRef.trackEvent("TAP_HINT_PLAYED");
			}
		} else {
			//print("no tap match, because ran: " + ran);
		}
	}

	private int getCounterPartByCardNum(int cardNumArg) {
		int tempID = -99;
		for (var i = 0; i < _allCardScriptsRefs.Length; i++) {
			if(_allCardScriptsRefs[i].returnCardNum() == cardNumArg && !_allCardScriptsRefs[i].returnFlipped()) {
				tempID = i;
			}
		}

		return tempID;
	}

	private int getCounterPartID(int cardNumArg) {
		int tempID = -99;
		int originalCardNum = _allCardScriptsRefs[cardNumArg].returnCardNum();
		for (var i = 0; i < _allCardScriptsRefs.Length; i++) {
			//print("getCounterPartID iteration: " + i);
			if(_allCardScriptsRefs[i].returnIDifCounterPartAndUnmatched(originalCardNum) != -99) {
				if(_allCardScriptsRefs[i].returnID() != _allCardScriptsRefs[cardNumArg].returnID()){
					tempID = i;
					break;
				}
			}
		}

		return tempID;
	}

	private IEnumerator unlockAllCards(bool unPauseArg, float delay) {

		//print("unlocking cards after " + delay + " seconds");
		yield return new WaitForSeconds(delay);

		for (int i = 0; i < _allCardScriptsRefs.Length; i++) {
			_allCardScriptsRefs[i].unlockCard();
			if (unPauseArg) {
				_allCardScriptsRefs[i].unPauseCard();
			}
		}
	}

	private void lockAllCards(bool pauseArg) {
		//print("lock all cards");
		for (int i = 0; i < _allCardScriptsRefs.Length; i++) {
			_allCardScriptsRefs[i].lockCard();
			if (pauseArg) {
				//print("pausing card too");
				_allCardScriptsRefs[i].pauseCard();
			}
		}
	}

	public void pause() {
		//print("gridManager:pause");
		lockAllCards(true);
		_unPauseBackGroundSpeed = returnBackgroundSpeed();
		setBackgroundSpeed(0f);
	}

	public void unPause() {
		//print("gridManager:unPause");
		StartCoroutine(unlockAllCards(true, 0f));
		setBackgroundSpeed(_unPauseBackGroundSpeed);
	}

	public void unPauseAllCards() {
		StartCoroutine(unlockAllCards(true, 0f));
	}

	private void checkMatch() {
		float flipMatchDelay = 0.35f;

		if (_flippedCards[0] == _flippedCards[1]) {
			//print("found match!");

			_flippedCardsScripts[0].setMatched();
			_flippedCardsScripts[1].setMatched();

			_flippedCardsScripts[1].restrictAudio();

			StartCoroutine(playMatchedCards(_flippedCardsScripts[0], _flippedCardsScripts[1], flipMatchDelay));

			StartCoroutine(updateScore(flipMatchDelay));

			_numMatches += 1;

			_gameManagerRef.resetHintCounter();

			evaluateMatchCount();

		} else {
			//print("no match...");
			_gameManagerRef.resetConsecutiveMatches();
			StartCoroutine(unFlipCards(_flippedCardsScripts[0], _flippedCardsScripts[1], flipMatchDelay * 1.3f));
			_gameManagerRef.checkAndCancelBonus();
			penalizeBackgroundSpeed();
		}

		_gameManagerRef.pauseHintCounter(false);
	}

	private IEnumerator playMatchedCards(CardObject cardOneRef, CardObject cardTwoRef, float delay) {
		CardObject tempCardOne = cardOneRef;
		CardObject tempCardTwo = cardTwoRef;

		yield return new WaitForSeconds(delay);

		tempCardOne.playMatched();
		tempCardTwo.playMatched();
	}

	private IEnumerator unFlipCards(CardObject cardOneRef, CardObject cardTwoRef, float delay) {
		//print("unflipCards");
		CardObject cardOneTemp = cardOneRef;
		CardObject cardTwoTemp = cardTwoRef;

		resetCardRefs();

		yield return new WaitForSeconds(delay);

		_gameManagerRef.playSound("play_mismatch");

		cardOneTemp.unFlipCard();
		cardTwoTemp.unFlipCard();

	}

	private void resetCardRefs() {
		_flippedCards[0] = -1;
		_flippedCards[1] = -1;

		_okToGo = true;
	}

	public bool approveFlip() {
		if (_okToGo) {
			return true;
		} else {
			return false;
		}
	}

	private void destroyAllCardsAndResetGrid() {
		StartCoroutine(resetGrid(destroyAllCardsAndRespondWithFloat()));


	}

	public void setBackgroundSpeed(float speed) {
		if (_gamePlayBGAnimator.speed != speed) {
			_gamePlayBGAnimator.speed = speed;
		}
	}

	private void resetBackgroundSpeed() {
		setBackgroundSpeed(1);
	}

	private float returnBackgroundSpeed() {
		return _gamePlayBGAnimator.speed;
	}

	private void penalizeBackgroundSpeed() {
		setBackgroundSpeed(0.5f);
		Invoke("resetBackgroundSpeed", 1f);
	}

	public float destroyAllCardsAndRespondWithFloat() {
		float delay = 0f;
		for (var i = 0; i < _allCardScriptsRefs.Length; i++) {
			delay = (float)i * 0.05f;
			StartCoroutine(destroyCardObject(i, delay));
		}

		return delay;
	}


	private IEnumerator resetGrid(float delayTimeArg) {
		yield return new WaitForSeconds(delayTimeArg);

		_allCardScriptsRefs = new CardObject[_numPairs * 2];
		resetCardRefs();
		_okToGo = true;
		initGrid();
	}

	private IEnumerator destroyCardObject(int scriptRefIndexArg, float delayTimeArg) {

		yield return new WaitForSeconds(delayTimeArg);
		_allCardScriptsRefs[scriptRefIndexArg].triggerDestroyCard();

		if (scriptRefIndexArg < _cardCandidates.Length) {
			_cardCandidates[scriptRefIndexArg] = null;
		}
	}

	private void evaluateMatchCount() {
		if (_numMatches == _numPairs) {
			Invoke("destroyAllCardsAndResetGrid", 2.5f);
			Invoke("triggerLevelWin", .75f);
		} else {
			//print("not finished yet...");
			getNewCounterPartPair();
			_gameManagerRef.evaluateAndResetHelpMode();
			resetCardRefs();
		}
	}

	private void triggerLevelWin() {
		_gameManagerRef.pauseClock();
		_gameManagerRef.incrementCurrentLevel();

		_gameManagerRef.disablePlayMode();

		showWinPop();
	}

	private void showWinPop() {
		_gameManagerRef.playWinFanFare();
		_gameManagerRef.updateWinPopLevelText();

		_winPopAnimator.Play("winPopPlay");
	}

	public void requestMatch(CardObject cardReference) {

	}

	public void playHiddenCounterPartHint() {
		_currentHintPair[0].playHiddenHint();
		_currentHintPair[1].playHiddenHint();
		_gameManagerRef.trackEvent("COUNTERPART_HINT_PLAYED");
	}

	private IEnumerator enablePlayMode(float delay) {
		yield return new WaitForSeconds(delay);

		getNewCounterPartPair();
		_gameManagerRef.enablePlayMode();
	}
}
