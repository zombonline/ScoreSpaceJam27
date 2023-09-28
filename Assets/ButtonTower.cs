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
        textCost.text = soTower.cost.ToString();
    }

    public void Press()
    {
        placementManager.AssignTowerHeld(soTower.towerPrefab, this);
    }

    public void EnablePressedState()
    {

    }

    public void DisablePressedState()
    {

    }


}
