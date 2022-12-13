using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BulletRocketChaser : BaseBullet
{
    public float turnSpeed = 50f;
    public LayerMask layerVictim;

    private Vector2 readyPosition;
    private bool isReady;
    private bool isPreparing;
    private bool isFacingRight;
    protected Collider2D[] victims = new Collider2D[10];


    protected override void Move()
    {
        if (isReady)
        {
            if (target)
            {
                Vector3 dir = target.position - transform.position;
                transform.right = Vector3.MoveTowards(transform.right, dir.normalized, turnSpeed * Time.deltaTime);
            }

            transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            if (isPreparing)
            {
                isPreparing = false;
                transform.DOMove(readyPosition, 0.2f).OnComplete(() =>
                {
                    isReady = true;
                    BaseUnit unit = GetNearestEnemy();
                    if (unit != null)
                    {
                        SetTarget(unit.BodyCenterPoint);
                    }
                });
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        BaseUnit unit = null;

        if (other.CompareTag(StaticValue.TAG_ENEMY_BODY_PART))
        {
            unit = GameController.Instance.GetUnit(other.gameObject);

            if (unit != null)
                unit.TakeDamage(attackData);
        }
        else if (other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackData.radiusDealDamage, victims, layerVictim);

            for (int i = 0; i < count; i++)
            {
                if (victims[i].CompareTag(StaticValue.TAG_ENEMY_BODY_PART) == false)
                {
                    unit = GameController.Instance.GetUnit(victims[i].transform.root.gameObject);

                    if (unit != null)
                    {
                        float distance = Vector3.Distance(transform.position, unit.BodyCenterPoint.position);
                        float distancePercent = Mathf.Clamp01((distance - 0.5f) / (attackData.radiusDealDamage - 0.5f));
                        float damagePercentByDistance = 1 - (distancePercent * 0.4f);
                        float finalDamage = attackData.damage * damagePercentByDistance;
                        attackData.damage = finalDamage;
                        unit.TakeDamage(attackData);
                    }
                }
            }
        }

        SpawnHitEffect();
        Deactive();
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    public void Active(AttackData attackData, Transform releasePoint, Vector2 readyPosition, float moveSpeed, Transform parent = null)
    {
        target = null;
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;
        this.readyPosition = readyPosition;
        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;

        isReady = false;
        isPreparing = true;
        isFacingRight = (readyPosition.x - releasePoint.position.x) > 0;

        gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        base.Deactive();

        target = null;

        PoolingController.Instance.poolBulletRocketChaser.Store(this);
    }

    private BaseUnit GetNearestEnemy()
    {
        BaseUnit unit = null;
        float nearestDistance = 15f;

        foreach (BaseUnit enemy in GameController.Instance.activeUnits.Values)
        {
            if (enemy.CompareTag(StaticValue.TAG_ENEMY) && enemy.isDead == false)
            {
                Vector2 pos = enemy.BodyCenterPoint.position;
                float distance = Vector2.Distance(pos, transform.position);

                if (distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    unit = enemy;
                }
            }
        }

        return unit;
    }
}
