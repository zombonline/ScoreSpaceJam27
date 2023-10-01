using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] RectTransform healthBar;

    [SerializeField] int startHitPoints;
    int hitPoints;

    [SerializeField] UnityEvent onGameOver;

    private void Awake()
    {
        hitPoints = startHitPoints;
    }

    public void UpdateHealthBar()
    {
        for (int i = healthBar.childCount; i > 0; i--)
        {
            if (i > hitPoints)
            {
                healthBar.GetChild(i - 1).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            AdjustHitPoints(-1);
        }
    }

    public void AdjustHitPoints(int adjustment)
    {
        hitPoints += adjustment;
        UpdateHealthBar();
        if (hitPoints <= 0)
        {
            onGameOver.Invoke();
        }
    }


}
