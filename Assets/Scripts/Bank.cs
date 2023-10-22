using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        textCoins.text = coins.ToString("00000");
        
    }

    public void AdjustCoins(int amount)
    {
        coins += amount;
        StartCoroutine(UpdateCoinsRoutine(coinsDisplayed, coins));
    }
    private IEnumerator UpdateCoinsRoutine(int valueToUpdate, int newValue)
    {
        var diffBetweenCost = Mathf.Abs(newValue - valueToUpdate);
        Debug.Log(diffBetweenCost);
        for (int i = 0; i < diffBetweenCost; i++)
        {
            if (valueToUpdate < newValue) { valueToUpdate++; }
            else if (valueToUpdate > newValue) { valueToUpdate--; }
            textCoins.text = valueToUpdate.ToString();
            yield return new WaitForSeconds(.5f / diffBetweenCost);
        }
        coinsDisplayed = valueToUpdate;
    }

}
