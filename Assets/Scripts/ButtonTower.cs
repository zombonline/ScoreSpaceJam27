using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour
{
    PlacementManager placementManager;

    [SerializeField] SOTower soTower;
    [SerializeField] Image imgCard;
    [SerializeField] TextMeshProUGUI textCost;
    int costDisplayed;
    float costFontSize;
    [SerializeField] Button purchaseButton;

    bool mouseOver = false, selected = false;

    Vector2 initialPosition, hoveredPosition;
    RectTransform cardTransform;

    [SerializeField] RectTransform smallRect, largeRect;
    private void Awake()
    {
        placementManager = FindObjectOfType<PlacementManager>();
        cardTransform = imgCard.GetComponent<RectTransform>();

        costDisplayed = soTower.GetCostOfTower();
        textCost.text = costDisplayed.ToString();

        costFontSize = textCost.fontSize;

        initialPosition = cardTransform.anchoredPosition;
        hoveredPosition = new Vector2(cardTransform.anchoredPosition.x, cardTransform.anchoredPosition.y + (cardTransform.rect.height*.8f));        
    }

    private void Update()
    {
        purchaseButton.interactable = soTower.GetCostOfTower() <= Bank.coins;
        if(soTower.GetCostOfTower() <= Bank.coins)
        {
            imgCard.sprite = soTower.cardGlow;
        }
        else
        {
            imgCard.sprite = soTower.card;
        }
        CheckMousePosition();
    }



    public void Press()
    {
        if(soTower.GetCostOfTower() > Bank.coins) { return; } //Player does not have enough coins.
        placementManager.AssignTowerHeld(soTower, this);
        foreach(var towerButton in FindObjectsOfType<ButtonTower>())
        {
            towerButton.DisablePressedState();
        }
        EnablePressedState();
    }

    public void UpdateCost()
    {
        LeanTween.value(costDisplayed, soTower.GetCostOfTower(), .25f).setOnUpdate(UpdateValueDisplay);
    }
    public void UpdateValueDisplay(float value)
    {
        costDisplayed = (int)value;
        textCost.text = costDisplayed.ToString();
    }

    public void CheckMousePosition()
    {

        Vector2 localMousePosition = smallRect.InverseTransformPoint(Input.mousePosition);
        if (smallRect.rect.Contains(localMousePosition) && !mouseOver)
        {
            mouseOver= true;
            LeanTween.move(cardTransform, hoveredPosition, .5f).setEase(LeanTweenType.easeInBack);

        }
        localMousePosition = largeRect.InverseTransformPoint(Input.mousePosition);
        if (!largeRect.rect.Contains(localMousePosition) && mouseOver && !selected)
        {
            mouseOver= false;
            LeanTween.move(cardTransform, initialPosition, .5f).setEase(LeanTweenType.easeOutBounce);

        }
    }

    public void EnablePressedState()
    {
        selected = true;
    }

    public void DisablePressedState()
    {
        selected = false;
    }

}
