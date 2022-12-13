using UnityEngine;
using System.Collections;

public class FreeGiftController : MonoBehaviour
{
    public GameObject[] notifications;
    public CellViewFreeGift[] freeGifts;

    public void Init()
    {
        for (int i = 0; i < freeGifts.Length; i++)
        {
            freeGifts[i].Init();
        }

        EventDispatcher.Instance.RegisterListener(EventID.NewDay, (sender, param) => OnNewDay());
    }

    public void Open()
    {
        gameObject.SetActive(true);

        SoundManager.Instance.PlaySfxClick();
    }

    public void Close()
    {
        gameObject.SetActive(false);

        SoundManager.Instance.PlaySfxClick();
    }

    public void CheckNotification()
    {
        int countViewAds = ProfileManager.UserProfile.countViewAdsFreeCoin;

        for (int i = 0; i < notifications.Length; i++)
        {
            notifications[i].SetActive(countViewAds < GameData.staticFreeGiftData.Count);
        }
    }

    private void UpdateState()
    {
        for (int i = 0; i < freeGifts.Length; i++)
        {
            freeGifts[i].UpdateState();
        }
    }

    private void OnNewDay()
    {
        UpdateState();
        CheckNotification();
    }
}
