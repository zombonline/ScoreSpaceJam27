using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startCoins;
    public static int coins { get; private set; }
    [SerializeField] TextMeshProUGUI textCoins;


    private void Awake()
    {
        coins = startCoins;
        textCoins.text = coins.ToString("00000");
    }


    public void AdjustCoins(int amount)
    {
        coins += amount;
        textCoins.text = coins.ToString();
    }


}
