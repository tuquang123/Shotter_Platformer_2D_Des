using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Loading : MonoBehaviour
{
    public static string nextScene = "Login";

    public Text textTip;
    public Text loadingPercent;
    public Image imgGuide;
    public Sprite[] sprGuides;

    private AsyncOperation async;
    private float timerLoading;
    private string formatPercent = "{0}%";
    private List<string> tips;


    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (async == null)
            return;

        float progress = Mathf.Clamp01(async.progress / 0.9f);
        //loadingPercent.text = string.Format(formatPercent, (int)(progress * 100f));
        if (progress >= 0.99f && Time.timeSinceLevelLoad - timerLoading >= 3f && async.allowSceneActivation == false)
        {
            async.allowSceneActivation = true;
            async = null;
        }
    }

    void OnEnable()
    {
        //RandomTips();
        RandomGuide();
        timerLoading = Time.timeSinceLevelLoad;
        async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;
    }

    public void Show()
    {
        SoundManager.Instance.StopMusic();
        gameObject.SetActive(true);
    }

    private void RandomTips()
    {
        if (tips == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_TIPS);
            tips = JsonConvert.DeserializeObject<List<string>>(textAsset.text);
        }

        if (tips.Count > 0)
        {
            int randomIndex = Random.Range(0, tips.Count);
            textTip.text = string.Format("TIPS: {0}", tips[randomIndex].ToUpper());
        }
        else
        {
            textTip.text = string.Empty;
        }
    }

    private void RandomGuide()
    {
        int index = Random.Range(0, sprGuides.Length);

        imgGuide.sprite = sprGuides[index];
        imgGuide.SetNativeSize();
    }
}
