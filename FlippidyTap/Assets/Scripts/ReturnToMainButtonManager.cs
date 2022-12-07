using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainButtonManager : MonoBehaviour {

    private GameManager _gameManagerRef;

	// Use this for initialization
	void Start () {
        _gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver () {
        if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.playSound("play_flipped");
            _gameManagerRef.loadScene("Main");
        }
    }
}
