using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private AudioSource soundSource;
    [SerializeField]
    private AudioSource music1Source;
    [SerializeField]
    private AudioSource music2Source;
    [SerializeField]
    private string introMusic;
    [SerializeField]
    private string levelMusic;

    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;
    private float _crossFadeRate = 1.5f;
    private bool _crossFaading;

    private float _musicVolume;

    public float MusicVolume
    {
        get { return _musicVolume; }

        set
        {
            _musicVolume = value;

            if (music1Source != null && !_crossFaading)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }

    public bool MusicMute
    {
        get
        {
            if (music1Source != null)
            {
                return music1Source.mute;
            }

            return false;
        }

        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }

    public ManagerStatus Status { get; private set; }

    public void PlayIntroMusic()
    {
        PlayMusic(Resources.Load("Music/" + introMusic) as AudioClip);
    }

    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load("Music/" + levelMusic) as AudioClip);
    }

    public float SoundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    public void PlaySound(AudioClip clip)
    {
        this.soundSource.PlayOneShot(clip);
    }

    public bool SoundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    public void Startup(NetworkService service = null)
    {
        Debug.Log("Audio manager starting...");

        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;

        _activeMusic = music1Source;
        _inactiveMusic = music2Source;

        SoundVolume = 1f;
        MusicVolume = 1f;

        Status = ManagerStatus.Started;
    }

    public void StopMusic()
    {
        music1Source.Stop();
        music2Source.Stop();
    }

    private void PlayMusic(AudioClip clip)
    {
        if (_crossFaading)
        {
            return;
        }

        StartCoroutine(CrossFadeMusic(clip));
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _crossFaading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = _crossFadeRate * _musicVolume;

        while (_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;

            yield return null;
        }

        AudioSource tmp = _activeMusic;

        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;

        _inactiveMusic = tmp;
        _inactiveMusic.Stop();

        _crossFaading = false;
    }
}