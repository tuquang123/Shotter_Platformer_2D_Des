using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public Transform destination;
    public Collider2D top;
    public GameObject baseElevator;
    public float moveSpeed = 1f;
    public Collider2D[] sideColliders;

    private bool isMoving;
    private Collider2D trigger;


    private void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
        top.enabled = true;
        trigger.enabled = true;
        ActiveSideColliders(false);
    }

    private void Update()
    {
        if (isMoving)
        {
            if (Mathf.Abs(destination.position.y - baseElevator.transform.position.y) >= 0.1f)
            {
                baseElevator.transform.position = Vector3.MoveTowards(baseElevator.transform.position, destination.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                baseElevator.transform.position = destination.position;
                isMoving = false;
                ActiveSideColliders(false);
                enabled = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            trigger.enabled = false;
            top.enabled = false;
            ActiveSideColliders(true);
            this.StartDelayAction(() => isMoving = true, 1f);
        }
    }

    private void ActiveSideColliders(bool isActive)
    {
        for (int i = 0; i < sideColliders.Length; i++)
        {
            sideColliders[i].enabled = isActive;
        }
    }
}
