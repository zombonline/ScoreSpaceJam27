using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Upgrade
{
    public int cost;
    public int amount;
}


[CreateAssetMenu(fileName = "New Ammo", menuName = "Ammo")]
public class SOAmmo : ScriptableObject
{
    public int initialMaxAmount;
    public int buildRefillCost;
    public int battleRefillCost;
    public Bullet bulletPrefab;
    public Sprite bulletIcon;
    public Upgrade[] upgrades;

    public int GetCostOfRefill()
    {
        if (WaveSystem.gameMode == GameMode.Battle)
        {

            return battleRefillCost;
        }
        else
        {

            return buildRefillCost;
        }
    }
}
