using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class SocialManager : MonoBehaviour {
	private bool _authenticated;
	private string _scoreBoardName;
	
	void Start() {
		_authenticated = false;
		checkAuthentication();
	}

	void Awake() {
		#if UNITY_ANDROID
		_scoreBoardName = "CgkI7N_OtLcEEAIQAQ";
		#elif UNITY_IPHONE
		_scoreBoardName = "grp.flippidytapleaderboard";
		#endif
	}

	private void checkAuthentication() {
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    private void ProcessAuthentication(bool success) {
        if (success) {
            _authenticated = true;
        } else {
            print("Failed to authenticate");
        }
        
    }
    private void ProcessLoadedAchievements(IAchievement[] achievements) {
        if (achievements.Length == 0) {
            //print("Error: no achievements found");
        } else {
            //print("Got " + achievements.Length + " achievements");
        }

        Social.ReportProgress("Achievement01", 100.0, result => {
			if (result) {
				//print("Successfully reported achievement progress");
			} else {
				//print("Failed to report achievement");
			}
        });

    }

    public void loadSocialHighscores() {
        if(!_authenticated) {
            checkAuthentication();
        }
        Social.ShowLeaderboardUI();
    }

	public void submitHighScore(int highScoreArg) {
		print("submitHighScore: " + highScoreArg);
		Social.ReportScore((long)highScoreArg, _scoreBoardName, catchSubmitHighScore);
	}

	private void catchSubmitHighScore(bool result) {
		if (result) {
			Debug.Log("score submission successful");
		} else {
			Debug.Log("score submission failed");
		}
	}
}
