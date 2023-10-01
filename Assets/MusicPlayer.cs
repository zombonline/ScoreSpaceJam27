using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;
    private BeatManager bS;

    public void Start()
    {
        instance = GetComponent<StudioEventEmitter>().EventInstance;
        bS = GetComponent<BeatManager>();
        bS.AssignBeatEvent(instance);
    }

}