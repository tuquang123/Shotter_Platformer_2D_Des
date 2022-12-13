using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBossVenom : MonoBehaviour
{
    public LineRenderer laserRender;
    public LineRenderer laserNoise;
    public Transform hitEffect;
    public LayerMask collisionMask;
    public float laserRange = 10f;
    public int noiseCount = 10;
    public float noiseWidth = 0.02f;
    public float noiseRandomOffset = 0.12f;
    public RaycastHit2D hit;

    private bool flagFirstHitGround;
    private BossVenom boss;
    private float timerApplyDamage;

    private void OnDisable()
    {
        flagFirstHitGround = false;
    }

    void Start()
    {
        laserNoise.positionCount = noiseCount + 1;
        laserNoise.startWidth = noiseWidth;
        laserNoise.endWidth = noiseWidth;

        boss = transform.root.GetComponent<BossVenom>();
    }

    void LateUpdate()
    {
        if (laserRender != null)
        {
            hit = Physics2D.Linecast(transform.position, transform.position + transform.right * laserRange, collisionMask);

            Vector3 hitPoint;
            float noiseDistance;

            if (hit)
            {
                laserRender.SetPosition(0, transform.position);
                hitPoint = hit.point;
                laserRender.SetPosition(1, hitPoint);

                noiseDistance = hit.distance / noiseCount;

                if (flagFirstHitGround == false && hit.collider.gameObject.layer == StaticValue.LAYER_GROUND)
                {
                    flagFirstHitGround = true;
                    EventDispatcher.Instance.PostEvent(EventID.LaserPoisonHitGround, hit.point);
                }
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

            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        timerApplyDamage += Time.deltaTime;

        if (timerApplyDamage >= 0.3f)
        {
            timerApplyDamage = 0;

            if (hit && hit.collider.transform.root.CompareTag(StaticValue.TAG_PLAYER))
            {
                float damage = boss.HpPercent > 0.5f ? ((SO_BossVenomStats)boss.baseStats).LaserDamage : ((SO_BossVenomStats)boss.baseStats).RageLaserDamage;
                AttackData atkData = new AttackData(boss, damage);
                GameController.Instance.Player.TakeDamage(atkData);
            }
        }
    }
}
