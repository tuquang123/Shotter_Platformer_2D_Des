using UnityEngine;
using System.Collections;

public class MilitaryBaseSensor : MonoBehaviour
{
    private CircleCollider2D sensor;
    private MilitaryBase office;

    void Awake()
    {
        sensor = GetComponent<CircleCollider2D>();
        office = transform.parent.GetComponent<MilitaryBase>();

        if (sensor == null)
            DebugCustom.LogError("Military Base Sensor NULL");

        if (office == null)
            DebugCustom.LogError("Military Base NULL");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            office.OnAlarm();
        }
    }
}
