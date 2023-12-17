using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider musicSlider, sfxSlider;
    FMOD.Studio.VCA sfx, music;

    private void Awake()
    {
        sfx = RuntimeManager.GetVCA("vca:/SFX VCA");
        music = RuntimeManager.GetVCA("vca:/Music VCA");
    }
    public void SetMusicVolume(float val)
    {
        music.setVolume(val);
    }
    public void SetSFXVolume(float val)
    {
        sfx.setVolume(val);
    }

}
