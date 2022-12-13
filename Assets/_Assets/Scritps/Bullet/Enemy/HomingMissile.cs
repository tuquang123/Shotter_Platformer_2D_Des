using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : BaseBullet
{
    public float turnSpeed;
    public float delayBeforeChase;
    public float delaySeek = 0.01f;
    public float timeOutChase = 2.5f;

    private float timer;
    private float timerSeek;
    private float countTimeOutChase;

    protected override void Move()
    {
        if (timer < delayBeforeChase)
        {
            timer += Time.deltaTime;
        }
        else if (target)
        {
            if (countTimeOutChase < timeOutChase)
            {
                countTimeOutChase += Time.deltaTime;

                timerSeek += Time.deltaTime;

                if (timerSeek > delaySeek)
                {
                    timerSeek = 0f;
                    Vector3 dir = target.position - transform.position;
                    transform.right = Vector3.MoveTowards(transform.right, dir.normalized, turnSpeed * Time.deltaTime);
                }
            }
        }

        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;

        gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        base.Deactive();

        timer = 0;
        countTimeOutChase = 0;

        PoolingController.Instance.poolHomingMissile.Store(this);
    }

    public void Rotate(int index)
    {
        float angle = index * 45f;

        transform.Rotate(0, 0, angle);
    }
}
