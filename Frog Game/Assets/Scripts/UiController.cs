using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
   public Slider _musicSlider, _sfxSlider;

    public void ToggleMusic(){
        AudioManager.instance.MuteMusic();
    }

    public void ToggleSfx(){
        AudioManager.instance.MuteSFX();
    }

    public void MusicVolume(){
        
        Debug.Log("Music Volume: " + _musicSlider.value);
        AudioManager.instance.MUSICVolume(_musicSlider.value);
    }

    public void SfxVolume(){
        AudioManager.instance.SFXVolume(_sfxSlider.value);
    }
}
