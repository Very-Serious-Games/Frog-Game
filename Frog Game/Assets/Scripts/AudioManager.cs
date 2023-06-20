using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else { 
           Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        PlayMusic("ambience");
    }

    public void PlayMusic(string name) { 
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null) {
            Debug.Log("Sound Not Found");
        } else { 
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.clip = sound.clip;
            sfxSource.Play();
        }
    }
    public void MuteMusic() { 
        musicSource.mute = !musicSource.mute;
    }
    public void MuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MUSICVolume(float volume)
    {
        Debug.Log("Music Volume: " + volume);
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
