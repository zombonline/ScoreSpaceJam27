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
    [SerializeField] SpriteRenderer spriteBase;
    [SerializeField] float lengthSpriteFlashSeconds = .5f, lengthBetweenFlashesSeconds = .1f;

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

    private IEnumerator FlashSpriteRoutine()
    {
        for (float i = 0; i < lengthSpriteFlashSeconds; i += lengthBetweenFlashesSeconds)
        {
            spriteBase.color = Color.red;
            yield return new WaitForSeconds(lengthBetweenFlashesSeconds);
            spriteBase.color = Color.white;
        }
        
    }

    public void AdjustHitPoints(int adjustment)
    {
        hitPoints += adjustment;
        UpdateHealthBar();
        if(adjustment < 0) { StartCoroutine(FlashSpriteRoutine()); } //if damage taken, flash sprite
        if (hitPoints <= 0)
        {
            onGameOver.Invoke();
        }
    }


}
