using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODController : MonoBehaviour
{
    static FMOD.Studio.EventInstance snapshotHealthDown, snapshotReading;

    static BeatManager beatManager;

    List<FMOD.Studio.EventInstance> loopingInstances = new List<FMOD.Studio.EventInstance>();

    private void Awake()
    {
        snapshotHealthDown = RuntimeManager.CreateInstance("snapshot:/Health Down");
        snapshotReading = RuntimeManager.CreateInstance("snapshot:/Reading");

        beatManager = FindObjectOfType<BeatManager>();
    }

    public void ChangeMusicState(int value)
    {
        beatManager.instance.setParameterByName("Music State New" , value, false);
    }
    public static void ToggleMusicDetune()
    {
        beatManager.instance.getParameterByName("Music Detune", out float value);
        if (value == 0)
        {
            beatManager.instance.setParameterByName("Music Detune", 1, false);
        }
        else if (value == 1) 
        {
            beatManager.instance.setParameterByName("Music Detune", 0 , false);
        }
    }
    public static void PlaySFX(string val, string param = null, int paramVal = 0)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
        if(param == null) { return; }
        newAudioEvent.setParameterByName(param, paramVal);
    }
    public static void PlaySFXNoParams(string val)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
    }

    public void StartLoopedSFX(string val)
    {
        var audioEvent = RuntimeManager.CreateInstance(val);
        audioEvent.start();
        loopingInstances.Add(audioEvent);
    }

    public void StopLoopedSFX(string val)
    {
        foreach(var audioEvent in loopingInstances)
        {
            audioEvent.getDescription(out FMOD.Studio.EventDescription eventDesc);
            eventDesc.getPath(out string path);
            if (path == val)
            {
                audioEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                loopingInstances.Remove(audioEvent);
            }
        }
    }

    public static void PlaySnapshot()
    {
        snapshotHealthDown.start();
    }
    public static void StopSnapshot()
    {
        snapshotHealthDown.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public static void PlayReadingSnapshot()
    {
        snapshotReading.start();
    }
    public static void StopReadingSnapshot()
    {
        snapshotReading.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
