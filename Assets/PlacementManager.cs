using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public  class PlacementManager : MonoBehaviour 
{
    bool canPlace = true;
    public SOTower towerHeld { get; private set; }

    ButtonTower towerHeldButton;
    public SOTower tempTower { get; private set; }   

    private MapTile lastSelectedTile;

    [SerializeField] Button buttonSellTower;

    public void Place(MapTile tile)
    {
        if (!canPlace) { return; }
        if (towerHeld == null) { return; }
        tile.ReceiveTower(towerHeld.towerPrefab);
        tile.SetRefundValue(towerHeld.refundCost);
        FindObjectOfType<Bank>().AdjustCoins(-towerHeld.GetCostOfTower());

        //if the player now has less than cost of currently held tower
        ClearTowerHeld();
    }

    public void ClearTowerHeld()
    {
        tempTower = null;
        towerHeld = null;
        towerHeldButton.DisablePressedState();
    }

    public void AssignTowerHeld(SOTower tower, ButtonTower buttonTower)
    {
        if(!canPlace) { return; }
        //check if player can afford
        towerHeld = tower;
        towerHeldButton = buttonTower;
        towerHeldButton.EnablePressedState();
        tempTower = tower;
    }


    public void ToggleCanPlace(bool val)
    {
        canPlace = val;
    }

    public void SetLastSelectedTile(MapTile tile)
    {
        lastSelectedTile = tile;
        if(lastSelectedTile.placedTower != null) //if tower on last selected tile
        {
            buttonSellTower.gameObject.SetActive(true);
            buttonSellTower.GetComponentInChildren<TextMeshProUGUI>().text = "Refund " + lastSelectedTile.placedTowerRefundValue.ToString();
        }
    }
    public void SellTowerOnLastSelectedTile()
    {
        lastSelectedTile.RemoveTower();
        FindObjectOfType<Bank>().AdjustCoins(lastSelectedTile.placedTowerRefundValue);
        Destroy(lastSelectedTile.placedTower);
        buttonSellTower.gameObject.SetActive(false); //enable sell tower button if tile selected has a tower.
    }


}
