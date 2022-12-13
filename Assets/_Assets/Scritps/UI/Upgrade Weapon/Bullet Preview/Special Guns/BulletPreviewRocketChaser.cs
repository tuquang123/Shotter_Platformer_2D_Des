using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BulletPreviewRocketChaser : BaseBulletPreview
{
    public float turnSpeed = 10f;

    private Transform target;
    private Vector2 readyPosition;
    private bool isReady;
    private bool isPreparing;

    protected override void Deactive()
    {
        base.Deactive();

        PoolingPreviewController.Instance.rocketChaser.Store(this);
    }

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
                    //EventDispatcher.Instance.PostEvent(EventID.PreviewRocketChaserReady, this);
                });
            }
        }
    }

    public void Active(Transform firePoint, Vector2 readyPosition, float moveSpeed, Transform parent = null)
    {
        target = null;
        this.moveSpeed = moveSpeed;

        this.readyPosition = readyPosition;
        transform.position = firePoint.position;
        transform.rotation = firePoint.rotation;
        transform.parent = parent;

        isReady = false;
        isPreparing = true;

        gameObject.SetActive(true);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
    }

    public void Focus(Transform target)
    {
        this.target = target;
    }
}
