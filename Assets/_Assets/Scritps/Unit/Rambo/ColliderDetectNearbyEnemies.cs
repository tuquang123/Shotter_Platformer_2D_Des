using UnityEngine;
using System.Collections;

public class ColliderDetectNearbyEnemies : MonoBehaviour
{
    private Rambo rambo;


    private void Awake()
    {
        rambo = transform.root.GetComponent<Rambo>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null && ((BaseEnemy)unit).isEffectMeleeWeapon)
            {
                rambo.OnEnemyEnterNearby(unit);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger && other.transform.root.CompareTag(StaticValue.TAG_ENEMY))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null && ((BaseEnemy)unit).isEffectMeleeWeapon)
            {
                rambo.OnEnemyExitNearby(unit);
            }
        }
    }
}
