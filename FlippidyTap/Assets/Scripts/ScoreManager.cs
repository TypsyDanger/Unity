using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	private int _theScore;                          
	private int _theHighScore;
	private int _consecutiveMatches;
	private int _baseScore;
	private GameObject _scoreCanvasObj;
	private string _highScoreMemName;
	private Text _scoreTextObj;
	private ParticleSystem _particleEmitter;
	private GameManager _gameManagerRef;
	private string _highestLevelMemName;


	public ScoreManager() { }

	public ScoreManager returnSelf() {
		return this;
	}

	private void initHighScore() {
		_theHighScore = PlayerPrefs.GetInt(_highScoreMemName);
	}

	public void setScoreIncrementAndMemNames(int baseScoreArg, string scoreMemNameArg, string highestLevelMemNameArg) {
		_baseScore = baseScoreArg;
		_highScoreMemName = scoreMemNameArg;
		_highestLevelMemName = highestLevelMemNameArg;
	}

	public void resetScoreAndUI() {
		_scoreTextObj = GameObject.Find("scoreText").GetComponent<Text>();
		_gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
		_particleEmitter = _scoreTextObj.gameObject.AddComponent<ParticleSystem>();
		_consecutiveMatches = 0;
		_theScore = getHighScore();
		_scoreTextObj.text = "" + _theScore;
	}

	public int getScore() {
		return _theScore;
	}

	public int getHighScore() {
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
		if (_consecutiveMatches > 0) {
			_theScore += _baseScore + (_baseScore * _consecutiveMatches);
		} else {
			_theScore += _baseScore;
		}

		_consecutiveMatches++;

		if (_theHighScore < _theScore) {
			_theHighScore = _theScore;
		}

		updateSavedHighScore();

		_scoreTextObj.text = "" + _theScore;

		return _theScore;
	}

	private void fireParticles() {
		if (_particleEmitter == null) {
			// _particleEmitter = _scoreTextObj.gameObject.AddComponent<ParticleSystem>();
		}

		_particleEmitter.Emit(10);
	}

	public int getConsecutiveMatches() {
		return _consecutiveMatches;
	}

	public void resetConsecutiveMatches() {
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
