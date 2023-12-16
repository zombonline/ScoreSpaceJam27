using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODController : MonoBehaviour
{

    public void ChangeMusicState(int value)
    {
        FindObjectOfType<BeatManager>().instance.setParameterByName("Music State New" , value, false);
    }
    public static void PlaySFX(string val)
    {
        var newAudioEvent = RuntimeManager.CreateInstance(val);
        newAudioEvent.start();
    }
}
