using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMuteToggleManager : MonoBehaviour {

    private GameManager _gameManagerRef;
    private Animator _animatorRef;

	// Use this for initialization
	void Start () {
        toggleButton();
	}

    void Awake() {
        _gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
        _animatorRef = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    private void toggleButton() {
        if (_gameManagerRef.isMusicMuted()){
            _animatorRef.Play("idleOn");
        } else {
            _animatorRef.Play("idleOff");
        }
    }

    private void OnMouseDown(){
        if (Input.GetMouseButtonDown(0)){
            _gameManagerRef.toggleMuteMusic();
            toggleButton();
        }
    }
}
