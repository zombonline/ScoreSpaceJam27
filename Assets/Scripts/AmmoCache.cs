using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class AmmoCache : MonoBehaviour
{
    [SerializeField] public SOAmmo ammo;
    [SerializeField] string refillSFX;
    [SerializeField] string[] upgradeSFX;
    [SerializeField] string ammoDrawerOpenSFX, ammoDrawerCloseSFX;

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

    [SerializeField] RectTransform smallRect, largeRect;

    [SerializeField] UnityEvent mouseEnter, mouseExit;
    bool mouseOver = false;

    private void Awake()
    {
        maxAmmo = ammo.initialMaxAmount;
        currentAmmo = maxAmmo;

        UpdateRefillCost();

        for(int i = 0; i< textUpgradeCosts.Length; i++)
        {
            textUpgradeCosts[i].text = ammo.upgrades[i].cost.ToString();
            textUpgradeAmounts[i].text = "Upgrade \n +" + ammo.upgrades[i].amount.ToString();
        }
    }
    
    public void UpdateRefillCost()
    {
        refillCost = ammo.GetCostOfRefill();
    }

    private void Update()
    {
        CheckMousePosition();
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
        if(currentAmmo == maxAmmo) { return; }
        if(Bank.coins >= refillCost)
        {
            Bank.AdjustCoins(-refillCost);
            FMODController.PlaySFX(refillSFX);
            currentAmmo = maxAmmo;
        }
    }
    public void UpgradeAmmo(int tier)
    {
        if(Bank.coins >= ammo.upgrades[tier].cost)
        {
            Bank.AdjustCoins(-ammo.upgrades[tier].cost);
            maxAmmo += ammo.upgrades[tier].amount;
            FMODController.PlaySFX(upgradeSFX[tier]);
        }
    }

    public int GetAmmoCount()
    {
        return currentAmmo;
    }

    public void CheckMousePosition()
    {
        Vector2 localMousePosition = smallRect.InverseTransformPoint(Input.mousePosition);
        if (smallRect.rect.Contains(localMousePosition) && !mouseOver)
        {
            mouseOver = true;
            mouseEnter.Invoke();
            FMODController.PlaySFX(ammoDrawerOpenSFX);
        }
        else if (!largeRect.rect.Contains(localMousePosition) && mouseOver)
        {
            mouseOver = false;
            mouseExit.Invoke();
            FMODController.PlaySFX(ammoDrawerCloseSFX);
        }
    }

    
}
