using UnityEngine;
using System.Collections;

public class LabAutoDoorSensor : MonoBehaviour
{
    public GameObject[] objectShowWhenOpen;

    private CircleCollider2D sensor;
    private bool isOpeningDoor;
    private Transform door;
    private Vector2 doorDestination;


    private void Awake()
    {
        sensor = GetComponent<CircleCollider2D>();
        door = transform.parent;

        Vector2 v = door.position;
        v.y += 2.35f;
        doorDestination = v;

        ShowObject(false);
    }

    private void Update()
    {
        if (isOpeningDoor)
        {
            if (Mathf.Abs(doorDestination.y - door.position.y) > 0.1f)
            {
                door.position = Vector2.MoveTowards(door.position, doorDestination, 2f * Time.deltaTime);
            }
            else
            {
                door.position = doorDestination;
                isOpeningDoor = false;
                ShowObject(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            isOpeningDoor = true;
            sensor.enabled = false;

            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_DOOR_OPEN);
        }
    }

    private void ShowObject(bool isShow)
    {
        for (int i = 0; i < objectShowWhenOpen.Length; i++)
        {
            objectShowWhenOpen[i].SetActive(isShow);
        }
    }
}
