using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPopNumberManager : MonoBehaviour {
	private Animator _numberAnimator;

	// Use this for initialization
	void Start () {
		
	}

	private void Awake() {
	}

	public void initNumber(int numberArg) {
		initNumberAnimator();
		float theNumber = numberArg / 10f;
		_numberAnimator.speed = 0.0f;
		_numberAnimator.Play("oneToNine", -1, theNumber);
	}

	public void setBlank() {
		initNumberAnimator();
		_numberAnimator.Play("blank", -1, 0f);
	}

	private void initNumberAnimator() {
		_numberAnimator = gameObject.GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update () {
		
	}
}
