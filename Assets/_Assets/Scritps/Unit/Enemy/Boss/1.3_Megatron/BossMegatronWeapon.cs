using UnityEngine;
using System.Collections;

public class BossMegatronWeapon : MonoBehaviour
{
    private BossMegatron boss;

    private void Awake()
    {
        boss = transform.root.GetComponent<BossMegatron>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            if (boss.IsSmashing)
            {
                BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

                if (unit != null)
                {
                    AttackData atkData = new AttackData(boss, ((SO_BossMegatronStats)boss.baseStats).SmashDamage);
                    unit.TakeDamage(atkData);
                }
            }
        }
    }
}
