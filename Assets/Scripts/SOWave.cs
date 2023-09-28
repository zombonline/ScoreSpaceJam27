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
}
[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]

public class SOWave : ScriptableObject
{
    public WaveStep[] waveSteps;
    public int[] beatsToSpawnOn;

}
