using UnityEngine;
using System.Collections;

public class BossMegatronFoot : BaseUnit
{
    private BossMegatron boss;

    protected override void Awake()
    {
        base.Awake();

        boss = transform.root.GetComponent<BossMegatron>();
    }

    void OnEnable()
    {
        GameController.Instance.AddUnit(gameObject, this);
    }

    void OnDisable()
    {
        GameController.Instance.RemoveUnit(gameObject);
    }

    public override void TakeDamage(AttackData attackData)
    {
        attackData.damage *= 0.8f;
        boss.TakeDamage(attackData);
    }
}
