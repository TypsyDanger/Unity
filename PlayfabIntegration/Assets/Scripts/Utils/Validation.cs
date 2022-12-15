using System;
using Constants;
using Managers;
using PlayFab;
using UnityEngine;

namespace Utils
{
    public static class Validation
    {
        public static bool ValidatePlayFabSession()
        {
            if (!ValidatePlayFabPlayerLogin())
            {
                Debug.Log("ERROR: PlayFab Login Session not found.");
                return false;
            }

            if (!ValidatePlayFabTitleId())
            {
                Debug.Log("ERROR: The PlayFab Title Data could not be verified.");
                return false;
            }

            return true;
        }

        public static bool ValidatePlayFabPlayerLogin()
        {
            if (!PlayFabPlayerProfile.IsLoggedIn)
            {
                Debug.Log(string.Format("ERROR: The player is not logged-in."));
                return false;
            }

            return true;
        }

        public static bool ValidatePlayFabTitleId()
        {
            try
            {
                if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
                {
                    PlayFabSettings.staticSettings.TitleId = PlayfabConstants.PLAYFAB_TITLE_ID;
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("ERROR: {0}", e.Message));
                return false;
            }
        }
    }
}