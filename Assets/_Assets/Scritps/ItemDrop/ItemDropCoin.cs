using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ItemDropCoin : BaseItemDrop
{
    private float timerDisappear;
    private float timerFlash;
    private bool flagFlash;
    private bool isAutoMoveToPlayer;

    private string methodNameAutoMove = "ActiveAutoMove";

    private void Update()
    {
        if (isAutoMoveToPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameController.Instance.Player.BodyCenterPoint.position, Time.deltaTime * 25f);
        }
        else
        {
            timerDisappear += Time.deltaTime;

            if (timerDisappear >= 7f)
            {
                timerDisappear = 0;
                Deactive();
            }
            else if (timerDisappear >= 4f)
            {
                flagFlash = true;
            }

            if (flagFlash)
            {
                timerFlash += Time.deltaTime;

                if (timerFlash > 0.2f)
                {
                    timerFlash = 0;

                    Color c = spr.color;
                    c.a = c.a == 1f ? 0f : 1f;
                    spr.color = c;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            EventDispatcher.Instance.PostEvent(EventID.GetItemDrop, data);
            Deactive();
        }
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolItemDropCoin.Store(this);
    }

    public override void Active(ItemDropData data, Vector2 position)
    {
        base.Active(data, position);

        flagFlash = false;
        isAutoMoveToPlayer = false;
        timerDisappear = 0;
        timerFlash = 0;
        spr.color = Color.white;
        rigid.bodyType = RigidbodyType2D.Dynamic;
        col.isTrigger = false;
        AddRandomForce();

        if (GameData.isAutoCollectCoin)
        {
            Invoke(methodNameAutoMove, 2f);
        }
    }

    private void AddRandomForce()
    {
        float forceX = 0;
        float forceY = 0;

        forceX = Random.Range(-80f, 80f);
        forceY = Random.Range(200f, 250f);

        Vector2 v;
        //v.x = Random.Range(0, 2) == 1 ? forceX : -forceX;
        v.x = forceX;
        v.y = forceY;

        rigid.AddForce(v, ForceMode2D.Force);
    }

    private void ActiveAutoMove()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = true;
        isAutoMoveToPlayer = true;
    }
}
