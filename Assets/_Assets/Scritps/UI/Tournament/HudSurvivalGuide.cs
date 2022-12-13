using UnityEngine;
using System.Collections;

public class HudSurvivalGuide : MonoBehaviour
{
    public GameObject popup;

    public void Open()
    {
        popup.SetActive(true);
    }

    public void Close()
    {
        popup.SetActive(false);
    }

    public void StartSurvival()
    {
        Close();

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_START_MISSION);
        EventDispatcher.Instance.PostEvent(EventID.StartFirstWave);
    }
}
