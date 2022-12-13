using UnityEngine;
using System.Collections;

public class LaserByGun : BaseBullet
{
    public LineRenderer laserRender;
    public LineRenderer laserNoise;
    public Transform hitEffect;
    public LayerMask victimLayerMask;
    public LayerMask stopLayerMask;
    public float laserRange = 10f;
    public int noiseCount = 10;
    public float noiseWidth = 0.02f;
    public float noiseRandomOffset = 0.12f;
    [HideInInspector]
    public GunLaser gun;
    public AudioClip soundLaser;

    protected float timerApplyDamage;
    protected Vector3 hitPoint;
    protected RaycastHit2D[] victims = new RaycastHit2D[3];

    private AudioSource aud;
    private RaycastHit2D hit;



    protected override void Awake()
    {
        base.Awake();

        aud = GetComponent<AudioSource>();
        aud.loop = true;
        aud.clip = soundLaser;
    }

    private void Start()
    {
        laserNoise.positionCount = noiseCount + 1;
        laserNoise.startWidth = noiseWidth;
        laserNoise.endWidth = noiseWidth;
    }

    void LateUpdate()
    {
        if (laserRender != null)
        {
            hit = Physics2D.Linecast(transform.position, transform.position + transform.right * laserRange, stopLayerMask);

            float noiseDistance;

            if (hit)
            {
                laserRender.SetPosition(0, transform.position);
                hitPoint = hit.point;
                laserRender.SetPosition(1, hitPoint);

                noiseDistance = hit.distance / noiseCount;
            }
            else
            {
                laserRender.SetPosition(0, transform.position);
                hitPoint = transform.position + transform.right * laserRange;
                laserRender.SetPosition(1, hitPoint);

                noiseDistance = laserRange / noiseCount;
            }

            hitEffect.transform.position = hitPoint;

            laserNoise.SetPosition(0, transform.position);
            laserNoise.SetPosition(10, hitPoint);

            for (int i = 1; i < 10; i++)
            {
                Vector3 v = (transform.position + transform.right * i * noiseDistance) + (transform.up * Random.Range(-noiseRandomOffset, noiseRandomOffset));
                laserNoise.SetPosition(i, v);
            }
        }

        ApplyDamage();
    }

    public void Active(bool isActive)
    {
        timerApplyDamage = 0;
        gameObject.SetActive(isActive);

        if (isActive)
        {
            aud.Play();
        }
        else
        {
            aud.Stop();
        }
    }

    private void ApplyDamage()
    {
        timerApplyDamage += Time.deltaTime;

        if (timerApplyDamage >= ((SO_GunLaserStats)gun.baseStats).TimeApplyDamage)
        {
            timerApplyDamage = 0;

            int count = Physics2D.LinecastNonAlloc(transform.position, hitPoint, victims, victimLayerMask);

            for (int i = 0; i < count; i++)
            {
                BaseUnit unit = GameController.Instance.GetUnit(victims[i].transform.root.gameObject);

                //if (unit != null && unit.CompareTag(StaticValue.TAG_ENEMY) && unit.IsOutOfScreen() == false)
                if (unit != null && unit.IsOutOfScreen() == false)
                {
                    AttackData atkData = ((Rambo)gun.shooter).GetGunAttackData();
                    unit.TakeDamage(atkData);
                }
            }

            if (count > 0)
                gun.ConsumeAmmo();
        }
    }
}
