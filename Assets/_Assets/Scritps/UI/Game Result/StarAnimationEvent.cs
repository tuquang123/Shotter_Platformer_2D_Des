using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarAnimationEvent : MonoBehaviour
{
    public void OneStarAnimationFinish()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_ONE_STAR);
        CameraFollow.Instance.AddShake(0.3f, 0.15f);
    }

    public void TwoStarAnimationFinish()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_TWO_STAR);
        CameraFollow.Instance.AddShake(0.3f, 0.15f);
    }

    public void ThreeStarAnimationFinish()
    {
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_THREE_STAR);
        CameraFollow.Instance.AddShake(0.3f, 0.15f);
    }
}
