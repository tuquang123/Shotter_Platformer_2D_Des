using UnityEngine;
using System.Collections;

public class TriggerLockType2 : MonoBehaviour
{
    public Collider2D col;
    public GameObject[] objShow;
    public GameObject[] objHide;
    public Transform cameraMarginPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_PLAYER))
        {
            for (int i = 0; i < objShow.Length; i++)
            {
                objShow[i].SetActive(true);
            }

            for (int i = 0; i < objHide.Length; i++)
            {
                objHide[i].SetActive(false);
            }

            CameraFollow.Instance.SetMarginLeft(cameraMarginPoint.position.x);
        }

        col.enabled = false;
    }
}
