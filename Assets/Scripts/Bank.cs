using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] int startCoins;
    public static int coins { get; private set; }
    static int coinsDisplayed;
    [SerializeField] TextMeshProUGUI textCoins;

    public static event System.Action<int> OnCoinsChanged;

    private void Awake()
    {
        coins = startCoins;
        coinsDisplayed = coins;
        textCoins.text = coins.ToString();
        OnCoinsChanged += UpdateDisplayValue;
    }

    public static void AdjustCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
    }

    public void UpdateDisplayValue(int value)
    {
        LeanTween.value(coinsDisplayed, coins, .25f).setOnUpdate(UpdateDisplay);
        void UpdateDisplay(float value)
        {
            coinsDisplayed = (int)value;
            textCoins.text = coinsDisplayed.ToString();
        }
    }
    
}
