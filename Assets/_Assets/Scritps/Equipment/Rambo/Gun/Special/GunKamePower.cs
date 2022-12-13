using UnityEngine;
using System.Collections;

public class GunKamePower : BaseGun
{
    public GameObject chargeEffect;
    public AudioSource audioSourceCharge;

    private float timerCharge;
    private bool isCharging;
    private bool isReadyShoot;
    private BaseUnit shooter;

    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_KAME_POWER, level);
        baseStats = Resources.Load<SO_GunKamePowerStats>(path);
    }

    protected override void Awake()
    {
        shooter = transform.root.GetComponent<BaseUnit>();
        chargeEffect.SetActive(false);

        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, (sender, param) => Shoot((bool)param));
    }

    private void Update()
    {
        if (isCharging)
        {
            timerCharge += Time.deltaTime;

            if (timerCharge >= ((SO_GunKamePowerStats)baseStats).ChargeTime)
            {
                timerCharge = 0;
                AttackData atkData = ((Rambo)shooter).GetCurentAttackData();
                ReleaseBullet(atkData, 1f);
            }
        }
    }

    public override void Attack(AttackData attackData)
    {
        isReadyShoot = true;
    }

    private void ReleaseBullet(AttackData attackData, float percentCharge)
    {
        if (isReadyShoot)
        {
            isReadyShoot = false;

            base.ReleaseBullet(attackData);

            if (isInfinityAmmo == false && ammo <= 0)
                return;

            BulletKamePower bullet = PoolingController.Instance.poolBulletKamePower.New();

            if (bullet == null)
            {
                bullet = Instantiate(bulletPrefab) as BulletKamePower;
            }

            attackData.damage *= percentCharge;

            float bulletSpeed = ((SO_GunKamePowerStats)baseStats).BulletSpeed;
            bulletSpeed *= percentCharge;

            bullet.Active(attackData, firePoint, bulletSpeed, percentCharge);

            ActiveMuzzle();
            PlaySoundAttack();
        }
    }

    private void Shoot(bool isFire)
    {
        if (this)
        {
            if (gameObject.activeInHierarchy)
            {
                if (isFire)
                {
                    isCharging = true;
                    timerCharge = 0;
                    chargeEffect.SetActive(true);
                    audioSourceCharge.Play();
                }
                else
                {
                    isCharging = false;
                    chargeEffect.SetActive(false);
                    audioSourceCharge.Stop();

                    float percentCharge = Mathf.Clamp(timerCharge / ((SO_GunKamePowerStats)baseStats).ChargeTime, 0.5f, 1f);
                    AttackData atkData = ((Rambo)shooter).GetCurentAttackData();
                    ReleaseBullet(atkData, percentCharge);
                }
            }
        }
    }
}
