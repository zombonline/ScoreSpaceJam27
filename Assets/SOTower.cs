using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class SOTower : ScriptableObject
{
    public int cost;
    public GameObject towerPrefab;
    public Sprite card;
}
