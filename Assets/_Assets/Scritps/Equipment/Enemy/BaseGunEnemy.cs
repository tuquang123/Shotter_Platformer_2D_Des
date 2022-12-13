using UnityEngine;
using System.Collections;
using Spine.Unity;

public class BaseGunEnemy : MonoBehaviour
{
    public SpriteRenderer spr;
    public Transform firePoint;
    public Transform muzzlePoint;
    public BaseBullet bulletPrefab;
    public BaseMuzzle muzzlePrefab;

    private BaseMuzzle muzzle;
    private BoneFollower bone;

    private void Awake()
    {
        bone = gameObject.AddComponent<BoneFollower>();
    }

    public void Active(BaseEnemy shooter)
    {
        bone.skeletonRenderer = shooter.skeletonAnimation;
        bone.boneName = shooter.gunBone;
        bone.followBoneRotation = true;
        bone.followZPosition = true;
        bone.followLocalScale = false;
        bone.followSkeletonFlip = true;

        gameObject.SetActive(true);
    }

    public virtual void Attack(BaseEnemy attacker)
    {
        if (muzzle == null)
        {
            muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
        }

        muzzle.Active();
    }
}
