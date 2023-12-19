using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider musicSlider, sfxSlider;
    FMOD.Studio.VCA sfx, music;

    public const string SFX_VOL_KEY = "sfx volume", MUSIC_VOL_KEY = "music volume";

    private void Awake()
    {
        sfx = RuntimeManager.GetVCA("vca:/SFX VCA");
        music = RuntimeManager.GetVCA("vca:/Music VCA");

        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 1);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOL_KEY, 1);
    }
    public void SetMusicVolume(float val)
    {
        music.setVolume(val);
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, val);
    }
    public void SetSFXVolume(float val)
    {
        sfx.setVolume(val);
        PlayerPrefs.SetFloat(SFX_VOL_KEY, val);
    }



}
