using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODController : MonoBehaviour
{
    static FMOD.Studio.EventInstance snapshotHealthDown, snapshotReading;

    private void Awake()
    {
        snapshotHealthDown = RuntimeManager.CreateInstance("snapshot:/Health Down");
        snapshotReading = RuntimeManager.CreateInstance("snapshot:/Reading");
    }
    public void ChangeMusicState(int value)
    {
        FindObjectOfType<BeatManager>().instance.setParameterByName("Music State New" , value, false);
    }
    public static void PlaySFX(string val)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
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
