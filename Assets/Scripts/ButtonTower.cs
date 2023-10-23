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
    int currentCost;
    float costFontSize;
    bool selected;

    bool coroutineRunning = false;
    private void Awake()
    {
        placementManager = FindObjectOfType<PlacementManager>();

        imgCard.sprite = soTower.card;
        currentCost = soTower.GetCostOfTower();
        textCost.text = currentCost.ToString();

        costFontSize = textCost.fontSize;
    }

    public void Press()
    {
        if(soTower.GetCostOfTower() > Bank.coins) { UnableToPurchase(); return; } //Player does not have enough coins.
        placementManager.AssignTowerHeld(soTower, this);
        EnablePressedState();
    }

    public void UpdateCost()
    {
        StartCoroutine(UpdateCostRoutine());
    }

    private IEnumerator UpdateCostRoutine()
    {
        var diffBetweenCost = Mathf.Abs(soTower.GetCostOfTower() - currentCost);
        Debug.Log(diffBetweenCost);
        StartCoroutine(EnlargeCostTextRoutine());
        for (int i = 0; i < diffBetweenCost; i++)
        {
            if (currentCost < soTower.GetCostOfTower()) { currentCost++; }
            else if (currentCost > soTower.GetCostOfTower()) { currentCost--; }
            textCost.text = currentCost.ToString();
            yield return new WaitForSeconds(.5f / diffBetweenCost);
        }

    }

   

    IEnumerator Animate()
    {
        coroutineRunning= true;
        while (selected)
        {
            var counter = 0;
            while (counter < 150f)
            {
                counter++;
                GetComponent<RectTransform>().eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - .02f);
                yield return new WaitForSeconds(0.001f);
            }
            counter = 0;
            while (counter < 150f)
            {
                counter++;
                GetComponent<RectTransform>().eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + .02f);
                yield return new WaitForSeconds(0.001f);
            }
            counter = 0;
            while (counter < 150f)
            {
                counter++;
                GetComponent<RectTransform>().eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + .02f);
                yield return new WaitForSeconds(0.001f);
            }
            counter = 0;
            while (counter < 150f)
            {
                counter++;
                GetComponent<RectTransform>().eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - .02f);
                yield return new WaitForSeconds(0.001f);
            }
        }
        coroutineRunning = false;

    }
    private IEnumerator EnlargeCostTextRoutine()
    {
        var diffBetweenSizes = Mathf.Abs(textCost.fontSize - costFontSize *1.2f);
        while (textCost.fontSize < costFontSize * 1.2f)
        {
            textCost.fontSize++;
            yield return new WaitForSeconds((.5f / diffBetweenSizes)/2f);
        }
        while (textCost.fontSize > costFontSize)
        {
            textCost.fontSize--;
            yield return new WaitForSeconds((.5f / diffBetweenSizes) / 2f);
        }
    }

    private void UnableToPurchase()
    {

    }

    public void EnablePressedState()
    {
        selected = true;

        if (!coroutineRunning)
        {
            StartCoroutine(Animate());
        }
    }

    public void DisablePressedState()
    {
        selected = false;
    }


}
