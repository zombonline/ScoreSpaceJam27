using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBank : MonoBehaviour
{
    int maxAmmoPistol, maxAmmoRifle, maxAmmoMortar;
    [SerializeField] int startAmmoPistol, startAmmoRifle, startAmmoMortar;
    [SerializeField] int currentAmmoPistol, currentAmmoRifle, currentAmmoMortar;

    public void RefillPistolAmmo()
    {
        currentAmmoPistol = maxAmmoPistol;
    }
    public void RefillRifleAmmo()
    {
        currentAmmoRifle = maxAmmoRifle;
    }
    public void RefillMortarAmmo()
    {
        currentAmmoMortar = maxAmmoMortar;
    }

    public void UpgradeMaxPistolAmmo(int percentage)
    {
        maxAmmoPistol = maxAmmoPistol + (startAmmoPistol * percentage);
    }
}
