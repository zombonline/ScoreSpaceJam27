using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODController : MonoBehaviour
{

    public void ChangMenuToBuilding( int value)
    {
        FindObjectOfType<BeatManager>().instance.setParameterByName("Menu to Building", value, false);
    }
    public void ChangeMusicState(int value)
    {
        FindObjectOfType<BeatManager>().instance.setParameterByName("Music State" , value, false);
    }
    public void TransitionToBuild()
    {
        //wave has ended, music should transition to a calm version of the song
        //marker will b esomething like build which means enemies are not spawning or moving on each beat.
    }

}
