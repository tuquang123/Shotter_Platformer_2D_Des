using UnityEngine;
using System.Collections;

public class Rate : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        SoundManager.Instance.PlaySfxClick();
        gameObject.SetActive(false);

        FirebaseAnalyticsHelper.LogEvent("N_HideRate");
    }

    public void GoToRate()
    {
        SoundManager.Instance.PlaySfxClick();
        ProfileManager.UserProfile.isNoLongerRate.Set(true);
        gameObject.SetActive(false);
        Utility.OpenStore();

        FirebaseAnalyticsHelper.LogEvent("N_GoStoreRate");
    }
}
