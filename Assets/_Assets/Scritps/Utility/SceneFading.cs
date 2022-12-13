using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFading : MonoBehaviour
{
    public enum FadeAlpha
    {
        Transparent = 0,
        Black
    }

    public static SceneFading Instance { get; private set; }

    [SerializeField]
    private Image fadeImg;
    private bool isFading;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeAlphaTo(FadeAlpha alpha, float fadingTime, bool resetAfterFinish, UnityAction callback = null)
    {
        if (isFading == false)
        {
            isFading = true;
            StartCoroutine(StartFadeTo(alpha, fadingTime, resetAfterFinish, callback));
        }
    }

    public void FadePingPongBlackAlpha(float fadingTime, UnityAction toBlackCallback = null, UnityAction finishCallback = null)
    {
        FadeAlphaTo(FadeAlpha.Black, fadingTime, false, () => ToBlack(fadingTime, toBlackCallback, finishCallback));
    }

    public void FadeOutAndLoadScene(string nextSceneName, bool isShowLoading = true, float fadingTime = 2f)
    {
        if (isShowLoading)
        {
            Loading.nextScene = nextSceneName;
            Popup.Instance.loading.Show();
        }
        else
        {
            FadePingPongBlackAlpha(fadingTime, toBlackCallback: () =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }

    public void ResetAlpha()
    {
        Color c = fadeImg.color;
        c.a = 0f;
        fadeImg.color = c;
    }

    IEnumerator StartFadeTo(FadeAlpha color, float fadingTime, bool resetAfterFinish, UnityAction callback)
    {
        Color c = fadeImg.color;
        int alpha = (int)color;

        while (c.a != alpha)
        {
            c.a = Mathf.MoveTowards(c.a, alpha, fadingTime * Time.deltaTime);
            fadeImg.color = c;
            yield return null;
        }

        isFading = false;

        if (resetAfterFinish)
        {
            c.a = 0f;
            fadeImg.color = c;
        }

        if (callback != null)
        {
            callback();
        }
    }

    void ToBlack(float fadingTime, UnityAction toBlackCallback, UnityAction finishCallback)
    {
        if (toBlackCallback != null)
            toBlackCallback();

        FadeAlphaTo(FadeAlpha.Transparent, fadingTime, false, finishCallback);
    }
}
