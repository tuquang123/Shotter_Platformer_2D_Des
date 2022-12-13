using UnityEngine;
using System.Collections;

public class TriggerOpenTransportDoor : MonoBehaviour
{
    public Transform startDoor;
    public Transform startDoorOpenPosition;
    public Transform endDoor;
    public Transform endDoorOpenPosition;
    public Transform playerPoint;
    public GameObject wallLockLeftUpstairs;
    public GameObject[] objectsHide;
    public GameObject[] objectsShow;

    private BoxCollider2D triggerPlayerEnter;
    private Vector2 startDoorClosedPosition;
    private Vector2 endDoorClosedPosition;
    private bool isOpeningDoor;
    private bool isClosingDoor;

    private void Awake()
    {
        triggerPlayerEnter = GetComponent<BoxCollider2D>();
        triggerPlayerEnter.enabled = false;

        startDoorClosedPosition = startDoor.position;
        endDoorClosedPosition = endDoor.position;

        isOpeningDoor = true;
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_DOOR_OPEN);
    }

    private void Update()
    {
        if (isOpeningDoor)
        {
            if (Mathf.Abs(startDoor.position.y - startDoorOpenPosition.position.y) > 0.1f)
            {
                startDoor.position = Vector2.MoveTowards(startDoor.position, startDoorOpenPosition.position, 2f * Time.deltaTime);
            }
            else
            {
                startDoor.position = startDoorOpenPosition.position;
                isOpeningDoor = false;
                triggerPlayerEnter.enabled = true;
            }
        }

        if (isClosingDoor)
        {
            if (Mathf.Abs(endDoor.position.y - endDoorClosedPosition.y) > 0.1f)
            {
                endDoor.position = Vector2.MoveTowards(endDoor.position, endDoorClosedPosition, 2f * Time.deltaTime);
            }
            else
            {
                endDoor.position = endDoorClosedPosition;
                isClosingDoor = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            triggerPlayerEnter.enabled = false;
            LoadObjects();
            endDoor.position = endDoorOpenPosition.position;
            SceneFading.Instance.FadePingPongBlackAlpha(2f, MovePlayerUpstairs, CloseDestinationDoor);
        }
    }

    private void LoadObjects()
    {
        for (int i = 0; i < objectsHide.Length; i++)
        {
            objectsHide[i].SetActive(false);
        }

        for (int i = 0; i < objectsShow.Length; i++)
        {
            objectsShow[i].SetActive(true);
        }
    }

    private void MovePlayerUpstairs()
    {
        GameController.Instance.Player.transform.position = playerPoint.position;
        GameController.Instance.CampaignMap.SetDefaultMapMargin();
        CameraFollow.Instance.SetMarginLeft(wallLockLeftUpstairs.transform.position.x);
    }

    private void CloseDestinationDoor()
    {
        isClosingDoor = true;
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_DOOR_CLOSE);
    }
}
