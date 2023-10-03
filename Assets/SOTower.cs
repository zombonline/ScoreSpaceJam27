using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class SOTower : ScriptableObject
{
    public int buildCost;
    public int battleCost;
    public GameObject towerPrefab;
    public Sprite card;

    public int GetCostOfTower()
    {
        if (FindObjectOfType<WaveSystem>().gameMode == GameMode.Battle)
        {

            return battleCost;
        }
        else
        {

            return buildCost;
        }
    }
}
