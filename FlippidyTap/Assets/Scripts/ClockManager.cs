using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour {

	private Text _clockText;
	private int _currentTime;
	private int _startingTime;
	private float _tickInterval;
	private ClockManager _instance;
	private bool _clockRunning;
    private GameObject _scorePopPrefab;
    private GameObject _scorePopParticlePrefab;
	private GameManager _gameManagerRef;
    private IEnumerator _coroutine;
    private string _clockPressureColor;
    private bool _justAddedTime;
    private ParticleSystem _clockSparkleParticle;
    private ParticleSystem _clockStreakOverParticle;
    private Animator _clockImageAnimator;
    private Animator _timerAnimator;

	void Start () {
        _clockText = GetComponentInParent<Text>();
		_startingTime = 60;
		_currentTime = _startingTime;
        _tickInterval = 1.0f;
        _gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(_gameManagerRef.returnGameMode() == 0) {
            _clockText.text = "" + _currentTime;
        } else {
            _clockText.text = "∞";
        }

        _clockPressureColor = "wh";
        _timerAnimator = this.gameObject.GetComponent<Animator>();
        _clockImageAnimator = GameObject.Find("clockImage").GetComponent<Animator>();
        _scorePopPrefab = Resources.Load("Prefabs/ScorePop") as GameObject;
        _scorePopParticlePrefab = Resources.Load("Prefabs/ScorePopParticle") as GameObject;
        _clockSparkleParticle = GameObject.Find("ClockParticle").GetComponent<ParticleSystem>();
        _clockStreakOverParticle = GameObject.Find("ClockParticle_streakOver").GetComponent<ParticleSystem>();
	}

	private IEnumerator _clockTick() {
        //print("_clockTick"); 
		yield return new WaitForSeconds (_tickInterval);

        if (_clockRunning) {
            if (_currentTime > 0)
            {
                _currentTime -= 1;

                // Colors!
                colorAndThrobCheck();
                _clockText.text = "" + _currentTime;

                StartCoroutine(_clockTick());
            }
            else
            {
                //print("clocks done, yo");
                _gameManagerRef.clockExpired();
            }
        } else {
            // Clocks paused
        }
	}

    private void colorAndThrobCheck() {
        if (_currentTime < 10)
        {
            throbClock();
        }
        else if (_currentTime < 20)
        {
            // color red
            _clockText.color = new Color(1f, 0f, 0f, 1f);
            _clockPressureColor = "re";
        }
        else if (_currentTime < 40)
        {
            // color yellow
            _clockText.color = new Color(1f, 0.92f, 0.016f, 1f);
            _clockPressureColor = "ye";
        }
        else if (_currentTime > 40)
        {
            // color white
            _clockText.color = new Color(1f, 1f, 1f, 1f);
            _clockPressureColor = "wh";
            //print(_clockText.color);
        }
    }

	public void startClockAfterDelay(float delay) {
        /* Called initially by GameManager */
        Invoke("startClock", delay);
	}

    private void startClock() {
        _clockRunning = true;

        StartCoroutine(_clockTick());
    }


    public void triggerBonus(int timeArg) {
        _currentTime = int.Parse(_clockText.text) + timeArg;
		_clockText.text = "" + _currentTime;
        showClockBonusPop();
        checkBonusModeAndStart();
        colorAndThrobCheck();
        _gameManagerRef.playSound("play_timeAward");
    }

    private void showClockBonusPop() {
        // Show the score pop text
        GameObject tempObj = GameObject.Instantiate(_scorePopPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        tempObj.transform.SetParent(GameObject.Find("ScoreCanvas").transform);

        tempObj.transform.position = new Vector3(gameObject.transform.position.x + 75f, gameObject.transform.position.y, gameObject.transform.position.z);
        tempObj.GetComponent<ScorePopManager>().playSelfAndDestroy();

        // Show the score pop particle effect
        GameObject tempParticleObj = GameObject.Instantiate(_scorePopParticlePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        tempParticleObj.transform.SetParent(GameObject.Find("WorldCanvas").transform);

        tempParticleObj.transform.position = new Vector3(gameObject.transform.position.x + 72f, gameObject.transform.position.y, 1);
        tempParticleObj.transform.localScale = new Vector3(1f, 1f, 1f);

        Destroy(tempParticleObj, 0.5f);
        //tempParticleObj.GetComponent<ScorePopManager>().playSelfAndDestroy();

    }

	public void pauseClockTick() {
        _clockRunning = false;
	}

    private void throbClock() {
        _timerAnimator.Play("clockThrob");
        _gameManagerRef.playSound("play_clockThrob");
    }

    public void checkBonusModeAndStart() {
        if(!_clockSparkleParticle.isPlaying) {
            _clockImageAnimator.Play("clockBonusThrob");
            _clockSparkleParticle.Play();
        }
    }

    public void checkBonusModeAndStop() {
        //print("checkBonusModeAndStop");
        if (_clockSparkleParticle.isPlaying) {
            //print("isPlaying, so stopping");
            _clockStreakOverParticle.Play();
            //_gameManagerRef.playSound("play_streakEnd");
            _clockImageAnimator.Play("clockStopBonus");
            _clockSparkleParticle.Stop();
        }
    }

    public void pause() {
        pauseClockTick();
        _clockImageAnimator.enabled = false;
        _clockSparkleParticle.Pause(true);
    }

    public void unPause() {
        startClock();
        _clockImageAnimator.enabled = true;
        _clockSparkleParticle.Play(true);
    }

	public int returnTime() {
		return _currentTime;
	}
}
