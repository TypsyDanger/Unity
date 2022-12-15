using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFabScripts.Handlers;

namespace Managers
{
    public class PlayfabManager : MonoBehaviour
    {
        public void Awake()
        {
            StartCoroutine(RunTests());
        }

        public IEnumerator RunTests()
        {
            Debug.Log("Attempting Login...");
            PlayFabLoginHandler PlayFabLogin = new PlayFabLoginHandler();
            PlayFabLogin.DoLogin();
            yield return new WaitWhile(() => PlayFabPlayerProfile.IsLoggedIn == false);
            
            Debug.Log("Attempting to get player title data...");
            PlayFabPlayerTitleDataHandler PlayFabDataHandler = new PlayFabPlayerTitleDataHandler();
            PlayFabDataHandler.GetData();
            yield return new WaitWhile(() => PlayFabPlayerProfile.TitleData == null);
            
            Debug.Log(string.Format("TitleData 'lastLogin': {0}", PlayFabPlayerProfile.TitleData.Find(x => x.Key == "lastLogin").Value));
            
            Debug.Log("All tests complete");
        }
    }
}