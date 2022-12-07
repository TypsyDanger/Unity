using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonManager : MonoExtended {

    private void OnMouseOver(){
		if (Input.GetMouseButtonDown(0)){
            _gameManagerRef.showStoreFromMain();    
        }
    }
}
