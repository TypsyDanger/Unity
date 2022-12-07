using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
	private AudioClip _swishClipOne;
	private AudioClip _swishClipTwo;
    private AudioClip _misMatchClip;
    private AudioClip _flippedClip;
	private AudioClip _flippedTwoClip;
    private AudioClip _matchClip;
    private AudioClip _roundWinClip;
    private AudioClip _cardAppearClip;
    private AudioClip _cardDestroyClip;
    private AudioClip _clockThrobClip;
    private AudioClip _timeAwardClip;
    private AudioClip _streakEndClip;
	private AudioClip _gameOverClip;
	private AudioClip _coinClip;
	private AudioClip _coinClipTwo;
	private AudioClip _buyFail;
	private AudioClip _winClipOne;
	private AudioClip _winClipTwo;
	private AudioClip _winClipThree;
	private AudioClip _winClipFour;
	private AudioClip _winClipFive;
	private AudioClip _winkClip;
	private AudioClip _uiSelect;
	private AudioClip _purchaseClip;
    private bool _muted;

	void Start() {
		
	}

    void Awake() {
        //print("AudioManager started");
		_audioSource = GetComponent<AudioSource>();
		_swishClipOne = Resources.Load<AudioClip>("SoundEffects/whip-whoosh-swoosh-1");
		_swishClipTwo = Resources.Load<AudioClip>("SoundEffects/whip-whoosh-swoosh-2");
        _misMatchClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_impact1");
        _flippedClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_button11");
		_flippedTwoClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_interaction1");
        _matchClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_fanfare3");
        _cardAppearClip = Resources.Load<AudioClip>("SoundEffects/sfx_menu_move1");
        _cardDestroyClip = Resources.Load<AudioClip>("SoundEffects/sfx_menu_move1b_lowerPitch");
        _clockThrobClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_damage1");
        _timeAwardClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_powerup16");
        _streakEndClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_powerup16_reversed");
		_gameOverClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_negative1");
		_coinClip = Resources.Load<AudioClip>("SoundEffects/sfx_coin_double3");
        _coinClipTwo = Resources.Load<AudioClip>("SoundEffects/sfx_coin_double4");
		_buyFail = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_error3");
		_winClipOne = Resources.Load<AudioClip>("SoundEffects/winFanfare1");
		_winClipTwo = Resources.Load<AudioClip>("SoundEffects/winFanfare2");
		_winClipThree = Resources.Load<AudioClip>("SoundEffects/winFanfare3");
		_winClipFour = Resources.Load<AudioClip>("SoundEffects/winFanfare4");
		_winClipFive = Resources.Load<AudioClip>("SoundEffects/winFanfare5");
		_winkClip = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_button1b");
		_uiSelect = Resources.Load<AudioClip>("SoundEffects/sfx_sounds_interaction19");
		_purchaseClip = Resources.Load<AudioClip>("SoundEffects/purchaseSound1");

        _muted = false;
        
    }

    public void playSound(string soundToPlay) {
        //print("playtest");
        if (!_muted){
			switch (soundToPlay){
				case "play_flipped":
					_audioSource.PlayOneShot(_flippedClip, 0.5f);
					break;
				case "play_swish_1":
					_audioSource.PlayOneShot(_swishClipOne, 0.5f);
					break;
                case "play_swish_2":
					_audioSource.PlayOneShot(_swishClipTwo, 0.75f);
					break;
				case "play_flipped_two":
					_audioSource.PlayOneShot(_flippedTwoClip, 0.75f);
					break;
                case "play_mismatch":
                    _audioSource.PlayOneShot(_misMatchClip, 0.75f);
                    break;
                case "play_match":
                    _audioSource.PlayOneShot(_matchClip, 1.25f);
                    break;
                case "play_cardAppear":
                    _audioSource.PlayOneShot(_cardAppearClip, 0.5f);
                    break;
                case "play_cardDestroy":
                    _audioSource.PlayOneShot(_cardDestroyClip, 0.5f);
                    break;
                case "play_roundWin":
                    _audioSource.PlayOneShot(_roundWinClip, 0.5f);
                    break;
                case "play_clockThrob":
                    _audioSource.PlayOneShot(_clockThrobClip, 1.0f);
                    break;
                case "play_timeAward":
                    _audioSource.PlayOneShot(_timeAwardClip, .75f);
                    break;
                case "play_streakEnd":
                    _audioSource.PlayOneShot(_streakEndClip, 1f);
                    break;
                case "play_gameOver":
                    _audioSource.PlayOneShot(_gameOverClip, 1f);
					break;
				case "play_coinGet":
					_audioSource.PlayOneShot(_coinClip, .7f);
					break;
				case "play_coinGet_2":
					_audioSource.PlayOneShot(_coinClipTwo, .5f);
					break;
				case "play_win_1":
					_audioSource.PlayOneShot(_winClipOne, 1f);
					break;
				case "play_win_2":
					_audioSource.PlayOneShot(_winClipTwo, 1f);
					break;
				case "play_win_3":
					_audioSource.PlayOneShot(_winClipThree, 1f);
					break;
				case "play_win_4":
					_audioSource.PlayOneShot(_winClipFour, 1f);
					break;
				case "play_win_5":
					_audioSource.PlayOneShot(_winClipFive, 1f);
					break;
				case "play_wink":
					_audioSource.PlayOneShot(_winkClip, .4f);
					break;
				case "play_buyFail":
					_audioSource.PlayOneShot(_buyFail, 1f);
					break;
				case "play_uiSelect":
					_audioSource.PlayOneShot(_uiSelect, 1f);
					break;
				case "play_purchase_1":
					_audioSource.PlayOneShot(_purchaseClip, 1f);
					break;
                default: break;
            }
        }
    }

    public void stopAudio() {
        _audioSource.Stop();
    }

    public bool toggleMute() {
        
        if(_muted) {
            _muted = false;
        } else {
            _muted = true;
        }


        return _muted;
    }

    public void setMute() {
        _muted = true;
    }

	// Update is called once per frame
	void Update() {
			
	}

    public bool returnMuted() {
        return _muted;
    }
}
