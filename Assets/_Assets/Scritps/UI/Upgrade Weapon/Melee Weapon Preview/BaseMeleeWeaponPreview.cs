using UnityEngine;
using System.Collections;
using Spine.Unity;

public class BaseMeleeWeaponPreview : MonoBehaviour
{
    public int id;
    public MeleeWeaponType type;
    public SO_MeleeWeaponStats baseStats;
    public WindBlade windEffect;

    public virtual void ActiveEffect(bool isActive)
    {
        if (windEffect)
            windEffect.Active(isActive);
    }

    public virtual void InitEffect(SkeletonAnimation skeleton, string boneName)
    {
        if (windEffect)
        {
            BoneFollower bone = windEffect.gameObject.AddComponent<BoneFollower>();
            bone.skeletonRenderer = skeleton;
            bone.boneName = boneName;
            windEffect.transform.parent = null;
        }
    }
}
