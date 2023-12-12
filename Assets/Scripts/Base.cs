using Spine.Unity;
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

    public static bool gameOver = false;

    private void Awake()
    {
        gameOver = false;
        hitPoints = startHitPoints;
    }

    public void UpdateHealthBar()
    {
        for (int i = healthBar.childCount; i > 0; i--)
        {
            if (i > hitPoints)
            {
                var skeletonGrahic = healthBar.GetChild(i - 1).GetComponent<SkeletonGraphic>();
                skeletonGrahic.Skeleton.SetSkin("Heart Dead");
                skeletonGrahic.Skeleton.SetSlotsToSetupPose();
                skeletonGrahic.LateUpdate();

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
            gameOver = true;
            onGameOver.Invoke();
        }
    }


}
