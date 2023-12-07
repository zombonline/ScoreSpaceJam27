using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startCoins;
    public static int coins { get; private set; }
    int coinsDisplayed;
    [SerializeField] TextMeshProUGUI textCoins;

    private void Awake()
    {
        coins = startCoins;
        coinsDisplayed = coins;
        textCoins.text = coins.ToString();
        
    }

    public void AdjustCoins(int amount)
    {
        coins += amount;
        LeanTween.value(coinsDisplayed, coins, .25f).setOnUpdate(UpdateCoinDisplay);
    }

    public void UpdateCoinDisplay(float value)
    {
        coinsDisplayed= (int)value;
        textCoins.text = coinsDisplayed.ToString();
    }
    
}
