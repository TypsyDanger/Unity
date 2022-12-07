﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour {

    private GameManager _gameManagerRef;

    void Start(){
        _gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update(){

    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)){
            _gameManagerRef.loadSocialHighscores();
        }
    }
}
