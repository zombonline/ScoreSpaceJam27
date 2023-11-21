using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeGraphicManager : MonoBehaviour
{

    SkeletonGraphic skeletonGraphic;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }
    
    public void ToggleGameModeGraphic(string mode)
    {
        StartCoroutine(ToggleGameModeGraphicRoutine(mode));
    }

    IEnumerator ToggleGameModeGraphicRoutine(string mode)
    {
        for (float i = 1f; i > 0 ; i-= 0.1f)
        {
            skeletonGraphic.color = new Color(skeletonGraphic.color.r, skeletonGraphic.color.g, skeletonGraphic.color.b, i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(.125f);
        skeletonGraphic.Skeleton.SetSkin(mode);
        skeletonGraphic.Skeleton.SetSlotsToSetupPose();
        skeletonGraphic.LateUpdate();
        skeletonGraphic.color = new Color(skeletonGraphic.color.r, skeletonGraphic.color.g, skeletonGraphic.color.b, 1f);
        skeletonGraphic.AnimationState.SetAnimation(0, "animation", false);
    }
}
