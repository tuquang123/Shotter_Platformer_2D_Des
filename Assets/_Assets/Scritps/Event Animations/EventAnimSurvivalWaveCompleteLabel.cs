using UnityEngine;
using System.Collections;

public class EventAnimSurvivalWaveCompleteLabel : MonoBehaviour
{
    public void OnComplete()
    {
        gameObject.SetActive(false);

        EventDispatcher.Instance.PostEvent(EventID.LabelWaveAnimateComplete);
    }

    public void ShakeCamera()
    {
        CameraFollow.Instance.AddShake(1.5f, 0.5f);
    }
}
