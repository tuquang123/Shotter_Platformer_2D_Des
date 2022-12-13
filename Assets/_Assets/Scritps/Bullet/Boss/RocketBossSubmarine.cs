using UnityEngine;
using System.Collections;

public class RocketBossSubmarine : BaseBullet
{
    public float turnSpeed = 4f;
    public AudioClip soundExplode;
    public AudioClip soundMoving;

    private Vector3 destination;


    protected override void Move()
    {
        Vector3 dir = destination - transform.position;
        transform.right = Vector3.MoveTowards(transform.right, dir.normalized, turnSpeed * Time.deltaTime);
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolRocketBossSubmarine.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);

        SoundManager.Instance.PlaySfx(soundExplode);
    }

    public void Active(AttackData attackData, Transform releasePoint, Vector3 destination, float moveSpeed, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;
        this.destination = destination;

        gameObject.SetActive(true);
        SoundManager.Instance.PlaySfx(soundMoving, -15f);
    }
}
