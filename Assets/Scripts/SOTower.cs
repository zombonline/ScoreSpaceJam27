using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class SOTower : ScriptableObject
{
    public int buildCost;
    public int battleCost;
    public int refundCost;
    public GameObject towerPrefab;
    public Sprite card,cardGlow;

    public int GetCostOfTower()
    {
        if (WaveSystem.gameMode == GameMode.Battle)
        {

            return battleCost;
        }
        else
        {

            return buildCost;
        }
    }
}
