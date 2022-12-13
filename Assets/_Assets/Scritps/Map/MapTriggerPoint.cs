using UnityEngine;
using System.Collections;

public class MapTriggerPoint : MonoBehaviour
{
    public MapTriggerPointType triggerType;

    // Enter new zone
    private Zone mainZone;

    // Load next enemy pack
    private int nextZoneId;

    void Start()
    {
        switch (triggerType)
        {
            case MapTriggerPointType.LoadNextEnemyPack:
                nextZoneId = int.Parse(name);
                break;

            case MapTriggerPointType.EnterZone:
                mainZone = transform.parent.GetComponent<Zone>();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            switch (triggerType)
            {
                case MapTriggerPointType.EnterZone:
                    EnterNewZone();
                    break;

                case MapTriggerPointType.LockZone:
                    LockCurrentZone();
                    break;

                case MapTriggerPointType.LoadNextEnemyPack:
                    LoadNextZoneEnemy();
                    break;
            }
        }

        gameObject.SetActive(false);
    }

    private void EnterNewZone()
    {
        if (mainZone != null)
        {
            EventDispatcher.Instance.PostEvent(EventID.EnterZone, mainZone.id);
            gameObject.SetActive(false);
        }
        else
        {
            DebugCustom.LogError("Main zone NULL");
        }
    }

    private void LockCurrentZone()
    {
        GameController.Instance.CampaignMap.LockCurrentZone();
    }

    private void LoadNextZoneEnemy()
    {
        ((CampaignModeController)GameController.Instance.modeController).CreateEnemyInZone(nextZoneId);
    }
}
