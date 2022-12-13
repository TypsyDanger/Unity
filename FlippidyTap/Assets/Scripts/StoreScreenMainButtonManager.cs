using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScreenMainButtonManager : MonoBehaviour {
    private GameManager _gameManagerRef;

    void Start () {
        _gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseOver(){
		if (Input.GetMouseButtonDown(0)){
			_gameManagerRef.playSound("play_flipped");
            _gameManagerRef.showMainFromStore();
        }
    }
}
