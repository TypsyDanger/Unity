using System.Collections.Generic;
using PlayFabScripts;
using UnityEngine;

namespace Managers
{
    public static class PlayFabPlayerProfile
    {
        public static string PlayFabId { get; set; }
        public static bool IsLoggedIn { get; set; }
        public static List<KeyValuePair<string, string>> TitleData { get; set; }
    
        public static KeyValuePair<string, string> AddToTitleData(KeyValuePair<string, string> theDatum)
        {
            return theDatum;
        }

        public static KeyValuePair<string, string> GetFromTitleData(string theKey)
        {
            // TODO: Make this work
            return new KeyValuePair<string, string>();
        }

        public static void SetLoggedIn(string thePlayerId)
        {
            if (string.IsNullOrEmpty(thePlayerId))
            {
                Debug.Log("ERROR: Player ID cannot be null or empty.");
                return;
            }

            PlayFabId = thePlayerId;
            IsLoggedIn = true;
        }
    }
}