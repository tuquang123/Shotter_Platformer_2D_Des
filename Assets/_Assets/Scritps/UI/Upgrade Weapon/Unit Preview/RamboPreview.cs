using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine;

public class RamboPreview : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public SkeletonRenderer skeletonRenderer;
    [SpineAnimation]
    public string shoot, throwGrenade, aim;
    [SpineAnimation]
    public string meleeAttack, knife, pan, guitar;
    [SpineEvent]
    public string eventThrowGrenade;
    [SpineBone]
    public string equipGunBoneName, equipMeleeWeaponBoneName, effectWindBoneName;

    public Vector2 throwGrenadeDirection;
    public Transform throwGrenadePoint;

    private float lastTimeMeleeAttack;
    private float timerFire;
    private float timerThrow;
    [SerializeField]
    private bool isReadyAttack;
    [SerializeField]
    private bool isUsingMeleeWeapon;
    [SerializeField]
    private bool flagMeleeAttack;
    [SerializeField]
    private bool flagThrowGrenade;
    private string readyAttackMethodName = "ReadyAttack";
    private BaseGunPreview gun;
    private BaseGrenadePreview grenade;
    private BaseMeleeWeaponPreview meleeWeapon;
    private Dictionary<int, BaseGunPreview> guns = new Dictionary<int, BaseGunPreview>();
    private Dictionary<int, BaseGrenadePreview> grenades = new Dictionary<int, BaseGrenadePreview>();
    private Dictionary<int, BaseMeleeWeaponPreview> meleeWeapons = new Dictionary<int, BaseMeleeWeaponPreview>();


    private void Start()
    {
        skeletonAnimation.AnimationState.Start += HandleAnimationStart;
        skeletonAnimation.AnimationState.Complete += HandleAnimationCompleted;
        skeletonAnimation.AnimationState.Event += HandleAnimationEvent;
    }

    private void Update()
    {
        if (isUsingMeleeWeapon)
        {
            if (meleeWeapon && isReadyAttack)
            {
                if (flagMeleeAttack == false && flagThrowGrenade == false)
                {
                    float current = Time.time;
                    if (current - lastTimeMeleeAttack > (1f / meleeWeapon.baseStats.AttackTimePerSecond))
                    {
                        lastTimeMeleeAttack = current;
                        flagMeleeAttack = true;
                        PlayAnimationMeleeAttack();
                    }
                }
            }
        }
        else
        {
            if (gun && isReadyAttack)
            {
                timerFire += Time.deltaTime;
                if (timerFire >= (1f / gun.baseStats.AttackTimePerSecond))
                {
                    timerFire = 0;
                    PlayAnimationShoot();
                    gun.Fire();
                }
            }

            if (grenade)
            {
                timerThrow += Time.deltaTime;
                if (timerThrow >= 3f)
                {
                    timerThrow = 0;
                    ShowGun(false);
                    isReadyAttack = false;
                    flagThrowGrenade = true;
                    PlayAnimationThrowGrenade();
                }
            }
        }
    }

    private void ShowGun(bool isShow)
    {
        if (gun)
        {
            gun.gameObject.SetActive(isShow);
        }
    }

    private void ShowMeleeWeapon(bool isShow)
    {
        if (meleeWeapon)
        {
            meleeWeapon.gameObject.SetActive(isShow);
        }
    }

    private void HandleAnimationStart(TrackEntry entry)
    {
        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            meleeWeapon.ActiveEffect(true);
        }
    }

    private void HandleAnimationCompleted(TrackEntry entry)
    {
        if (string.Compare(entry.animation.name, throwGrenade) == 0)
        {
            flagThrowGrenade = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            ActiveAim(true);
            Invoke(readyAttackMethodName, 1f);
        }

        if (string.Compare(entry.animation.name, meleeAttack) == 0)
        {
            flagMeleeAttack = false;
            skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            meleeWeapon.ActiveEffect(false);
        }
    }

    private void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, eventThrowGrenade) == 0)
        {
            float throwForceValue = Random.Range(2f, 2.5f);
            Vector2 v = throwGrenadeDirection * throwForceValue;

            if (grenade)
                grenade.Active(throwGrenadePoint.position, v, transform);
        }
    }

    private void ActiveAim(bool isActive)
    {
        //if (isActive)
        //{
        //    skeletonAnimation.AnimationState.SetAnimation(2, aim, false);
        //}
        //else
        //{
        //    skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
        //}
    }


    #region Gun

    public void EquipGun(int id)
    {
        timerFire = 0;
        isReadyAttack = false;
        isUsingMeleeWeapon = false;
        grenade = null;

        if (guns.ContainsKey(id))
        {
            ActiveGun(id);
        }
        else
        {
            CreateGunPreview(id);
        }

        ShowMeleeWeapon(false);
        Invoke(readyAttackMethodName, 0.5f);
    }

    private void CreateGunPreview(int id)
    {
        string resourcesName = string.Format("gun_preview_{0}", id);

        BaseGunPreview gunPrefab = Resources.Load<BaseGunPreview>(StaticValue.PATH_GUN_PREVIEW_PREFAB + resourcesName);
        BaseGunPreview gunInstance = Instantiate<BaseGunPreview>(gunPrefab, transform);

        BoneFollower bone = gunInstance.gameObject.AddComponent<BoneFollower>();

        if (bone != null)
        {
            bone.skeletonRenderer = skeletonRenderer;
            bone.boneName = equipGunBoneName;
        }

        gunInstance.gameObject.name = id.ToString();
        guns.Add(id, gunInstance);

        ActiveGun(id);
    }

    private void ActiveGun(int id)
    {
        foreach (KeyValuePair<int, BaseGunPreview> pair in guns)
        {
            if (pair.Key == id)
            {
                pair.Value.gameObject.SetActive(true);
                gun = pair.Value;
            }
            else
            {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }

    private void PlayAnimationShoot()
    {
        TrackEntry track = skeletonAnimation.AnimationState.SetAnimation(0, shoot, false);
        track.AttachmentThreshold = 1f;
        track.MixDuration = 0f;
        TrackEntry empty = skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0.1f);
        empty.AttachmentThreshold = 1f;
    }

    #endregion

    #region Grenade

    public void EquipGrenade(int id)
    {
        timerThrow = 2f;
        isUsingMeleeWeapon = false;

        if (grenades.ContainsKey(id))
        {
            ActiveGrenade(id);
        }
        else
        {
            CreateGrenadePreview(id);
        }
    }

    private void CreateGrenadePreview(int id)
    {
        string resourcesName = string.Format("grenade_preview_{0}", id);

        BaseGrenadePreview grenadePrefab = Resources.Load<BaseGrenadePreview>(StaticValue.PATH_GRENADE_PREVIEW_PREFAB + resourcesName);
        BaseGrenadePreview grenadeInstance = Instantiate<BaseGrenadePreview>(grenadePrefab, transform);

        grenadeInstance.gameObject.name = string.Format("grenade_{0}", id);
        grenadeInstance.gameObject.SetActive(false);
        grenades.Add(id, grenadeInstance);

        ActiveGrenade(id);
    }

    private void ActiveGrenade(int id)
    {
        foreach (KeyValuePair<int, BaseGrenadePreview> pair in grenades)
        {
            if (pair.Key == id)
            {
                grenade = pair.Value;
            }
        }
    }

    private void PlayAnimationThrowGrenade()
    {
        ActiveAim(false);
        skeletonAnimation.AnimationState.SetAnimation(1, throwGrenade, false);
    }

    private void ReadyAttack()
    {
        ShowMeleeWeapon(isUsingMeleeWeapon);
        ShowGun(!isUsingMeleeWeapon);
        isReadyAttack = true;
    }

    #endregion

    #region Melee Weapon

    public void EquipMeleeWeapon(int id)
    {
        isReadyAttack = false;
        isUsingMeleeWeapon = true;
        grenade = null;

        if (meleeWeapons.ContainsKey(id))
        {
            ActiveMeleeWeapon(id);
        }
        else
        {
            CreateMeleeWeaponPreview(id);
        }

        ShowGun(false);
        Invoke(readyAttackMethodName, 0.5f);
    }

    private void CreateMeleeWeaponPreview(int id)
    {
        string resourcesName = string.Format("melee_weapon_preview_{0}", id);

        BaseMeleeWeaponPreview weaponPrefab = Resources.Load<BaseMeleeWeaponPreview>(StaticValue.PATH_MELEE_WEAPON_PREVIEW_PREFAB + resourcesName);
        BaseMeleeWeaponPreview weaponInstance = Instantiate<BaseMeleeWeaponPreview>(weaponPrefab, transform);

        BoneFollower bone = weaponInstance.gameObject.AddComponent<BoneFollower>();

        if (bone != null)
        {
            bone.skeletonRenderer = skeletonRenderer;
            bone.boneName = equipMeleeWeaponBoneName;
        }

        weaponInstance.gameObject.name = id.ToString();
        weaponInstance.InitEffect(skeletonAnimation, effectWindBoneName);
        meleeWeapons.Add(id, weaponInstance);

        ActiveMeleeWeapon(id);
    }

    private void ActiveMeleeWeapon(int id)
    {
        foreach (KeyValuePair<int, BaseMeleeWeaponPreview> pair in meleeWeapons)
        {
            if (pair.Key == id)
            {
                pair.Value.gameObject.SetActive(true);
                meleeWeapon = pair.Value;
            }
            else
            {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }

    private void PlayAnimationMeleeAttack()
    {
        if (meleeWeapon)
        {
            switch (meleeWeapon.type)
            {
                case MeleeWeaponType.Knife:
                    meleeAttack = knife;
                    break;

                case MeleeWeaponType.Pan:
                    meleeAttack = pan;
                    break;

                case MeleeWeaponType.Guitar:
                    meleeAttack = guitar;
                    break;
            }
        }

        skeletonAnimation.AnimationState.SetAnimation(1, meleeAttack, false);
    }

    #endregion
}
