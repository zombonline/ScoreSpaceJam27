using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    static FMOD.Studio.Bus musicBus;
    static FMOD.Studio.Bus sfxBus;
    public static string MUSIC_VOLUME_KEY = "music_volume_key";
    public static string SFX_VOLUME_KEY = "sfx_volume_key";
    static string musicBusPath = "", sfxBusPath = "";

    static float musicBusVol, sfxBusVol;
    
    private static void Awake()
    {
        musicBus = FMODUnity.RuntimeManager.GetBus(musicBusPath);
        sfxBus = FMODUnity.RuntimeManager.GetBus(sfxBusPath);
        musicBus.getVolume(out float volume);
        musicBusVol = volume;


    }

    public static void AssignInitialBusVolume(FMOD.Studio.Bus bus)
    {
        bus.getVolume(out float volume);

    }

    public static void SetMusicVolume(float newVol)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, newVol);
    }
    public static void SetSFXVolume(float newVol)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, newVol);
    }


}
