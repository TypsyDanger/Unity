using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButtonManager : MonoExtended {

    private void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)) {
			_gameManagerRef.playSound("play_flipped");
            _gameManagerRef.showGamePlayOptions();
        }
    }
}
