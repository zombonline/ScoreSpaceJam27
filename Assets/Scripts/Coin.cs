using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{

    [SerializeField] int value;
    [SerializeField] bool destroyOnPickup;
    private void OnMouseDown()
    {
        FindObjectOfType<Bank>().AdjustCoins(value);
        if(destroyOnPickup) { Destroy(gameObject); }
    }
}
