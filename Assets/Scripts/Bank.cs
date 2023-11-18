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
        textCoins.text = coins.ToString("00000");
        
    }

    public void AdjustCoins(int amount)
    {
        coins += amount;
        StartCoroutine(UpdateCoinsRoutine(coinsDisplayed, coins));
    }
    private IEnumerator UpdateCoinsRoutine(int valueToUpdate, int newValue)
    {
        int diffBetweenCost = Mathf.Abs(newValue - valueToUpdate);
        while(valueToUpdate != newValue)
        { 
            if (valueToUpdate < newValue)
            {
                valueToUpdate += diffBetweenCost/10 ;
                if(valueToUpdate > newValue) { valueToUpdate = newValue; }
            }
            else if (valueToUpdate > newValue)
            {
                valueToUpdate -= diffBetweenCost/10;
                if (valueToUpdate < newValue) { valueToUpdate = newValue; }
            }
            textCoins.text = valueToUpdate.ToString();
            yield return new WaitForSeconds(.05f);
        }
        coinsDisplayed = newValue;
    }

}
