using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ReactOnBeat : MonoBehaviour
{
    [SerializeField] int[] beatsToReactOn;
    [SerializeField] string[] markersToReactOn;

    [SerializeField] UnityEvent reactEvent;

    int currentBeat;
    private void Awake()
    {
        currentBeat = BeatManager.beat;
    }
    private void Update()
    {
        if(BeatManager.beat != currentBeat)
        {
            currentBeat = BeatManager.beat;
            NewBeat();
        }
    }

    public void NewBeat()
    {
        if (Base.gameOver) { return; }
        if (beatsToReactOn.Contains(BeatManager.beat) && markersToReactOn.Contains(BeatManager.marker))
        {
            reactEvent.Invoke();
        }
    }

    public void SetBeatsToReactOn(int[] newBeatsToReactOn)
    {
        beatsToReactOn = newBeatsToReactOn;
    }

}
