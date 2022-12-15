using System;
using Managers;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Utils;

namespace PlayFabScripts.Handlers
{
    public class PlayFabLoginHandler
    {

        private Action _successCallback;
        private Action _errorCallback;
        public void DoLogin(Action theSuccessCallback = null, Action theErrorCallback = null) {

            _successCallback = theSuccessCallback;
            _errorCallback = theErrorCallback;

            if (!Validation.ValidatePlayFabTitleId())
            {
                Debug.Log("ERROR: The Playfab Title ID cannot be set.");
                return;
            }

            LoginWithCustomIDRequest loginRequest = new LoginWithCustomIDRequest
            {
                CustomId = Utils.GetUniqueIdentifier.generate(),
                CreateAccount = true
            };
            
            PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnLoginError);
        }

        private void OnLoginSuccess(LoginResult theResult)
        {
            PlayFabPlayerProfile.SetLoggedIn(theResult.PlayFabId);
            Debug.Log(string.Format("PLAYFAB: LOGIN SUCCESSFUL FOR {0}", PlayFabPlayerProfile.PlayFabId));
            
            if (_successCallback != null)
            {
                _successCallback();
            }
        }

        private void OnLoginError(PlayFabError theError)
        {
            Debug.Log(string.Format("PLAYFAB: LOGIN ERROR: {0}", theError.ErrorMessage));

            if (_errorCallback != null)
            {
                _errorCallback();
            }
        }
    }
}