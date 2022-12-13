using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;

public class LabelMissionStart : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    [SpineAnimation]
    public string anim;

    private void Awake()
    {
        gameObject.transform.localScale = Vector3.zero;
        skeletonGraphic.AnimationState.Complete += HandleAnimationCompleted;
    }

    public void Show()
    {
        gameObject.transform.localScale = Vector3.one;
        skeletonGraphic.AnimationState.SetAnimation(0, anim, false);
    }

    private void HandleAnimationCompleted(TrackEntry entry)
    {
        if (string.Compare(entry.animation.name, anim) == 0)
        {
            gameObject.transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
