using UnityEngine;
using System.Collections;

public class BulletPreviewSplit : BaseBulletPreview
{
    public Transform splitPoint;
    public SpriteRenderer sprRenderer;
    public Sprite sprBulletSub;
    public Animator animator;

    private bool isSplit;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_ENEMY))
        {
            EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);

            if (isSplit)
            {
                for (int i = -2; i < 3; i++)
                {
                    BulletPreviewSplit bullet = PoolingPreviewController.Instance.split.New();

                    if (bullet == null)
                    {
                        bullet = Instantiate(this) as BulletPreviewSplit;
                    }

                    bullet.Active(splitPoint, moveSpeed, isSplit: false, parent: PoolingPreviewController.Instance.group);
                    bullet.transform.Rotate(0, 0, i * 45f);
                }
            }
        }

        SpawnHitEffect();
        Deactive();
    }

    public void Active(Transform firePoint, float moveSpeed, bool isSplit, Transform parent = null)
    {
        base.Active(firePoint, moveSpeed, parent);

        this.isSplit = isSplit;

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
    }

    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.split.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactSplitGun, transform.position);
    }
}
