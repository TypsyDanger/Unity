using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFabHandlers;

namespace Managers
{
    public class PlayfabManager : MonoBehaviour
    {
        public void Awake()
        {
            PlayFabLoginHandler PlayFabLogin = new PlayFabLoginHandler();

            PlayFabLogin.DoLogin();
        }
    }
}F