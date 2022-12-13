using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleUnit : BaseUnit
{
    public bool isDealDamageAround = false;
    public LayerMask layerVictimExplode;
    public EffectObjectName destructEffect;
    public float yOffsetSpawnEffect;

    private SpriteRenderer render;
    private bool isBlinkingEffect;
    private Collider2D[] victims = new Collider2D[5];


    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        transform.parent = null;

        stats.Init(baseStats);

        GameController.Instance.AddUnit(gameObject, this);
    }

    protected override void EffectTakeDamage()
    {
        if (!isBlinkingEffect)
        {
            isBlinkingEffect = true;
            render.DOColor(Color.red, 0.1f).OnComplete(ChangeColorToDefault);
        }
    }

    protected override void Die()
    {
        base.Die();

        Vector3 pos = transform.position;
        pos.y += yOffsetSpawnEffect;
        EffectController.Instance.SpawnParticleEffect(destructEffect, pos);

        if (isDealDamageAround)
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, 1f, victims, layerVictimExplode);

            for (int i = 0; i < count; i++)
            {
                BaseUnit unit = GameController.Instance.GetUnit(victims[i].transform.root.gameObject);

                if (unit != null)
                {
                    if (unit.CompareTag(StaticValue.TAG_ENEMY) || unit.CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
                    {
                        AttackData atkData = GetCurentAttackData();
                        unit.TakeDamage(atkData);
                    }
                }
            }

            CameraFollow.Instance.AddShake(0.15f, 0.3f);
        }

        gameObject.SetActive(false);
        GameController.Instance.RemoveUnit(gameObject);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }

    public override void TakeDamage(AttackData attackData)
    {
        if (isDead || attackData.attacker.isDead)
            return;

        stats.AdjustStats(-attackData.damage, StatsType.Hp);

        EffectTakeDamage();

        if (stats.HP <= 0)
            Die();
    }

    private void ChangeColorToDefault()
    {
        render.color = Color.white;
        isBlinkingEffect = false;
    }
}
