using UnityEngine;
using System.Collections;

public class HouseSpawnEnemySensor : MonoBehaviour
{
    private CircleCollider2D sensor;
    private HouseSpawnEnemy house;

    void Awake()
    {
        sensor = GetComponent<CircleCollider2D>();
        house = transform.parent.GetComponent<HouseSpawnEnemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            house.Open();
            gameObject.SetActive(false);
        }
    }
}
