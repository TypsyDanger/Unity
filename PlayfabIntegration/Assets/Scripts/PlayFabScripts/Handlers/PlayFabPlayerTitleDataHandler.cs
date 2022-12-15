using System;
using System.Collections.Generic;
using Managers;
using Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFabScripts.Handlers
{
    public class PlayFabPlayerTitleDataHandler
    {
        private Action _successCallback;
        private Action _errorCallback;
        
        public void GetData(Action theSuccessCallback = null, Action theErrorCallback = null)
        {
            _successCallback = theSuccessCallback;
            _errorCallback = theErrorCallback;
            
            if (!Validation.ValidatePlayFabSession())
            {
                Debug.Log("ERROR: Playfab is not logged in properly");
                return;
            }

            GetUserDataRequest dataRequest = new GetUserDataRequest
            {
                PlayFabId = PlayFabPlayerProfile.PlayFabId
            };

            PlayFabClientAPI.GetUserData(dataRequest, OnUserDataRequestSuccess, OnUserDataRequestError);
        }

        private void OnUserDataRequestSuccess(GetUserDataResult theResult)
        {
            Debug.Log("PLAYFAB: Received title data successfully");
            PlayFabPlayerProfile.TitleData = ConvertPlayFabTitleDataToList(theResult.Data);

            if (_successCallback != null)
            {
                _successCallback();
            }
        }

        private void OnUserDataRequestError(PlayFabError theError)
        {
            Debug.Log(string.Format("ERROR: {0}", theError.ErrorMessage));

            if (_errorCallback != null)
            {
                _errorCallback();
            }
        }

        private List<KeyValuePair<string, string>> ConvertPlayFabTitleDataToList(Dictionary<string, UserDataRecord> theTitleData)
        {
            List<KeyValuePair<string, string>> convertedList = new List<KeyValuePair<string, string>>();
            foreach (var theDatum in theTitleData)
            {
                convertedList.Add(new KeyValuePair<string, string>(theDatum.Key, theDatum.Value.Value.ToString()));
            }

            if (convertedList.Count <= 0)
            {
                Debug.Log("ERROR: The converted list was empty.");
                return new List<KeyValuePair<string, string>>();
            }

            return convertedList;
        }
    }
}