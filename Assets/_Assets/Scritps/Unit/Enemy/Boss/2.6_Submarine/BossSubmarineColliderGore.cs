using UnityEngine;
using System.Collections;

public class BossSubmarineColliderGore : MonoBehaviour
{
    private BossSubmarine boss;

    private void Awake()
    {
        boss = transform.root.GetComponent<BossSubmarine>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = other.transform.root.GetComponent<BaseUnit>();

            if (unit != null)
            {
                float damage = boss.HpPercent > 0.5f ? ((SO_BossSubmarineStats)boss.baseStats).GoreDamage : ((SO_BossSubmarineStats)boss.baseStats).RageGoreDamage;
                AttackData atkData = new AttackData(boss, damage);
                unit.TakeDamage(atkData);
                unit.AddForce(unit.transform.right, 6f);
                CameraFollow.Instance.AddShake(0.3f, 0.5f);
                gameObject.SetActive(false);
            }
        }
    }
}
