using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArrowManager : MonoBehaviour {

    private GameManager _gameManagerRef;
	public string targetParent;

    void Start () {
        _gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.switchMainUIButton("next", targetParent);
        }
    }
}
