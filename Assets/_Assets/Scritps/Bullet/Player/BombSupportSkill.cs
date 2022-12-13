using UnityEngine;
using System.Collections;

public class BombSupportSkill : BaseBullet
{
    public LayerMask layerVictim;

    private float damage;
    private Collider2D[] victims = new Collider2D[10];

    protected override void Move() { }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground Upstairs") == false)
        {
            Explode();
            SpawnHitEffect();
            Deactive();
        }
    }

    private void Explode()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, 2.5f, victims, layerVictim);

        for (int i = 0; i < count; i++)
        {
            BaseUnit unit = null;

            if (victims[i].CompareTag(StaticValue.TAG_ENEMY_BODY_PART) || victims[i].CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
            {
                unit = GameController.Instance.GetUnit(victims[i].gameObject);
            }
            else if (victims[i].transform.root.CompareTag(StaticValue.TAG_ENEMY))
            {
                unit = GameController.Instance.GetUnit(victims[i].transform.root.gameObject);
            }

            if (unit)
                unit.TakeDamage(damage);
        }
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    public void Active(Vector2 position, float damage, Transform parent = null)
    {
        this.damage = damage;

        transform.position = position;
        transform.parent = parent;

        gameObject.SetActive(true);

        //rigid.AddForce(Random.onUnitSphere * 1000f);
        //rigid.AddTorque(250f);
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBombSupportSkill.Store(this);
    }
}
