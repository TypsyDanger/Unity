using UnityEngine;
using System.Collections;

public class HintCounterManager : MonoExtended {
	// V1.0 - 04/29/2018 - MOVING OVER TO FLIPPIDY TAP
	// V1.1 - 04/29/2018 - Updated initHintCounter to check if started, also _hintCounterStarted being set to false in awake().  Other bits and pieces.

	private int _hintCounterTick;
	private bool _hintCounterStarted;
	private bool _hintCounterPaused;
	private static float _hintCounterTickDuration = 1f;
	private int _hintCounterTimeout;
	private int _hintHelpCounterTimeout;
	private bool _inHelpMode;
	private int _helpModeIterationAmount;
	private int _helpModeIteration;
	private static int _hintCounterRecursionFactor = 2;
	private IEnumerator _hintCounter;

	new void Awake() {
		base.Awake();
		_hintCounter = hintCounterCoroutine();
		_hintCounterStarted = false;
		_inHelpMode = false;
		_helpModeIterationAmount = 3;
		_helpModeIteration = 0;
	}

	public void initHintCounter(int timeoutArg, int helpTimeoutArg) {
		//printStatus("initHintCounter");
		_hintCounterTick = 0;

		_hintCounterTimeout = timeoutArg > 0 ? timeoutArg : 5;
		_hintHelpCounterTimeout = helpTimeoutArg > 0 ? helpTimeoutArg : 3;

		if (!_hintCounterStarted) {
			_hintCounterStarted = true;
			StartCoroutine(_hintCounter);
		}

		_hintCounterPaused = false;
	}

	private IEnumerator hintCounterCoroutine() {

		var counterTimeout = _hintCounterTimeout;

		while(_hintCounterStarted) {
			
			yield return new WaitForSeconds(_hintCounterTickDuration);
			_hintCounterTick++;
			if(!_hintCounterPaused) {
				if(_inHelpMode) {
					if (_helpModeIteration <= _helpModeIterationAmount) {
						counterTimeout = _hintHelpCounterTimeout;
						_helpModeIteration++;
					} else {
						_inHelpMode = false;
						counterTimeout = _hintCounterTimeout;
					}

				}

				//print(string.Format("counterTimeout: {0}, inHelpMode: {1}, helpModeIteration: {2}", counterTimeout, _inHelpMode, _helpModeIteration));

				if(_hintCounterTick < counterTimeout) {
					//printStatus(string.Format("{0}", _hintCounterTick));
				} else {
					if(_hintCounterTick == counterTimeout || (_hintCounterTick - counterTimeout) % 2 == 0) {
						playHint();
					}
				}
			} else {
				//printStatus("hintCounter paused");
			}
		}
		//printStatus("ending coroutine");
	}

	public void resetHintCounter() {
		_hintCounterTick = 0;
	}

	private void playHint() {
		_gameManagerRef.playMatchHint();
	}

	public void pauseHintCounter(bool pauseStatusArg) {
		_hintCounterPaused = pauseStatusArg;
	}

	public void pauseHintCounter() {
		if(!_hintCounterPaused) {
			_hintCounterPaused = true;
		} else {
			_hintCounterPaused = false;
		}
	}

	public void setHelpMode(bool stateArg) {
		_inHelpMode = stateArg;
		_helpModeIteration = 0;
	}

	public void evaluateAndResetHelpMode() {
		if(_inHelpMode) {
			_helpModeIteration = 0;
		}
	}
}

