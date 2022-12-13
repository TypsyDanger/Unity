using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPopPrefab : MonoBehaviour {
	private MatchPopNumberManager[] _digitRefs;
	private Animator _signAnimator;
	private Animator _iconAnimator;

	public void initNumberSequence(string iconName, string signName, int numberSequence) {
		_signAnimator = gameObject.transform.Find("matchPopContainer").gameObject.transform.Find("matchSigns").GetComponent<Animator>();
		_signAnimator.speed = 0.0f;

		_iconAnimator = gameObject.transform.Find("matchPopContainer").gameObject.transform.Find("matchIcons").gameObject.GetComponent<Animator>();

		_iconAnimator.speed = 0.0f;

		switch(signName) {
			case "multiply":
				_signAnimator.Play("idleMultiply", -1, 0.0f);
				break;
			case "plus":
				_signAnimator.Play("idlePlus", -1, 0.0f);
				break;
		}

		switch(iconName) {
			case "coin":
				_iconAnimator.Play("idleCoin", -1, 0.0f);
				break;
			case "clock":
				_iconAnimator.Play("idleClock", -1, 0.0f);
				break;
		}

		_digitRefs = gameObject.transform.Find("matchPopContainer").gameObject.transform.Find("numberString").gameObject.GetComponentsInChildren<MatchPopNumberManager>();

		if(numberSequence > 99) {
			_digitRefs[0].initNumber(int.Parse(numberSequence.ToString()[0].ToString()));
			_digitRefs[1].initNumber(int.Parse(numberSequence.ToString()[1].ToString()));
			_digitRefs[2].initNumber(int.Parse(numberSequence.ToString()[2].ToString()));
		} else if(numberSequence > 9) {
			_digitRefs[0].initNumber(int.Parse(numberSequence.ToString()[0].ToString()));
			_digitRefs[1].initNumber(int.Parse(numberSequence.ToString()[1].ToString()));
			_digitRefs[2].setBlank();
			
		} else if(numberSequence < 10){
			_digitRefs[0].initNumber(int.Parse(numberSequence.ToString()[0].ToString()));
			_digitRefs[1].setBlank();
			_digitRefs[2].setBlank();
		}
	}
}
