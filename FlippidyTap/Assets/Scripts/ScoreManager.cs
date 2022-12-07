using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	/* // OVERVIEW & DESCRIPTION
	* ScoreManager.cs
	* Author: tim@reallybigsmile.com
	* Last edited: 02/06/2018
	* 
	* This class handles all things related to score tallying, including saving/restoring highscore.  
	* 
	* // IMPLEMENTATION AND USAGE
	* 
	* Implmentation and usage is primarly happening in GameManager.  GameManager creates a private object of class ScoreManager,
    * Then initializes with the base score increment.  There's also some particle effects involved when matches occur.
	* 
	* GameManager detects when the "Main" (Title Screen) scene is loaded and makes a call to ScoreManager to fetch the highscore.
	* 
	* GameManager has other proxy methods that interact with ScoreManager, which are made available to other objects int he project (like the GridManager).
	* 
	* // METHOD LIST (see actual methods for descriptions.
	* 
	* - ScoreManager()
    * - setScoreIncrementAndInit()
	* - returnSelf()
	* - resetScoreAndUI() 
	* - getScore()
	* - getHighScore()
	* - addMatchToScoreAndReturn()
    * - fireParticles()
	* - getConsecutiveMatches()
	* - resetConsecutiveMatches()
	*/

	private int _theScore;                          // Not sure how I used this yet.
	private int _theHighScore;
	private int _consecutiveMatches;
	private int _baseScore;
	private GameObject _scoreCanvasObj;
	private string _highScoreMemName;
	private Text _scoreTextObj;
	private ParticleSystem _particleEmitter;
	private GameManager _gameManagerRef;
	private string _highestLevelMemName;


	public ScoreManager() {
		/* ScoreManager Description
		 * 
		 * CONSTRUCTOR... neato.
		 * 
		 */

	}

	public ScoreManager returnSelf() {
		/* returnSelf description
		 * 
		 * Simply returns this... likely not used
		 */
		// print ("do I exist?: " + this);
		return this;
	}

	void Awake() {

	}

	private void initHighScore() {
		_theHighScore = PlayerPrefs.GetInt(_highScoreMemName);
	}

	public void setScoreIncrementAndMemNames(int baseScoreArg, string scoreMemNameArg, string highestLevelMemNameArg) {
		/*
         * This is called from GameManager.  It sets the stage with the baseScoreArg, then instantiates all of the bits and pieces.
         */
		_baseScore = baseScoreArg;
		_highScoreMemName = scoreMemNameArg;
		_highestLevelMemName = highestLevelMemNameArg;
	}

	// Update is called once per frame
	void Update() {

	}

	public void resetScoreAndUI() {
		/* resetScoreAndUI description
		 * 
		 * Gets a reference to the score text in the gameplay scene, assigns it to _scoreTextObj and sets to "0", and zeroes out _consecutiveMatches.
		 * 
		 * */

		_scoreTextObj = GameObject.Find("scoreText").GetComponent<Text>();
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_particleEmitter = _scoreTextObj.gameObject.AddComponent<ParticleSystem>();
		_consecutiveMatches = 0;
		_theScore = getHighScore();
		_scoreTextObj.text = "" + _theScore;
	}

	public int getScore() {
		/* getScore description
		 * 
		 * Returns _theScore
		 * 
		 * */

		return _theScore;
	}

	public int getHighScore() {
		/* getHighScore description
		 * 
		 * Returns _theHighScore
		 * 
		 * */
		_theHighScore = PlayerPrefs.GetInt(_highScoreMemName);

		return _theHighScore;
	}

	public int updateHighScoreAndReturn(int amountArg) {
		_theHighScore += amountArg;
		updateSavedHighScore();

		return _theHighScore;
	}

	private void updateSavedHighScore() {
		PlayerPrefs.SetInt(_highScoreMemName, _theHighScore);
	}

	public int addMatchToScoreAndReturn() {
		/* addMatchToScoreAndReturn description
		 * 
		 * Adds new score to current score, but applies bonus score if the latest match was a consecutive match
		 * 
		 * */

		if (_consecutiveMatches > 0) {
			_theScore += _baseScore + (_baseScore * _consecutiveMatches);
		} else {
			_theScore += _baseScore;
		}

		_consecutiveMatches++;

		//fireParticles();

		if (_theHighScore < _theScore) {
			_theHighScore = _theScore;
		}

		updateSavedHighScore();

		_scoreTextObj.text = "" + _theScore;

		return _theScore;
	}

	private void fireParticles() {
		/*
         * This guy fires off particles, it's called from addMatchToScoreAndReturn()
         */

		if (_particleEmitter == null) {
			// _particleEmitter = _scoreTextObj.gameObject.AddComponent<ParticleSystem>();
		}

		_particleEmitter.Emit(10);
	}

	public int getConsecutiveMatches() {
		/* getConsecutiveMatches description
		 * 
		 * Returns _consecutiveMatches
		 * 
		 * */

		return _consecutiveMatches;
	}

	public void resetConsecutiveMatches() {
		/* resetConecutiveMatches Description
		 * 
		 * Sets _consecutiveMatches to 0
		 * 
		 * */

		// print ("resetConsecutiveMatches();");

		_consecutiveMatches = 0;
	}

	public void doubleCoinRate() {
		_baseScore = _baseScore * 2;
	}

	public int returnCurrentCoinValue() {
		return _baseScore;
	}

	public void evaluateHighestLevel(int levelArg) {
		print("evaluateHighScore: " + levelArg + ", using : " + _highestLevelMemName);
		if(PlayerPrefs.GetInt(_highestLevelMemName) < levelArg) {
			PlayerPrefs.SetInt(_highestLevelMemName, levelArg);
			_gameManagerRef.updateScoreBoard(levelArg);
		}
	}

	public int returnHighestLevel() {
		return PlayerPrefs.GetInt(_highestLevelMemName);
	}
}
