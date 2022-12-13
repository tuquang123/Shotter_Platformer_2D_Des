using UnityEngine;
using System.Collections;

public class NearSensor : MonoBehaviour
{
    public CircleCollider2D col;

    private BaseEnemy owner;


    private void Awake()
    {
        owner = transform.root.GetComponent<BaseEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (owner.isDead)
            return;

        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
                owner.OnUnitGetInNearSensor(unit);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (owner.isDead)
            return;

        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            BaseUnit unit = GameController.Instance.GetUnit(other.transform.root.gameObject);

            if (unit != null)
            {
                owner.OnUnitGetOutNearSensor(unit);
            }
        }
    }
}
