using UnityEngine;
using System.Collections;

public class WarningBoss : MonoBehaviour
{
    public void Active()
    {
        gameObject.SetActive(true);

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_WARNING);
    }

    public void Deactive()
    {
        gameObject.SetActive(false);

        EventDispatcher.Instance.PostEvent(EventID.WarningBossDone);
    }
}
