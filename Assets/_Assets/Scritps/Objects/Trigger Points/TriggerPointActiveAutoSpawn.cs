using UnityEngine;
using System.Collections;

public class TriggerPointActiveAutoSpawn : MonoBehaviour
{
    public bool isActiveAutoSpawn = true;

    private BoxCollider2D sensor;


    void Awake()
    {
        sensor = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            ((CampaignModeController)GameController.Instance.modeController).IsAllowSpawnSideEnemy = isActiveAutoSpawn;
            gameObject.SetActive(false);
        }
    }
}
