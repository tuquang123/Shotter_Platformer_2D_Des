using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class WindBlade : MonoBehaviour
{
    public float animTimeScale = 1f;
    public SkeletonAnimation skeletonAnimation;
    [SpineAnimation]
    public string animAttack;
    public bool isDeactiveCompleteAnimation = true;

    private void Awake()
    {
        skeletonAnimation.AnimationState.Complete += HandleSpineEventCompleted;
    }

    public void Active(bool isActive)
    {
        if (isActive)
        {
            gameObject.SetActive(true);
            skeletonAnimation.AnimationState.SetAnimation(0, animAttack, false).TimeScale = animTimeScale;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleSpineEventCompleted(TrackEntry entry)
    {
        if (isDeactiveCompleteAnimation)
        {
            if (string.Compare(entry.animation.name, animAttack) == 0)
            {
                skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0);
                gameObject.SetActive(false);
            }
        }

    }
}
