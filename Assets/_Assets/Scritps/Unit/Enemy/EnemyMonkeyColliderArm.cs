using UnityEngine;
using System.Collections;

public class EnemyMonkeyColliderArm : MonoBehaviour
{
    private EnemyMonkey monkey;

    private void Awake()
    {
        monkey = transform.root.GetComponent<EnemyMonkey>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            if (monkey.IsAttacking)
            {
                BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

                if (unit != null)
                {
                    AttackData atkData = monkey.GetCurentAttackData();
                    unit.TakeDamage(atkData);
                }
            }
        }
    }
}
