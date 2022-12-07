using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

	/* // OVERVIEW & DESCRIPTION
	* GameManager.cs
	* Author: tim@reallybigsmile.com
	* Last edited: 03/14/2018
	* 
	* This class handles all things, including switching scenes, setting up the other manager objects (game, clock, score), 
	* instantiating them and providing proxy methods they can communicate with eachother through.  The GameManager ensures 
	* that it's persistent between scenes and cannot exist more than once.  It also instantiates and operates an _audioManager object that plays all sounds. 
    * It can be referenced from other objects via the GameManager.
	* 
	* // IMPLEMENTATION AND USAGE
	* 
	* Implementation is in the "Main.unity" scene.  It detects if it already exists and prevents duplicates from being created when the user
	* returns to the "Main" scene.  
	* 
	* Upon instantiation, the GameManager gets the highScore if it exists and displays it.  It also handles scene switches, as well as 
	* setting up references to the Clock, Grid and Score Manager object when appropriate.
	* 
	* Several proxy methods exist to allow the other managers to communicate with eachother without needing to setup references to 
	* eachother inside of themselves.
	* 
	* // METHOD LIST (see actual methods for descriptions).
	* 
	* - Awake()
	* - checkSceneChange
	* - updateHighScoreText
	* - loadScene
	* - getScore
	* - getConsecutiveMatches
	* - resetConsecutiveMatches
    * - startSplashCountdown
	* - updateAndReturnScore
    * - initMainScreen
	* - initGameplay
    * - showAllGameplayUI
    * - hideAllGameplayUI
	* - checkBonusAndAddTimeToClock
    * - checkAndCancelBonus
	* - pauseClock
	* - startClock
	* - clockExpired    
    * - triggerGameOver
    * - showCurtain
    * - hideCurtain
	* - _returnToMain
    * - playSound
    * - incrementCurrentLevel
    * - returnCurrentLevel
	*/

	private static GameManager _instance;
	private static MusicManager _musicManager;
	private GameObject _gridManagerObject;
	private GridManager _gridManagerRef;
	private ClockManager _clockManagerRef;
	private LocalizationManager _localizationManagerRef;
	private ScoreManager _scoreManager; // referenced directly in grid manager via the gameManagerRef object, or whatever it's named.
	private AudioManager _audioManager;
	private SocialManager _socialManager;
	private HintCounterManager _hintCounterManagerRef;
	private int _currentLevel;
	private Animator _gameOverAnimator;
	private Animator _clockImageAnimator;
	private Animator _clockTextAnimator;
	private Animator _clockBGAnimator;
	private Animator _scoreTextAnimator;
	private Animator _scoreBGAnimator;
	private Animator _curtainAnimator;
	private Animator _optionsScreenAnimator;
	private Animator _mainScreenAnimator;
	private Animator _startLockOverlayAnimator;
	private int _hintDuration;
	private int _hintHelpDuration;

	private int _enduroModeInt;

	private bool _previewMode;

	private bool _hasTriggeredPurchase;
	private bool _gamePlayStarted;

	private StartButtonStackManager _startButtonManager;
	private CardSelectionStackManager _cardPackManager;

	private int _gameMode;
	private int _activePack;

	private int _helpModeChanceFactor;
	private int _tapHintChanceFactor;

	private bool _optionsActive;
	private bool _optionsAllowed;

	private string _gameModeMemName;
	private string _musicMutedMemName;
	private string _soundMutedMemName;
	private string _playerBankMemName;
	private string _cardPackMemName;
	private string _foodPackOwnedMemName;
	private string _flagPackOneOwnedMemName;
	private string _fullGameOwnedMemName;
	private string _languageMemName;
	private string _timesPlayedMemName;
	private string _hasReviewedMemName;
	private string _highestLevelMemName;

	private IAPManager _iapManager;

	private int _foodPackOwned;
	private int _flagOnePackOwned;
	private int _fullGameOwned;
	private int _timesPlayed;
	private int _coinValue;
	private int _playerHasReviewed;

	private Animator _levelTextAnimation;

	private string _fullGamePriceString;

	private Animator _lowerGameplayUIForegroundAnimator;
	private Animator _upperGameplayUIForegroundAnimator;
	private bool _preloadComplete;

	private string _purchaseStatusDefaultMessage;

	private Text _fullGamePriceTextObj;
	private Text _purchaseStatusText;
	private Text _store_cardPackLabelText;
	private Text _store_cardPackCopyText;
	private Text _levelTextObj;
	private Text _playerStoreCoins;
	private Text _iap_statusWindowText;
	private Text _game_uiLevelNumberLabel;
	private Text _game_winPopLevelText;

	private string _currentLanguage;

	private LocalizedUILabels _uiLabelDataLabels;

	private PurchaseStatusBackButtonManager _purchaseStatusBackButton;

	private bool _isOnMainScreen;

	private LocalizedCardPackItem[] _cardPackDataArray;

	private string _currentView;
	private const string _VIEW_MAIN_MENU = "view_mainMenu";
	private const string _VIEW_MAIN_MENU_STORE = "view_main_store";
	private const string _VIEW_MAIN_MENU_BUY = "view_main_buy";
	private const string _VIEW_MAIN_MENU_INFO = "view_main_info";
	private const string _VIEW_MAIN_MENU_CREDITS = "view_main_credits";
	private const string _VIEW_MAIN_MENU_OPTIONS = "view_main_options";
	private const string _VIEW_MAIN_MENU_LANGUAGE = "view_main_language";
	private const string _VIEW_MAIN_MENU_RATE = "view_main_rate";
	private const string _VIEW_GAME_PLAY = "view_game_play";
	private const string _VIEW_GAME_OPTIONS = "view_game_options";
	private const string _VIEW_MAIN_MENU_STATUS = "view_main_status";

	void Awake() {
		/* This checks to see if _instance is uninstantiated.  If it is, it assigns the current object to _instance which is used for safety
		 * checks elsewhere in this class to ensure replicates of GameManager aren't instantiated.  It then ensures that this.gameObject can't
		 * be destroyed between scene changes, instantiates the _scoreManager object and uh... does something with activeSceneChanged... not sure what anymore.
		 * If _instance has been instantiated, it destroys the current this.gameObject because it's a replicate and deserves to die.  Stupid replicate.
		 */

		if (!_instance) {
			_instance = this;

			DontDestroyOnLoad(this.gameObject);

			SceneManager.activeSceneChanged += checkSceneChange;

			_coinValue = 4;  // per match

			_scoreManager = this.gameObject.AddComponent<ScoreManager>();
			_audioManager = this.gameObject.AddComponent<AudioManager>();
			_musicManager = this.gameObject.AddComponent<MusicManager>();
			_socialManager = this.gameObject.AddComponent<SocialManager>();
			_localizationManagerRef = this.gameObject.AddComponent<LocalizationManager>();

			_optionsAllowed = false;

			_preloadComplete = false;
			_gamePlayStarted = false;

			_gameModeMemName = "ftap_active_game_mode";
			_musicMutedMemName = "ftap_music_muted";
			_soundMutedMemName = "ftap_sound_muted";
			_playerBankMemName = "ftap_player_coins";
			_cardPackMemName = "ftap_active_card_pack";
			_foodPackOwnedMemName = "ftap_food_pack_owned";
			_flagPackOneOwnedMemName = "ftap_flag_pack_one_owned";
			_fullGameOwnedMemName = "ftap_full_game_owned";
			_languageMemName = "ftap_selected_language";
			_timesPlayedMemName = "ftap_times_played";
			_hasReviewedMemName = "ftap_has_reviewed";
			_highestLevelMemName = "ftap_highest_level";

			_playerHasReviewed = PlayerPrefs.GetInt(_hasReviewedMemName);

			checkLanguageAndInitLocalization(PlayerPrefs.GetString(_languageMemName));

			_timesPlayed = PlayerPrefs.GetInt(_timesPlayedMemName) + 1;
			PlayerPrefs.SetInt(_timesPlayedMemName, _timesPlayed);

			//_localizationManagerRef.initAndLocalize("english");
			//_localizationManagerRef.initAndLocalize("pirate");
			//_localizationManagerRef.initAndLocalize("sa_spanish");

			_cardPackDataArray = _localizationManagerRef.returnCardPackArray();
			_uiLabelDataLabels = _localizationManagerRef.returnUILevelLabels();

			_gameMode = PlayerPrefs.GetInt(_gameModeMemName);
			_activePack = PlayerPrefs.GetInt(_cardPackMemName);
			_foodPackOwned = PlayerPrefs.GetInt(_foodPackOwnedMemName);
			_flagOnePackOwned = PlayerPrefs.GetInt(_flagPackOneOwnedMemName);
			_fullGameOwned = PlayerPrefs.GetInt(_fullGameOwnedMemName);

			if (_fullGameOwned == 1) {
				_coinValue = _coinValue * 2;
			}

			_enduroModeInt = 0;

			_scoreManager.setScoreIncrementAndMemNames(_coinValue, _playerBankMemName, _highestLevelMemName);

			//resetEverythingAndSetBankAmount(10000, false);

			_previewMode = true;

			if (PlayerPrefs.GetInt(_musicMutedMemName) == 1) {
				_musicManager.setMute();
			}

			if (PlayerPrefs.GetInt(_soundMutedMemName) == 1) {
				_audioManager.setMute();
			}

			trackEvent("GAME_STARTED");

		} else {
			Destroy(this.gameObject);
		}
	}

	private void checkLanguageAndInitLocalization(string langArg) {
		_currentLanguage = langArg;
		//print("checkLang: " + _currentLanguage);

		if (_currentLanguage != "") {
			//print("language is not blank");
			_localizationManagerRef.initAndLocalize(_currentLanguage);
		} else {
			//print("language set");
			_localizationManagerRef.initAndLocalize("english");
		}
	}

	public bool isLanguageSet() {
		if(PlayerPrefs.GetString(_languageMemName) != "") {
			return true;
		} else {
			return false;
		}
	}

	// Update is called once per frame
	void Update() {
		if ((Input.GetKeyDown(KeyCode.Escape) && Application.platform == RuntimePlatform.Android) || (Input.GetKeyDown("space"))) {
			/*AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call<bool>("moveTaskToBack", true);
			//Application.Quit();*/
			assessAndroidBackButtonTap();
		}
	}

	private void assessAndroidBackButtonTap() {
		/*
	private const string _VIEW_MAIN_MENU = "view_mainMenu";
	private const string _VIEW_MAIN_MENU_STORE = "view_main_store";
	private const string _VIEW_MAIN_MENU_BUY = "view_main_buy";
	private const string _VIEW_MAIN_MENU_INFO = "view_main_info";
	private const string _VIEW_MAIN_MENU_CREDITS = "view_main_credits";
	private const string _VIEW_MAIN_MENU_OPTIONS = "view_main_options";
	private const string _VIEW_MAIN_MENU_LANGUAGE = "view_main_language";
	private const string _VIEW_MAIN_MENU_RATE = "view_main_rate";
	private const string _VIEW_GAME_PLAY = "view_game_play";
	private const string _VIEW_GAME_OPTIONS = "view_game_options";
	private const string _VIEW_MAIN_MENU_STATUS = "view_main_status";
	*/
		if(_currentView == _VIEW_MAIN_MENU){
			// nothing, for now
		} else if(_currentView == _VIEW_MAIN_MENU_STORE) {
			showMainFromStore();
		} else if(_currentView == _VIEW_MAIN_MENU_BUY) {
			showMainFromPurchase();
		} else if(_currentView == _VIEW_MAIN_MENU_INFO) {
			showMainFromInfo();
		} else if(_currentView == _VIEW_MAIN_MENU_RATE) {
			hideReviewWindow();
		} else if(_currentView == _VIEW_MAIN_MENU_CREDITS) {
			showInfoFromCredits();
		} else if(_currentView == _VIEW_MAIN_MENU_OPTIONS) {
			showInfoFromOptions();
		} else if(_currentView == _VIEW_MAIN_MENU_LANGUAGE) {
			showInfoFromLanguage();
		} else if(_currentView == _VIEW_MAIN_MENU_STATUS) {
			// nothing, for now
		} else if(_currentView == _VIEW_GAME_PLAY) {
			// nothing, for now
		} else if(_currentView == _VIEW_GAME_OPTIONS) {
			hideGamePlayOptions();
		}
	}

	private void checkSceneChange(Scene scene, Scene scenetwo) {
		/* This method first checks to make sure GameManager already exists.  if so, it checks the name of the current scene
		 * and does a switch on it.  If it's GamePlay, it runs loadGridAndSetClock.  If it's Main, it updates the highscore 
		 * using updateHighScoreText
		 * 
		 */
		if (this == _instance) {
			string theScene = SceneManager.GetActiveScene().name;

			if (theScene == "GamePlay") {
				Invoke("initGameplay", 0.25f);
			} else if (theScene == "Main") {
				if (_preloadComplete) {
					initMainScreen();
				} else {
					StartCoroutine(preloadLargeAssets());
				}
			} else if (theScene == "BigSmileSplash") {
				// just wait for it...
			}
		}
	}

	IEnumerator preloadLargeAssets() {
		ResourceRequest mainSong = Resources.LoadAsync("Music/memphis-funk_mainScreenLoop");
		ResourceRequest gameplaySong = Resources.LoadAsync("Music/music_gameplay");

		while (!mainSong.isDone && !gameplaySong.isDone) {
			yield return 0;
		}
		_preloadComplete = true;

		_musicManager.setMusic(mainSong.asset as AudioClip, gameplaySong.asset as AudioClip);

		initMainScreen();

	}

	private void updateHighScoreText() {
		/* Finds and sets the text for the high score using the value of _scoreManager.getHighScore
		 */

		GameObject.Find("ScoreText").GetComponent<UnityEngine.UI.Text>().text = "" + _scoreManager.getHighScore();
	}

	public void loadScene(string theSceneArg) {
		/* Loads a scene by name using the arg
		 */
		_musicManager.stopAudio();
		SceneManager.LoadScene(theSceneArg);
	}

	public int getScore() {
		/* It... gets the score from scoreManager...
		 */

		return _scoreManager.getScore();
	}

	public int getConsecutiveMatches() {
		/* Big shock, but it gets consecutive matches from _scoreManager
		 */

		return _scoreManager.getConsecutiveMatches();
	}

	public void resetConsecutiveMatches() {
		/* Solves world hunger... Actually it resets consecutive matches via the _scoreManager method.
		 */

		_scoreManager.resetConsecutiveMatches();
	}

	public void updateScore() {
		/* Basically adds a match to the score and returns... Not sure where it's used
		 */

		_scoreManager.addMatchToScoreAndReturn();
	}


	private void initMainScreen() {
		//print("initMainScreen");
		_localizationManagerRef.localizeMainScreen();
		_musicManager.playMusic("play_musicMain", true);
		_startButtonManager = GameObject.Find("startButtonStack").GetComponent<StartButtonStackManager>();
		_cardPackManager = GameObject.Find("cardButtonStack").GetComponent<CardSelectionStackManager>();

		_store_cardPackLabelText = GameObject.Find("cardPackLabel").GetComponent<UnityEngine.UI.Text>();
		_store_cardPackCopyText = GameObject.Find("cardPackText").GetComponent<UnityEngine.UI.Text>();
		_iap_statusWindowText = GameObject.Find("iap_statusWindowText").GetComponent<UnityEngine.UI.Text>();


		if (_iapManager != null) {
			Destroy(_iapManager);
		}

		_iapManager = this.gameObject.AddComponent<IAPManager>();

		_mainScreenAnimator = GameObject.Find("MainMenuContainer").GetComponent<Animator>();

		_playerStoreCoins = GameObject.Find("playerStoreCoins").GetComponent<UnityEngine.UI.Text>();
		_fullGamePriceTextObj = GameObject.Find("priceText").GetComponent<Text>();
		_fullGamePriceTextObj.text = _fullGamePriceString;

		_startButtonManager.initStack(_gameMode);
		_cardPackManager.initStack(_activePack);

		updateHighScoreText();
		hideCurtain();
		initStoreBank();


		print(string.Format("playerHasReviewed: {0}, gamePlayStarted: {1}, timesPlayed: {2}", _playerHasReviewed, _gamePlayStarted, _timesPlayed));
		if (_playerHasReviewed != 1) {
			if (_gamePlayStarted && (_timesPlayed == 2 || _timesPlayed % 5 == 0)) {
				showReviewWindow();
			}
		}

		//showReviewWindow();
	}

	private void initGameplay() {
		/* Instantiates a GridManagerPrefab, gets the GridManager script component from it and 
		 * assigns to _gridManagerRef, then gets a reference to the clockText in the scene and
		 * assigns it to _clockManagerRef.  So, this is wierd because I use two different approaches 
		 * to getting access to objects when I could have just used one.  I did the former (load prefab as resource)
		 * strictly for experiementation purposes.  I could have just thrown it in the scene and references it by it's
		 * ID, but I didn't.
		 */
		_gamePlayStarted = true;
		_gameOverAnimator = GameObject.Find("gameOverPop").GetComponent<Animator>();
		_clockBGAnimator = GameObject.Find("clockUI").GetComponent<Animator>();
		_clockTextAnimator = GameObject.Find("clockText").GetComponent<Animator>();
		_clockImageAnimator = GameObject.Find("clockImage").GetComponent<Animator>();
		_scoreBGAnimator = GameObject.Find("scoreUI").GetComponent<Animator>();
		_scoreTextAnimator = GameObject.Find("scoreText").GetComponent<Animator>();
		_curtainAnimator = GameObject.Find("curtain").GetComponent<Animator>();
		_optionsScreenAnimator = GameObject.Find("optionsScreen").GetComponent<Animator>();
		_game_uiLevelNumberLabel = GameObject.Find("game_uiLevelNumberLabel").GetComponent<UnityEngine.UI.Text>();
		_game_winPopLevelText = GameObject.Find("LevelNumber").GetComponent<UnityEngine.UI.Text>();

		setCurrentView(_VIEW_GAME_PLAY);

		if(_hintCounterManagerRef != null) {
			Destroy(this.gameObject.GetComponent<HintCounterManager>());
		}

		_hintCounterManagerRef = this.gameObject.AddComponent<HintCounterManager>();

		_levelTextAnimation = GameObject.Find("game_uiLevelNumberLabel").GetComponent<Animator>();

		_optionsActive = false;

		_currentLevel = 1;


		_hintDuration = 5;
		_hintHelpDuration = 2;
		_helpModeChanceFactor = 3;
		_tapHintChanceFactor = 3;

		if (_gameMode != 0) {
			switch (_gameMode) {
				case 1: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.easyModeLabel; break;
				case 2: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.mediumModeLabel; _hintDuration = 7; break;
				case 3: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.hardModeLabel; _hintDuration = 10; break;
			}
		}

		_upperGameplayUIForegroundAnimator = GameObject.Find("upperGameplayUIForeground").GetComponent<Animator>();
		_lowerGameplayUIForegroundAnimator = GameObject.Find("lowerGameplayUIForeground").GetComponent<Animator>();

		_musicManager.playMusic("play_musicGameplay", true);

		_curtainAnimator.Play("curtainHide");
		Invoke("showAllGameplayUI", .3f);

		_gridManagerObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GridManagerPrefab"), new Vector3(0, 0, 0), Quaternion.identity);
		_gridManagerRef = _gridManagerObject.GetComponent<GridManager>();
		_clockManagerRef = GameObject.Find("clockText").GetComponent<ClockManager>();

		_localizationManagerRef.localizeGamePlay();
		trackEvent("GAMEPLAY_STARTED");
	}

	private void setCurrentView(string viewArg) {
		_currentView = viewArg;
	}



	private void showAllGameplayUI() {
		_scoreBGAnimator.Play("show");
		_lowerGameplayUIForegroundAnimator.Play("show");

		_clockBGAnimator.Play("show");
		_upperGameplayUIForegroundAnimator.Play("show");
	}

	private void hideAllGamePlayUI() {
		_scoreBGAnimator.Play("hide");
		_clockBGAnimator.Play("hide");
		_upperGameplayUIForegroundAnimator.Play("hide");
		_lowerGameplayUIForegroundAnimator.Play("hide");

	}

	public void checkBonusAndAddTimeToClock(int timeArg) {
		/* Add time to clock, used when matches get consecutive match bonuses.
		 */

		if (returnIsEnduroMode()) {
			_clockManagerRef.triggerBonus(timeArg);
		}
	}

	public void setHintDuration(int hintDurationArg) {
		_hintDuration = hintDurationArg;
	}

	public void checkAndCancelBonus() {
		_clockManagerRef.checkBonusModeAndStop();
	}

	public void pauseClock() {
		/* Believe it or not, it pauses the clock.  This is called by GridManager when resetting the grid.
		 */
		if (returnIsEnduroMode()) {
			_clockManagerRef.pauseClockTick();
		}
	}

	public void startClockAfterDelay(float delay) {
		/* Unlike it's predecessor, it starts the clock
		 */

		if (returnIsEnduroMode()) {
			_clockManagerRef.startClockAfterDelay(delay);
		}
	}

	public void clockExpired() {
		/* Clock ran out.  Wait a moment and _returnToMain
		 */

		triggerGameOver();
		_gridManagerRef.destroyAllCardsAndRespondWithFloat();
		float delay = 2.25f;
		Invoke("_returnToMain", delay);


	}

	private void triggerGameOver() {
		_gridManagerRef.setBackgroundSpeed(0);
		_musicManager.stopAudio();
		_audioManager.playSound("play_gameOver");
		_gameOverAnimator.Play("gameOverPop");
		_scoreManager.evaluateHighestLevel(returnCurrentLevel());
		hideAllGamePlayUI();
		trackEvent("GAME_OVER_AT_LEVEL_" + _currentLevel);
		Invoke("showCurtain", 0.5f);
	}

	private void showCurtain() {
		_curtainAnimator.Play("curtainShow");
	}

	private void hideCurtain() {
		if (_curtainAnimator == null) {
			// in case of scene change, use this guy to recreate the curtain
			_curtainAnimator = GameObject.Find("curtain").GetComponent<Animator>();
		}

		_curtainAnimator.Play("curtainHide");
	}

	private void _returnToMain() {
		/* Y'know, sometimes I think it's ironic that I'll write a comment that's longer than the actual code in the method...
		 * but what the hell.  This returns to the Main scene.
		 */

		_musicManager.stopAudio();
		loadScene("Main");
	}

	public void playSound(string soundToPlay) {
		/* This guy facilitates playSound requests from other objects.  It leverages the audioManger object.
         */
		_audioManager.playSound(soundToPlay);
	}

	public void incrementCurrentLevel() {
		if (returnIsEnduroMode()) {
			evaluateAndSetHintDuration(_clockManagerRef.returnTime());
			_currentLevel++;
			trackLevelCount();
		}

		updateChanceFactors(_currentLevel);
	}

	private void updateChanceFactors(int levelArg) {
		if (levelArg <= 10) {
			_helpModeChanceFactor = 2;
			_tapHintChanceFactor = 2;
		} else if (levelArg > 10 && levelArg <= 20) {
			_helpModeChanceFactor = 3;
			_tapHintChanceFactor = 4;
		} else if (levelArg > 20 && levelArg <= 30) {
			_helpModeChanceFactor = 4;
			_tapHintChanceFactor = 4;
		} else if (levelArg > 30 && levelArg <= 40) {
			_helpModeChanceFactor = 5;
			_tapHintChanceFactor = 4;
		} else if (levelArg > 40) {
			_helpModeChanceFactor = 6;
			_tapHintChanceFactor = 6;
		}
	}

	private void evaluateAndSetHintDuration(int timeArg) {
		if(timeArg <= 10) {
			evaluateAndSetHintHelpMode();
		} else if(timeArg > 10 && timeArg < 30) {
			setHintDuration(4);
			_hintCounterManagerRef.setHelpMode(false);
		} else if(timeArg >= 30 && timeArg < 70) {
			setHintDuration(5);
			_hintCounterManagerRef.setHelpMode(false);
		} else if(timeArg >= 70 && timeArg < 100) {
			setHintDuration(6);
			_hintCounterManagerRef.setHelpMode(false);
		} else if(timeArg >= 100) {
			setHintDuration(7);
			_hintCounterManagerRef.setHelpMode(false);
		}
	}

	public int returnCurrentLevel() {
		return _currentLevel;
	}

	private void evaluateAndSetHintHelpMode() {
		var ran = Random.Range(0, _helpModeChanceFactor);
		if(ran == 1) {
			//print("setting help mode: " + ran);
			_hintCounterManagerRef.setHelpMode(true);
		} else {
			//print("no help mode this time: " + ran);
		}
	}

	public void loadSocialHighscores() {
		_socialManager.loadSocialHighscores();
	}

	public void gotoFacebookPage() {
		Application.OpenURL("http://www.facebook.com/Flippidy-Tap-154354238729738/");
	}

	public void switchMainUIButton(string direction, string parent) {
		switch (parent) {
			case "start":
				_startButtonManager.switchButton(direction);
				break;
			case "cards":
				_cardPackManager.switchButton(direction);
				break;
		}
	}

	public void setGameMode(int gameMode) {
		_gameMode = gameMode;
		PlayerPrefs.SetInt(_gameModeMemName, gameMode);
	}

	public int returnGameMode() {
		return _gameMode;
	}

	public void checkFullGameAndStartLevel() {
		if (_fullGameOwned != 1 && _gameMode == 0) {
			loadScene("GamePlay");
		} else if (_fullGameOwned == 1) {
			loadScene("GamePlay");
		}
	}

	public void updateLevelUI() {
		if (_levelTextAnimation == null) {
			_levelTextAnimation = GameObject.Find("levelText").GetComponent<Animator>();
		}

		_levelTextAnimation.Play("pulse");

		switch (_gameMode) {
			case 0: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.winPopLevelPrefix + _currentLevel; break;
			case 1: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.easyModeLabel; break;
			case 2: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.mediumModeLabel; break;
			default: _game_uiLevelNumberLabel.text = _uiLabelDataLabels.hardModeLabel; break;
		}
	}

	public int returnActivePack() {
		return _activePack;
	}

	public void updateWinPopLevelText() {
		switch (_gameMode) {
			case 0: _game_winPopLevelText.text = _uiLabelDataLabels.winPopLevelPrefix + _currentLevel; break;
			case 1: _game_winPopLevelText.text = _uiLabelDataLabels.easyModeLabel; break;
			case 2: _game_winPopLevelText.text = _uiLabelDataLabels.mediumModeLabel; break;
			default: _game_winPopLevelText.text = _uiLabelDataLabels.hardModeLabel; break;
		}
	}

	public void showGamePlayOptions() {
		setCurrentView(_VIEW_GAME_OPTIONS);
		if (!_optionsActive && _optionsAllowed) {
			_optionsActive = true;
			_optionsScreenAnimator.Play("show");
			_gridManagerRef.pause();
			if (returnIsEnduroMode()) {
				_clockManagerRef.pause();
			}
		}
	}

	public void hideGamePlayOptions() {
		if (_optionsActive) {
			_optionsActive = false;
			_optionsScreenAnimator.Play("hide");
			playSound("play_flipped");
			_gridManagerRef.unPause();
			if (returnIsEnduroMode()) {
				_clockManagerRef.unPause();
			}
		}
	}

	public void toggleMuteMusic() {
		if (_musicManager.toggleMute()) {
			PlayerPrefs.SetInt(_musicMutedMemName, 1);
		} else {
			PlayerPrefs.SetInt(_musicMutedMemName, 0);
		}
	}


	public void toggleMuteSound() {
		if (_audioManager.toggleMute()) {
			PlayerPrefs.SetInt(_soundMutedMemName, 1);
		} else {
			PlayerPrefs.SetInt(_soundMutedMemName, 0);
		}
	}

	public bool isMusicMuted() {
		return _musicManager.returnMuted();
	}


	public bool isAudioMuted() {
		return _audioManager.returnMuted();
	}

	public void enablePlayMode() {
		_optionsAllowed = true;
		_hintCounterManagerRef.initHintCounter(_hintDuration, _hintHelpDuration);
	}

	public void disablePlayMode() {
		_optionsAllowed = false;
		_hintCounterManagerRef.pauseHintCounter();
	}

	public void showStoreFromMain() {
		_mainScreenAnimator.Play("showStore");
		setCurrentView(_VIEW_MAIN_MENU_STORE);
		playSound("play_flipped");
	}

	public void showPurchaseOverlay() {
		_mainScreenAnimator.Play("showPurchaseOverlay");
		playSound("play_flipped");
		setCurrentView(_VIEW_MAIN_MENU_BUY);
		initPurchaseStatus();
	}

	private void showReviewWindow() {
		setCurrentView(_VIEW_MAIN_MENU_RATE);
		_mainScreenAnimator.Play("showReviewWindow");
	}

	public void hideReviewWindow() {
		setCurrentView(_VIEW_MAIN_MENU);
		_mainScreenAnimator.Play("hideReviewWindow");
		playSound("play_flipped");
	}

	public void showMainFromStore() {
		setCurrentView(_VIEW_MAIN_MENU);
		_mainScreenAnimator.Play("showMainFromStore");
		playSound("play_flipped");
	}

	public void showCreditsFromInfo() {
		setCurrentView(_VIEW_MAIN_MENU_CREDITS);
		_mainScreenAnimator.Play("showCreditsFromInfo");
		trackEvent("CREDITS_VIEWED");
		playSound("play_flipped");
	}

	public void showMainFromInfo() {
		setCurrentView(_VIEW_MAIN_MENU);
		_mainScreenAnimator.Play("showMainFromInfo");
		playSound("play_flipped");
	}

	public void showInfoFromOptions() {
		setCurrentView(_VIEW_MAIN_MENU_INFO);
		_mainScreenAnimator.Play("showInfoFromOptions");
		playSound("play_flipped");
	}

	public void showInfoFromLanguage() {
		setCurrentView(_VIEW_MAIN_MENU_INFO);
		trackEvent("INFO_VISITED");
		_mainScreenAnimator.Play("showInfoFromLanguage");
		playSound("play_flipped");
	}

	public void showLanguageFromInfo() {
		setCurrentView(_VIEW_MAIN_MENU_LANGUAGE);
		_mainScreenAnimator.Play("showLanguageFromInfo");
		playSound("play_flipped");
	}	

	public void showMainFromPurchase() {
		setCurrentView(_VIEW_MAIN_MENU);
		_mainScreenAnimator.Play("showMainFromPurchaseOverlay");
		playSound("play_flipped");
	}

	public void showInfoFromCredits() {
		setCurrentView(_VIEW_MAIN_MENU_INFO);
		_mainScreenAnimator.Play("showInfoFromCredits");
		playSound("play_flipped");
	}

	public void showPurchaseFromStatus() {
		setCurrentView(_VIEW_MAIN_MENU_BUY);
		_mainScreenAnimator.Play("showPurchaseFromStatus");
		StartCoroutine(resetPurchaseStatus(0.4f));
	}

	public void showMainFromStatus() {
		setCurrentView(_VIEW_MAIN_MENU);
		_mainScreenAnimator.Play("showMainFromStatus");
		playSound("play_flipped");
	}

	public void showOptionsFromInfo() {
		setCurrentView(_VIEW_MAIN_MENU_OPTIONS);
		_mainScreenAnimator.Play("showOptionsFromInfo");
		playSound("play_flipped");
	}

	public void showStatusFromPurchase() {
		setCurrentView(_VIEW_MAIN_MENU_STATUS);
		_mainScreenAnimator.Play("showStatusFromPurchase");
		playSound("play_flipped");
	}

	public void hideFullGameBuyOverlay() {
		showMainFromPurchase();
	}

	public void showInfoFromMain() {
		setCurrentView(_VIEW_MAIN_MENU_INFO);
		_mainScreenAnimator.Play("showInfoFromMain");
		playSound("play_flipped");
	}

	private void initStoreBank() {
		_playerStoreCoins.text = "" + _scoreManager.getHighScore();
	}

	public void showMainAndReviewGame() {
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.bigsmile.flippidytap");
		trackEvent("REVIEW_FROM_ANDROID");
		#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/id1370093327");
		trackEvent("REVIEW_FROM_IOS");
		#endif
		_playerHasReviewed = 1;
		PlayerPrefs.SetInt(_hasReviewedMemName, 1);
		_mainScreenAnimator.Play("hideReviewWindow");
	}

	public void selectLanguage(string langArg) {
		PlayerPrefs.SetString(_languageMemName, langArg);
		var theScene = SceneManager.GetActiveScene().name;
		if(theScene == "Main") {
			reLocalizeGame(langArg);
		} else if(theScene == "LanguageSet") {
			checkLanguageAndInitLocalization(langArg);
			GameObject.Find("LanguageBox").GetComponent<Animator>().Play("slideOut", -1, 0f);
		}
		trackEvent("LANGUAGE_SELECTED_" + langArg);
	}

	public void reLocalizeGame(string langArg) {
		_localizationManagerRef.initAndLocalize(langArg);
		_cardPackDataArray = _localizationManagerRef.returnCardPackArray();
		_uiLabelDataLabels = _localizationManagerRef.returnUILevelLabels();
		_localizationManagerRef.localizeMainScreen();
		setCardPackTexts(_cardPackManager.returnActivePack());

		showInfoFromLanguage();
	}

	public void updatePlayerAndStoreBank(int amountArg) {
		_playerStoreCoins.text = "" + _scoreManager.updateHighScoreAndReturn(amountArg);
	}

	public void initScore() {
		_scoreManager.resetScoreAndUI();
	}


	public void selectCardPack() {
		_activePack = _cardPackManager.returnActivePack();
		_cardPackManager.selectCurrentPack();
		PlayerPrefs.SetInt(_cardPackMemName, _activePack);
		trackEvent("CARD_PACK_SELECTED_" + _activePack);
	}

	public int returnPackOwned(int packNumber) {
		int returnVal = -1;

		switch (packNumber) {
			case 0:
				returnVal = 1;
				break;
			case 1:
				returnVal = _foodPackOwned;
				break;
			case 2:
				returnVal = _flagOnePackOwned;
				break;
		}

		return returnVal;
	}

	public int returnFullGameOwned() {
		return _fullGameOwned;
	}

	public void buyCardPack() {
		if (_scoreManager.getHighScore() < 10000) {
			_cardPackManager.playPurchaseFail();
		} else {
			int dummy = _scoreManager.updateHighScoreAndReturn(-10000);
			switch (_cardPackManager.returnActivePack()) {
				case 1:
					PlayerPrefs.SetInt(_foodPackOwnedMemName, 1);
					_foodPackOwned = 1;
					trackEvent("CARD_PACK_BOUGHT_" + 1);
					//print("purchase food pack " + PlayerPrefs.GetInt(_foodPackOwnedMemName));
					break;
				case 2:
					PlayerPrefs.SetInt(_flagPackOneOwnedMemName, 1);
					_flagOnePackOwned = 1;
					trackEvent("CARD_PACK_BOUGHT_" + 2);
					//print("purchase flag pack " + PlayerPrefs.GetInt(_flagPackOneOwnedMemName));
					break;
			}
			_cardPackManager.playPurchaseSuccess();
			initStoreBank();
		}
	}

	public void playWinFanFare() {
		if (returnIsEnduroMode()) {
			if ((returnCurrentLevel() - 1) < 5) {
				playSound("play_win_" + (returnCurrentLevel() - 1));
			} else {
				playSound("play_win_5");
			}
		} else {
			playSound("play_win_3");
		}
		StartCoroutine(_musicManager.dropWinVolume(2.25f));
	}

	private void resetEverythingAndSetBankAmount(int bankAmount, bool isFullGameOwned) {
		print("!!!GAME IS CLEARLY IN DEBUG MODE!!! THIS SHOULD BE INACTIVE!!!");
		PlayerPrefs.SetInt(_fullGameOwnedMemName, 0);
		PlayerPrefs.SetInt(_foodPackOwnedMemName, 0);
		PlayerPrefs.SetInt(_flagPackOneOwnedMemName, 0);
		PlayerPrefs.SetInt(_cardPackMemName, 0);
		PlayerPrefs.SetInt(_gameModeMemName, 0);
		PlayerPrefs.SetString(_languageMemName, "");
		PlayerPrefs.SetInt(_hasReviewedMemName, 0);
		if (isFullGameOwned) {
			_fullGameOwned = 1;
		} else {
			_fullGameOwned = 0;
		}

		_scoreManager.updateHighScoreAndReturn(bankAmount);
	}

	public void buyFullGame() {
		//print("game manager is attempting to buy full game");
		showStatusFromPurchase();
		StartCoroutine(_iapManager.BuyFullGame(1.0f));
		//StartCoroutine(_iapManager.BuyError(8, 1.0f));
		_hasTriggeredPurchase = true;
	}

	public void restoreFullGame() {
		showStatusFromPurchase();
		StartCoroutine(_iapManager.attemptRestore(1.0f));
		_hasTriggeredPurchase = true;
	}

	public void buyError() {
		//print("this is going to try to buy a non-existant IAP to test error handling with the app store");
		showStatusFromPurchase();
		StartCoroutine(_iapManager.BuyError(0, 1.0f));
		_hasTriggeredPurchase = true;
	}

	public void setFullGamePurchased() {
		if (_hasTriggeredPurchase) {
			//print("Full Game Owned!!");
			_hasTriggeredPurchase = false;
			showMainFromStatus();
			StartCoroutine(_startButtonManager.playPurchaseSuccess(0.5f));
			updatePlayerAndStoreBank(10000);
			_scoreManager.doubleCoinRate();
			_fullGameOwned = 1;
			PlayerPrefs.SetInt(_fullGameOwnedMemName, 1);
			StartCoroutine(playStartUnlock(1.5f));
			trackEvent("FULL_GAME_PURCHASED");
		} else {
			//print("unity IAP reported purchase but game hasn't started...");
			if (PlayerPrefs.GetInt(_fullGameOwnedMemName) == 0) {
				PlayerPrefs.SetInt(_fullGameOwnedMemName, 1);
				_fullGameOwned = 1;
				//print("full game not owned but IAP manager claimed it was...... setting to purchased.");
			} else {
				//print("full game owned, and IAP manager confirmed that.  Good dog.");
			}

		}

	}

	private IEnumerator playStartUnlock(float delay) {
		yield return new WaitForSeconds(delay);
		_startButtonManager.playStartUnlock();
	}

	private void initPurchaseStatus() {
		_purchaseStatusText = GameObject.Find("iap_statusWindowText").GetComponent<Text>();
		_purchaseStatusBackButton = GameObject.Find("purchaseStatusBackButton").GetComponent<PurchaseStatusBackButtonManager>();
		_purchaseStatusDefaultMessage = _localizationManagerRef.returnStatusCode(8);
		_purchaseStatusText.text = _purchaseStatusDefaultMessage;

	}

	public void purchaseError(int errorArg, string subArgOne, string subArgTwo) {
		var tempArgOne = "" + subArgOne;
		var tempArgTwo = "" + subArgTwo;
		var statusMessage = string.Format(_localizationManagerRef.returnStatusCode(errorArg), tempArgOne, tempArgTwo);
		_iap_statusWindowText.text = statusMessage;

		trackEvent(statusMessage);

		_purchaseStatusBackButton.showButton();
	}

	public IEnumerator resetPurchaseStatus(float delay) {
		//print("resetPurchaseStatus");
		yield return new WaitForSeconds(delay);
		//_localizationManagerRef.setIapStatusText(99, "", "");	// reset
		_purchaseStatusText.text = _purchaseStatusDefaultMessage;
		_purchaseStatusBackButton.hideButton();
	}

	public void setFulLGamePrice(string priceArg) {
		_fullGamePriceString = priceArg;
		if (_fullGamePriceTextObj.text == "") {
			_fullGamePriceTextObj.text = priceArg;
		}
		trackEvent("GAME_PRICE_" + priceArg);
	}

	public int returnCurrentCoinValue() {
		return _scoreManager.returnCurrentCoinValue();
	}

	public bool returnPreviewMode() {
		return _previewMode;
	}

	public int returnEnduroModeInt() {
		return _enduroModeInt;
	}

	public bool returnIsEnduroMode() {
		return (_gameMode == _enduroModeInt);
	}

	public void setCardPackTexts(int numArg) {
		_store_cardPackLabelText.text = _cardPackDataArray[numArg].cardPackLabel;
		_store_cardPackCopyText.text = _cardPackDataArray[numArg].cardPackCopy;
	}

	public void playMatchHint() {
		_gridManagerRef.playHiddenCounterPartHint();
	}

	public void pauseHintCounter(bool state) {
		_hintCounterManagerRef.pauseHintCounter(state);
	}

	public void resetHintCounter() {
		_hintCounterManagerRef.resetHintCounter();
	}

	public void evaluateAndResetHelpMode() {
		_hintCounterManagerRef.evaluateAndResetHelpMode();
	}

	public void updateScoreBoard(int scoreArg) {
		_socialManager.submitHighScore(scoreArg);
	}

	public void trackLevelCount() {
		print("tracking level: "+_currentLevel);
		Analytics.CustomEvent("START_LEVEL_"+_currentLevel, new Dictionary<string, object> {
			{ "timeLeft", _clockManagerRef.returnTime() }
		});
	}

	public void trackEvent(string eventArg) {
		print(string.Format("tracking event: {0}", eventArg));
		Analytics.CustomEvent(eventArg);
	}

	/*private void OnApplicationFocus(bol focus) {
		print("application came back: " + focus);
		if(focus) {
			if (_iapManager.returnPurchaseAttemptInProgress()) {
				purchaseError("Did you cancel a purchase manually? If not, hold tight and finish what you're doing.");
			}
		}
	}*/
}