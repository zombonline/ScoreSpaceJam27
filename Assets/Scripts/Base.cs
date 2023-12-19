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
    [SerializeField] float lengthSpriteFlashSeconds = .5f, lengthBetweenFlashesSeconds = .1f;
    [SerializeField] string baseDamageSFX;
    [SerializeField] float snapShotDuration = .5f;

    public static bool gameOver = false;

    int hitPointsLastBeat = 0;
    [SerializeField] SkeletonAnimation spineObject;
    [SerializeField] string[] skins;

    private void Awake()
    {
        gameOver = false;
        hitPoints = startHitPoints;
        hitPointsLastBeat = hitPoints;
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

    public void AdjustHitPoints(int adjustment)
    {
        hitPoints += adjustment;
        UpdateHealthBar();
        StartCoroutine(DamageSFXRoutine());
        SetSpineSkin();
        GetComponent<SpineAnimator>().SetAnimation("Damage");
        if (hitPoints <= 0)
        {
            gameOver = true;
            onGameOver.Invoke();
        }
    }

    public void SetSpineSkin()
    {
        switch(hitPoints)
        {
            case 0:
                spineObject.skeleton.SetSkin(skins[0]);
                break;
            case (<3):
                spineObject.skeleton.SetSkin(skins[1]);
                break;
            case (<5):
                spineObject.skeleton.SetSkin(skins[2]);
                break;
            case (<7):
                spineObject.skeleton.SetSkin(skins[3]);
                break;
            case (<9):
                spineObject.skeleton.SetSkin(skins[4]);
                break;
            case (< 11):
                spineObject.skeleton.SetSkin(skins[5]);
                break;
        }
    }

    IEnumerator DamageSFXRoutine()
    {
        FMODController.PlaySnapshot();
        FMODController.PlaySFX(baseDamageSFX);
        yield return new WaitForSeconds(snapShotDuration);
        FMODController.StopSnapshot();
    }

    public void CheckIfDamaged()
    {
        if(hitPoints != hitPointsLastBeat)
        {
            FMODController.ToggleMusicDetune();
        }
        hitPointsLastBeat = hitPoints;
    }
}
