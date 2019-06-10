using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    public void OnSoundToggle()
    {
        ManagersProvider.Audio.SoundMute = !ManagersProvider.Audio.SoundMute;
        ManagersProvider.Audio.PlaySound(sound);
    }

    public void OnMusicToggle()
    {
        ManagersProvider.Audio.MusicMute = !ManagersProvider.Audio.MusicMute;
        ManagersProvider.Audio.PlaySound(sound);
    }

    public void OnSoundValue(float volume)
    {
        ManagersProvider.Audio.SoundVolume = volume;
    }

    public void OnMusicValue(float volume)
    {
        ManagersProvider.Audio.MusicVolume = volume;
    }

    public void OnPlayMusic(int selector)
    {
        ManagersProvider.Audio.PlaySound(sound);

        switch (selector)
        {
            case 1:
                ManagersProvider.Audio.PlayIntroMusic();
                break;
            case 2:
                ManagersProvider.Audio.PlayLevelMusic();
                break;
            default:
                ManagersProvider.Audio.StopMusic();
                break;
        }
    }
}