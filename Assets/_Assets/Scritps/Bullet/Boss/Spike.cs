using UnityEngine;
using System.Collections;

public class Spike : BaseBullet
{
    private bool isReady;

    protected override void Move()
    {
        if (isReady)
            transform.Translate(-transform.up * moveSpeed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
                unit.TakeDamage(attackData);
                EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeHitPlayer);
            }
        }

        SpawnHitEffect();
        Deactive();
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.StoneRainExplosion, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    public void Active(AttackData attackData, Vector2 position, float moveSpeed, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;

        isReady = false;
        SetTagAndLayer();
        transform.position = position;
        gameObject.SetActive(true);

        Invoke("ReadyDrop", 1f);
    }

    public override void Deactive()
    {
        base.Deactive();

        CancelInvoke();
        EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeDeactive);
        PoolingController.Instance.poolSpike.Store(this);
    }

    private void ReadyDrop()
    {
        isReady = true;
    }
}
