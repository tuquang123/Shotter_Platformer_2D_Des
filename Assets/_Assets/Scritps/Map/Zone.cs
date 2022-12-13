using UnityEngine;
using System.Collections;

public class Zone : MonoBehaviour
{
    public int id;
    public bool isFinalZone;
    public bool isLockWallEndWhenClear;
    public Collider2D wallStart;
    public CameraLockDirection wallStartLockDir = CameraLockDirection.Left;
    public Collider2D wallEnd;
    public CameraLockDirection wallEndLockDir = CameraLockDirection.Right;
    public GameObject[] objectAppearWhenClear;

    void Awake()
    {
        wallStart.gameObject.SetActive(false);
        wallEnd.gameObject.SetActive(false);
        ShowObjects(false);
    }

    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClearZone, OnClearZone);
    }

    public void Lock()
    {
        wallStart.gameObject.SetActive(true);
        wallEnd.gameObject.SetActive(true);
        SetCameraMargin();
    }

    private void OnClearZone(Component sender, object param)
    {
        int zoneId = (int)param;

        if (zoneId == id)
        {
            if (isFinalZone == false)
                wallEnd.gameObject.SetActive(false);
        }
    }

    public void ShowObjects(bool isShow)
    {
        for (int i = 0; i < objectAppearWhenClear.Length; i++)
        {
            objectAppearWhenClear[i].SetActive(isShow);
        }
    }

    public void SetCameraMargin()
    {
        if (wallStartLockDir == CameraLockDirection.Left && wallEndLockDir == CameraLockDirection.Right)
        {
            CameraFollow.Instance.SetMarginLeft(wallStart.transform.position.x);
            CameraFollow.Instance.SetMarginRight(wallEnd.transform.position.x);
        }
        else if (wallStartLockDir == CameraLockDirection.Right && wallEndLockDir == CameraLockDirection.Left)
        {
            CameraFollow.Instance.SetMarginRight(wallStart.transform.position.x);
            CameraFollow.Instance.SetMarginLeft(wallEnd.transform.position.x);
        }
        else
        {
            DebugCustom.Log(string.Format("Invalid lock dir, wall start={0}, wall end={1}", wallStartLockDir, wallEndLockDir));
        }
    }
}
