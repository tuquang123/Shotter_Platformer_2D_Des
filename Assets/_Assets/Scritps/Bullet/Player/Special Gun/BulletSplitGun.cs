using UnityEngine;
using System.Collections;

public class BulletSplitGun : BaseBullet
{
    public Transform splitPoint;
    public SpriteRenderer sprRenderer;
    public Sprite sprBulletSub;
    public Animator animator;

    private BaseUnit firstHitUnit;
    private float splitDamage;
    private bool isSplit;

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolBulletSplitGun.Store(this);
    }

    public void Active(AttackData attackData, Transform releasePoint, float moveSpeed, bool isSplit, float splitDamage = 0, BaseUnit firstHitUnit = null, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;
        this.isSplit = isSplit;
        this.splitDamage = splitDamage;
        this.firstHitUnit = firstHitUnit;

        if (isSplit)
        {
            animator.enabled = true;
            sprRenderer.transform.localScale = Vector3.one * 0.75f;
        }
        else
        {
            animator.enabled = false;
            sprRenderer.sprite = sprBulletSub;
            sprRenderer.transform.localScale = Vector3.one;
        }

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;

        gameObject.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        BaseUnit unit = null;

        if (other.CompareTag(StaticValue.TAG_ENEMY_BODY_PART) || other.CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
        {
            unit = GameController.Instance.GetUnit(other.gameObject);
        }
        else if (other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            unit = GameController.Instance.GetUnit(other.transform.root.gameObject);
        }


        if (unit != null)
        {
            if (isSplit)
            {
                unit.TakeDamage(attackData);

                for (int i = -2; i < 3; i++)
                {
                    BulletSplitGun bullet = PoolingController.Instance.poolBulletSplitGun.New();

                    if (bullet == null)
                    {
                        bullet = Instantiate(this) as BulletSplitGun;
                    }

                    attackData.damage = splitDamage;
                    bullet.Active(attackData, splitPoint, moveSpeed, isSplit: false, firstHitUnit: unit, parent: PoolingController.Instance.groupBullet);
                    bullet.transform.Rotate(0, 0, i * 45f);
                }

                SpawnHitEffect();
                Deactive();
            }
            else if (ReferenceEquals(firstHitUnit.gameObject, unit.gameObject) == false)
            {
                unit.TakeDamage(attackData);
                SpawnHitEffect();
                Deactive();
            }
        }
        else
        {
            SpawnHitEffect();
            Deactive();
        }
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactSplitGun, transform.position);
    }
}
