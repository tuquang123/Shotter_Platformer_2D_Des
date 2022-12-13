using UnityEngine;
using System.Collections;
using Spine.Unity;

public class BaseMeleeWeaponEnemy : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    [SpineBone]
    public string windBone;

    private BoneFollower bone;

    private void Awake()
    {
        bone = gameObject.AddComponent<BoneFollower>();
    }

    public void Active(BaseEnemy shooter)
    {
        bone.skeletonRenderer = shooter.skeletonAnimation;
        bone.boneName = shooter.knifeBone;
        bone.followBoneRotation = true;
        bone.followZPosition = true;
        bone.followLocalScale = false;
        bone.followSkeletonFlip = true;

        gameObject.SetActive(true);
    }
}
