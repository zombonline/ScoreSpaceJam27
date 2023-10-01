using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour
{
    PlacementManager placementManager;

    [SerializeField] SOTower soTower;
    [SerializeField] Image imgCard;
    [SerializeField] TextMeshProUGUI textCost;

    private void Awake()
    {
        placementManager = FindObjectOfType<PlacementManager>();

        imgCard.sprite = soTower.card;
        textCost.text = soTower.buildCost.ToString();
    }

    public void Press()
    {
        if(soTower.GetCostOfTower() > Bank.coins) { UnableToPurchase(); return; } //Player does not have enough coins.
        placementManager.AssignTowerHeld(soTower, this);
    }

    public void UpdateCost()
    {
        textCost.text = soTower.GetCostOfTower().ToString(); // put this on wave start and wave end.
    }

    private void UnableToPurchase()
    {

    }

    public void EnablePressedState()
    {

    }

    public void DisablePressedState()
    {

    }


}
