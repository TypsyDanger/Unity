using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private AudioSource _musicSource;
    private AudioClip _mainMusic;
    private AudioClip _gamePlaymusic;
    private bool _muted;
    
    void Awake() {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _muted = false;
        _musicSource.volume = 1f;
    }

    public void playMusic(string trackArg, bool isLoopedArg) {
        //print("music Source: "+_musicSource);
        if(checkMusicSource()) {
            _musicSource.loop = isLoopedArg;
            switch(trackArg) {
                case "play_musicMain":
                    _musicSource.clip = _mainMusic;
                    _musicSource.Play();
                    break;
                case "play_musicGameplay":
                    _musicSource.clip = _gamePlaymusic;
                    _musicSource.Play();
                    break;
                default:
                    break;
            }
        }

    }

    public bool checkMusicSource() {
        if(_musicSource == null) {
            //print("music source was null");
            _musicSource = gameObject.AddComponent<AudioSource>();
            return true;
        } else {
            //print("music source was not null");
            return true;
        }
    }

    public void stopAudio() {
        _musicSource.Stop();
    }

    public void setMusic(AudioClip mainMusicArg, AudioClip gameplayMusicArg) {
        _mainMusic = mainMusicArg;
        _gamePlaymusic = gameplayMusicArg;
    }

    public bool toggleMute() {
        if(!_muted) {
            _muted = true;;
            _musicSource.volume = 0f;
        } else {
            _muted = false;
            _musicSource.volume = 1f;
        }

        return _muted;
    }

    public void setMute() {
        _muted = true;
        _musicSource.volume = 0f;
    }

    public bool returnMuted() {
        return _muted;
    }

	public IEnumerator dropWinVolume(float delay) {
		if(!_muted) {
			_musicSource.volume = 0.75f;
			yield return new WaitForSeconds(delay);
			_musicSource.volume = 1f;
		}
	}

}
