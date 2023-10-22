using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCache : MonoBehaviour
{
    [SerializeField] public SOAmmo ammo;

    int _maxAmmo;
    int maxAmmo
    {
        get { return _maxAmmo; }
        set
        {
            _maxAmmo = value;
            textMax.text = _maxAmmo.ToString();
        }
    }

    int _currentAmmo;
    int currentAmmo
    {
        get { return _currentAmmo; }
        set
        {
            _currentAmmo = value;
            textCurrent.text = _currentAmmo.ToString();
        }
    }

    int _refillCost;
    int refillCost
    {
        get { return _refillCost; }
        set
        {
            _refillCost = value;
            textRefillCost.text = _refillCost.ToString();
        }
    }

    [SerializeField] TextMeshProUGUI textCurrent, textMax, textRefillCost;
    [SerializeField] TextMeshProUGUI[] textUpgradeAmounts;
    [SerializeField] TextMeshProUGUI[] textUpgradeCosts;

    [SerializeField] RectTransform upgradePanel;

    private void Awake()
    {
        maxAmmo = ammo.initialMaxAmount;
        currentAmmo = maxAmmo;

        UpdateRefillCost();

        for(int i = 0; i< textUpgradeCosts.Length; i++)
        {
            textUpgradeCosts[i].text = ammo.upgrades[i].cost.ToString();
            textUpgradeAmounts[i].text = ammo.upgrades[i].amount.ToString();
        }
    }

    public void UpdateRefillCost()
    {
        refillCost = ammo.GetCostOfRefill();
    }

    public void UseAmmo()
    {
        if(currentAmmo <= 0)
        { 
            Debug.LogWarning("Ammo being used when empty.");
            return;
        }
        currentAmmo -= 1;
    }

    public void RefillAmmo()
    {
        if(Bank.coins >= refillCost)
        {
            FindObjectOfType<Bank>().AdjustCoins(-refillCost);
            currentAmmo = maxAmmo;
        }
    }
    public void UpgradeAmmo(int tier)
    {
        if(Bank.coins >= ammo.upgrades[tier].cost)
        {
            FindObjectOfType<Bank>().AdjustCoins(-ammo.upgrades[tier].cost);
            maxAmmo += ammo.upgrades[tier].amount;
        }
    }

    public int GetAmmoCount()
    {
        return currentAmmo;
    }

    public void OpenUpgradesPanel()
    {
        LeanTween.scaleX(upgradePanel.gameObject, 0, 1f);
    }
    public void CloseUpgradesPanel()
    {

    }
}
