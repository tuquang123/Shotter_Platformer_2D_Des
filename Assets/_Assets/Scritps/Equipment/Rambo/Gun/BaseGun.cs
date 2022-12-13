using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : BaseWeapon
{
    public AudioSource audioSource;

    [Header("STATS")]
    public SO_GunStats baseStats;

    [Header("BASE GUN PROPERTIES")]
    public GunType gunType;
    public BaseBullet bulletPrefab;
    public BaseMuzzle muzzlePrefab;
    public Transform firePoint;
    public Transform muzzlePoint;
    public ParticleSystem bulletTrash;

    protected float bulletSpeed;
    protected int bulletPerShoot;
    protected bool isInfinityAmmo = true;
    [SerializeField]
    protected bool hasCartouche = true;
    protected int numberCrossBullet = 3;
    protected BaseMuzzle muzzle;

    private AudioClip soundCartouche;
    private bool isInitialized;

    public int ammo;



    protected virtual void Awake()
    {
        if (SoundManager.Instance != null)
            soundCartouche = SoundManager.Instance.GetAudioClip(StaticValue.SOUND_SFX_CARTOUCHE);

        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, (sender, param) =>
        {
            if (this)
            {
                if (gameObject.activeInHierarchy && (bool)param == false)
                    PlaySoundCartouche();
            }
        });
    }

    public override void LoadScriptableObject() { }

    public override void Init(int level)
    {
        base.Init(level);

        if (isInitialized == false)
        {
            isInitialized = true;
            ammo = GameData.playerGuns.GetGunAmmo(id);
        }

        damage = baseStats.Damage;
        attackTimePerSecond = baseStats.AttackTimePerSecond;
        criticalRate = baseStats.CriticalRate;
        criticalDamageBonus = baseStats.CriticalDamageBonus;
        hasCartouche = baseStats.HasCartouche;
        bulletSpeed = baseStats.BulletSpeed;
        bulletPerShoot = baseStats.BulletPerShoot;

        StaticGunData gunData = GameData.staticGunData.GetData(id);
        isInfinityAmmo = gunData == null ? true : !gunData.isSpecialGun;
    }

    public override void Attack(AttackData attackData)
    {
        if (bulletTrash != null)
        {
            bulletTrash.Play();
        }

        ReleaseBullet(attackData);
        PlaySoundAttack();
    }

    public override void PlaySoundAttack()
    {
        if (attackSounds.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, attackSounds.Length);
            audioSource.PlayOneShot(attackSounds[index]);
        }
    }

    protected virtual void PlaySoundCartouche()
    {
        if (hasCartouche)
            SoundManager.Instance.PlaySfx(soundCartouche, -15f);
    }

    protected virtual void ActiveMuzzle()
    {
        if (muzzlePrefab)
        {
            if (muzzle == null)
            {
                muzzle = Instantiate<BaseMuzzle>(muzzlePrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint.parent);
            }

            muzzle.Active();
        }
    }

    public virtual void ConsumeAmmo(int amount = 1)
    {
        if (isInfinityAmmo == false)
        {
            if (ammo <= 0)
            {
                ammo = 0;
                EventDispatcher.Instance.PostEvent(EventID.OutOfAmmo);
                return;
            }

            ammo -= amount;
            UIController.Instance.UpdateGunTypeText(false, ammo);
        }
    }

    protected virtual void ReleaseBullet(AttackData attackData)
    {
        ConsumeAmmo();
    }

    public virtual void ReleaseCrossBullets(AttackData attackData, Transform crossFirePoint, bool isFacingRight)
    {
        ConsumeAmmo(numberCrossBullet);
    }
}
