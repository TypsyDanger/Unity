using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Constants;
using Utils;

namespace PlayFabHandlers
{
    public class PlayFabLoginHandler
    {
        public void DoLogin()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = Constants.PlayfabConstants.PLAYFAB_TITLE_ID;
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
            Debug.Log(string.Format("PLAYFAB: LOGIN SUCCESSFUL FOR {0}", theResult.PlayFabId));
        }

        private void OnLoginError(PlayFabError theError)
        {
            Debug.Log(string.Format("PLAYFAB: LOGIN ERROR: {0}", theError.ErrorMessage));
        }
    }
}