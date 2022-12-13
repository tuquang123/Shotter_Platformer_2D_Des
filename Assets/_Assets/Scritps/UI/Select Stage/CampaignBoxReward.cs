using UnityEngine;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine.UI;

public class CampaignBoxReward : MonoBehaviour
{
    public int index;
    public SkeletonGraphic skeletonGraphic;
    [SpineAnimation]
    public string normal, ready, opened;
    [SpineSkin]
    public string skinNormal, skinReady, skinOpened;
    public Button button;

    private int requireStar;

    public void LoadState(int currentStar, List<bool> progress)
    {
        if (progress[index])
        {
            skeletonGraphic.AnimationState.SetAnimation(0, opened, false).TimeScale = 50f;
            button.enabled = false;
        }
        else
        {
            requireStar = (index + 1) * 8;

            if (currentStar >= requireStar)
            {
                skeletonGraphic.AnimationState.SetAnimation(0, ready, true);
                button.enabled = true;
            }
            else
            {
                skeletonGraphic.AnimationState.SetAnimation(0, normal, false).TimeScale = 50f;
                button.enabled = false;
            }
        }
    }

    public void Claim()
    {
        EventDispatcher.Instance.PostEvent(EventID.ClaimCampaignBox, index);
    }
}
