using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource audioMusic;
    public AudioSource audioSfx;

    public AudioClip sfxButtonClick;
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    public Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();

    [Header("AUDIO MIXER")]
    public AudioMixer audioMixer;
    //private string masterParameterName = "masterVolume";
    private string musicParameterName = "musicVolume";
    private string sfxParameterName = "sfxVolume";


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < musicClips.Length; i++)
        {
            if (musicClips[i] != null)
                musicDictionary.Add(musicClips[i].name, musicClips[i]);
        }

        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i] != null)
                sfxDictionary.Add(sfxClips[i].name, sfxClips[i]);
        }

        AdjustMusicVolume(ProfileManager.UserProfile.musicVolume);
        AdjustSoundVolume(ProfileManager.UserProfile.soundVolume);
    }

    public void PlaySfxClick()
    {
        audioSfx.PlayOneShot(sfxButtonClick, audioSfx.volume);
    }

    public void PlaySfx(string sfxName, float decibel = 0f)
    {
        if (sfxDictionary.ContainsKey(sfxName))
        {
            AudioClip clip = sfxDictionary[sfxName];
            PlaySfx(clip, decibel);
        }
        else
        {
            DebugCustom.LogError(string.Format("Sfx clip name does not exist in dictionary: {0}", sfxName));
        }
    }

    public void PlaySfx(AudioClip clip, float decibel = 0f)
    {
        if (clip)
        {
            audioMixer.SetFloat(sfxParameterName, decibel);
            audioSfx.PlayOneShot(clip, audioSfx.volume);
        }
        else
        {
            DebugCustom.Log("[PlaySfx] Clip NULL");
        }
    }

    public void StopSfx()
    {
        audioSfx.Stop();
    }

    public void StopMusic()
    {
        if (audioMusic.isPlaying)
        {
            audioMusic.Stop();
            audioMusic.clip = null;
        }
    }

    public void PlayMusic(string musicName, float decibel = 0f)
    {
        if (musicDictionary.ContainsKey(musicName))
        {
            if (IsPlayingMusic(musicName))
                return;

            audioMixer.SetFloat(musicParameterName, decibel);
            audioMusic.clip = musicDictionary[musicName];
            audioMusic.loop = true;
            audioMusic.Play();
        }
        else
        {
            DebugCustom.LogError("Music name not found=" + musicName);
        }
    }

    public void PlayMusicFromBeginning(string musicName, float decibel = 0f)
    {
        if (musicDictionary.ContainsKey(musicName))
        {
            audioMixer.SetFloat(musicParameterName, decibel);
            audioMusic.clip = musicDictionary[musicName];
            audioMusic.loop = true;
            audioMusic.Play();
        }
        else
        {
            DebugCustom.LogError("Music name not found=" + musicName);
        }
    }

    public bool IsPlayingMusic(string musicName)
    {
        if (audioMusic.clip != null)
        {
            if (string.Compare(audioMusic.clip.name, musicName) == 0)
            {
                return true;
            }
        }

        return false;
    }

    public AudioClip GetAudioClip(string soundName)
    {
        if (sfxDictionary.ContainsKey(soundName))
        {
            return sfxDictionary[soundName];
        }

        return null;
    }

    public void AdjustSoundVolume(float vol)
    {
        audioSfx.volume = vol;
    }

    public void AdjustMusicVolume(float vol)
    {
        audioMusic.volume = vol;
    }

    public void SetMute(bool isMute)
    {
#if !UNITY_EDITOR
        audioMusic.mute = isMute;
        audioSfx.mute = isMute;
#endif
    }
}
