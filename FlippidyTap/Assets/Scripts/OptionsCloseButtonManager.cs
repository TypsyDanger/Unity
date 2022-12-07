using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsCloseButtonManager : MonoExtended {

    private void OnMouseOver(){
        if (Input.GetMouseButtonDown(0)){
            _gameManagerRef.hideGamePlayOptions();
        }
    }
}
