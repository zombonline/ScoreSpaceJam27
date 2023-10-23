using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveStep
{
    [Range(0,2)]
    public int mice;
    [Range(0, 2)]
    public int birds;
    [Range(0, 2)]
    public int dogs;
    [Range(0, 2)]
    public int micelvl2;
    [Range(0, 2)]
    public int birdslvl2;
    [Range(0, 2)]
    public int dogslvl2;
}
[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]

public class SOWave : ScriptableObject
{
    public WaveStep[] waveSteps;
    public int[] beatsToSpawnOn;
    [TextArea(0, 3)]
    public string hint;
}
