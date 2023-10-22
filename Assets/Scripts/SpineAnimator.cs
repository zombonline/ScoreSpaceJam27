using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class SpineAnimator : MonoBehaviour
{
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] SkeletonAnimation skeletonAnimation;
    Spine.AnimationState graphicState, animationState;

    private void Awake()
    {
        if (skeletonGraphic != null)
        {
            graphicState = skeletonGraphic.AnimationState;
        }
        if(skeletonAnimation != null)
        {
            animationState= skeletonAnimation.AnimationState;
        }
    }


    public void SetUIAnimation(string anim)
    {
        graphicState.SetAnimation(0, anim, false);
    }
    public void SetUILoopingAnimation(string anim)
    {
        graphicState.SetAnimation(0, anim, true);
    }
    public void SetAnimation(string anim)
    {
        animationState.SetAnimation(0, anim, false);
    }
}
