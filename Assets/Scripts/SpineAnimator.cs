using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class SpineAnimator : MonoBehaviour
{
    SkeletonGraphic skeletonGraphic;
    Spine.AnimationState animationState;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        animationState = skeletonGraphic.AnimationState;
    }


    public void SetAnimation(string anim)
    {
        animationState.SetAnimation(0, anim, false);
    }

}
