using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SlowMotion : MonoBehaviour
{
    private UnityAction endSlowMotionCallback;
    private float duration;
    private float timer;

    public bool IsShowing { get; set; }


    private void LateUpdate()
    {
        if (IsShowing)
        {
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            timer += Time.deltaTime * 5f;

            if (timer >= duration)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                IsShowing = false;
                //CameraFollow.Instance.lensBlur.enabled = false;

                if (endSlowMotionCallback != null)
                {
                    endSlowMotionCallback.Invoke();
                    endSlowMotionCallback = null;
                }
            }
        }
    }

    public void Show(float duration = 3.5f, UnityAction callback = null)
    {
        IsShowing = true;

        //if (callback == null)
        //{
        //    endSlowMotionCallback = Reset;
        //}
        //else
        //{
        endSlowMotionCallback = callback;
        //}

        //CameraFollow.Instance.lensBlur.enabled = true;

        CameraFollow.Instance.SetSlowMotion();

        this.duration = duration;
        timer = 0f;
        Time.timeScale = 0.2f;
    }

    private void Reset()
    {
        CameraFollow.Instance.ResetCameraToPlayer();
    }
}
