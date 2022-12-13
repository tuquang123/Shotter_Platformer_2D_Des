using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class MapChooser : MonoBehaviour
{
    public static WorldMapNavigation navigation;
    public StageInformation stageInfoController;

    [Header("MAP")]
    public int currentMapIndex = 0;
    public GameObject btnNextMap;
    public GameObject btnPreviousMap;
    public Text currentStar;
    public Text maxStar;
    public Image starProgress;
    public MapOverview[] mapOverviews;
    public CampaignBoxReward[] boxes;
    //public RectTransform[] mapRectTransforms;

    [Header("MAP PAGE")]
    public Sprite pageActive;
    public Sprite pageDeactive;
    public Image[] pageMap;

    private int totalMap;
    private int totalDifficulty;
    private string selectingStageId;
    private Difficulty currentDifficulty;

    //private Vector2 leftPos = new Vector2(-725f, 0f);
    //private Vector2 displayPosition = Vector2.zero;
    //private Vector2 rightPos = new Vector2(725f, 0f);
    //private bool isTweening;
    //private float tweenSpeed = 0.5f;

    //public static StageData SelectingStageData;



    #region UNITY METHODS

    void Awake()
    {
        totalMap = Enum.GetNames(typeof(MapType)).Length;
        totalDifficulty = Enum.GetNames(typeof(Difficulty)).Length;

        for (int i = 0; i < mapOverviews.Length; i++)
        {
            mapOverviews[i].Init();
        }
    }

    void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClickStageOnWorldMap, (sender, param) => ShowStageInformation((string)param));
        EventDispatcher.Instance.RegisterListener(EventID.ClaimCampaignBox, (sender, param) => OnClaimBoxReward((int)param));
    }

    void OnEnable()
    {
        string stage = string.Empty;
        MapType map;

        switch (navigation)
        {
            case WorldMapNavigation.None:
                stage = MapUtils.GetCurrentProgressStageId();
                map = MapUtils.GetMapType(stage);
                currentMapIndex = (int)map - 1;
                break;

            case WorldMapNavigation.NextStageFromGame:
                stage = MapUtils.GetNextStage(GameData.currentStage);
                map = MapUtils.GetMapType(stage);
                currentMapIndex = (int)map - 1;
                ShowStageInformation(stage);
                break;
        }

        navigation = WorldMapNavigation.None;
        UpdateWorldMapInformation();
    }

    void OnDisable()
    {
        if (stageInfoController)
            stageInfoController.Close();
    }

    #endregion

    public void NextMap()
    {
        //if (!isTweening)
        //{
        //    int nextMap = Mathf.Clamp(currentMapIndex + 1, 0, mapRectTransforms.Length - 1);

        //    if (nextMap != currentMapIndex)
        //    {
        //        UpdateWorldMapInformation(nextMap);

        //        isTweening = true;

        //        mapRectTransforms[nextMap].anchoredPosition = rightPos;

        //        mapRectTransforms[currentMapIndex].DOAnchorPos(leftPos, tweenSpeed);
        //        mapRectTransforms[nextMap].DOAnchorPos(displayPosition, tweenSpeed).OnComplete(() =>
        //        {
        //            pageMap[currentMapIndex].sprite = pageDeactive;
        //            pageMap[nextMap].sprite = pageActive;

        //            currentMapIndex = nextMap;
        //            isTweening = false;

        //            btnNextMap.SetActive(currentMapIndex < mapRectTransforms.Length - 1);
        //            btnPreviousMap.SetActive(true);

        //            EventDispatcher.Instance.PostEvent(EventID.AnimateWorldMapDone);
        //        });
        //    }
        //}

        SoundManager.Instance.PlaySfxClick();
        currentMapIndex++;
        UpdateWorldMapInformation();
    }

    public void PreviousMap()
    {
        //if (!isTweening)
        //{
        //    int previousMap = Mathf.Clamp(currentMapIndex - 1, 0, mapRectTransforms.Length - 1);

        //    if (previousMap != currentMapIndex)
        //    {
        //        UpdateWorldMapInformation(previousMap);

        //        isTweening = true;

        //        mapRectTransforms[previousMap].anchoredPosition = leftPos;

        //        mapRectTransforms[currentMapIndex].DOAnchorPos(rightPos, tweenSpeed);
        //        mapRectTransforms[previousMap].DOAnchorPos(displayPosition, tweenSpeed).OnComplete(() =>
        //        {
        //            pageMap[currentMapIndex].sprite = pageDeactive;
        //            pageMap[previousMap].sprite = pageActive;

        //            currentMapIndex = previousMap;
        //            isTweening = false;

        //            btnNextMap.SetActive(true);
        //            btnPreviousMap.SetActive(currentMapIndex > 0);

        //            EventDispatcher.Instance.PostEvent(EventID.AnimateWorldMapDone);
        //        });
        //    }
        //}

        SoundManager.Instance.PlaySfxClick();
        currentMapIndex--;
        UpdateWorldMapInformation();
    }

    private void ShowStageInformation(string stageId)
    {
        if (string.IsNullOrEmpty(stageId))
            return;

        stageInfoController.Open(stageId);
    }

    private void UpdateWorldMapInformation()
    {
        MapType mapType = GetMapType(currentMapIndex);

        for (int i = 0; i < totalMap; i++)
        {
            mapOverviews[i].Active(i == currentMapIndex);
            pageMap[i].sprite = i == currentMapIndex ? pageActive : pageDeactive;
        }

        // Stars
        int numberOfStages = MapUtils.GetNumberOfStage(mapType);
        int curStars = MapUtils.GetNumberOfStar(mapType);
        int totalStars = numberOfStages * totalDifficulty;
        maxStar.text = totalStars.ToString();
        currentStar.text = curStars.ToString();
        //starProgress.fillAmount = Mathf.Clamp01((float)curStars / (float)(totalStars-numberOfStages));

        // Arrow
        btnNextMap.SetActive(currentMapIndex < mapOverviews.Length - 1);
        btnPreviousMap.SetActive(currentMapIndex > 0);

        // Box rewards
        if (GameData.playerCampaignRewardProgress.ContainsKey(mapType) == false)
        {
            GameData.playerCampaignRewardProgress.AddNewProgress(mapType);
        }

        this.StartActionEndOfFrame(LoadBoxRewardState);
    }

    private void LoadBoxRewardState()
    {
        MapType mapType = GetMapType(currentMapIndex);
        List<bool> rewardProgress = GameData.playerCampaignRewardProgress[mapType];
        int currentStar = MapUtils.GetNumberOfStar(mapType);

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].LoadState(currentStar, rewardProgress);
        }
    }

    private void OnClaimBoxReward(int index)
    {
        MapType mapType = GetMapType(currentMapIndex);

        List<RewardData> rewards = GameData.staticCampaignBoxRewardData.GetRewards(mapType, index);
        RewardUtils.Receive(rewards);
        Popup.Instance.ShowReward(rewards);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

        GameData.playerCampaignRewardProgress.ClaimReward(mapType, index);
        LoadBoxRewardState();

        FirebaseAnalyticsHelper.LogEvent("N_ClaimBoxCampaign", string.Format("Map {0} - Box {1}", (int)mapType, index + 1));
    }

    private MapType GetMapType(int mapIndex)
    {
        return (MapType)(mapIndex + 1);
    }
}
