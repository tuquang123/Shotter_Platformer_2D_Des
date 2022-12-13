using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBall : BaseBullet
{
    private float distance;
    private Vector2 startPoint;

    private float timeApplyDamage;
    private float timerDamage;

    private List<BaseUnit> victims = new List<BaseUnit>();


    protected override void Update()
    {
        base.Update();

        if (transform.localScale.x <= 2)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * 2f, 2f * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        ApplyDamage();
    }

    protected override void TrackingDeactive()
    {
        if (Vector2.Distance(transform.position, startPoint) >= distance)
        {
            Deactive();
            return;
        }

        if (isDeactiveWhenOutScreen)
        {
            bool isOutOfScreenX = transform.position.x < CameraFollow.Instance.left.position.x - 1f || transform.position.x > CameraFollow.Instance.right.position.x + 1f;
            bool isOutOfScreenY = transform.position.y > CameraFollow.Instance.top.position.y + 1f;

            if (isOutOfScreenX || isOutOfScreenY)
            {
                Deactive();
            }
        }
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
        if (unit != null && victims.Contains(unit) == false)
        {
            victims.Add(unit);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
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

        if (unit != null && victims.Contains(unit))
        {
            victims.Remove(unit);
        }
    }

    public void Active(AttackData attackData, Transform releasePoint, float moveSpeed, float timeApplyDamage, float distance, Transform parent = null)
    {
        this.attackData = attackData;
        this.moveSpeed = moveSpeed;
        this.timeApplyDamage = timeApplyDamage;
        this.distance = distance;

        SetTagAndLayer();

        transform.position = releasePoint.position;
        transform.rotation = releasePoint.rotation;
        transform.parent = parent;
        startPoint = transform.position;

        transform.localScale = Vector3.one;

        timerDamage = 0;
        distance = 0;
        victims.Clear();

        gameObject.SetActive(true);
    }

    private void ApplyDamage()
    {
        timerDamage += Time.deltaTime;

        if (timerDamage >= timeApplyDamage)
        {
            timerDamage = 0;

            for (int i = 0; i < victims.Count; i++)
            {
                victims[i].TakeDamage(attackData);
            }
        }
    }
}
