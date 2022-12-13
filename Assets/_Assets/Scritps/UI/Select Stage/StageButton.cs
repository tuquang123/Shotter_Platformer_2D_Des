using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageButton : MonoBehaviour
{
    public string stageNameId;
    public Button icon;
    public Sprite iconLock;
    public Sprite iconUnlock;
    public Sprite starLock;
    public Sprite starUnlock;
    public Text textStageName;
    public GameObject focus;
    public Image[] stars;

    private bool isLock;

    public void SelectStage()
    {
        SoundManager.Instance.PlaySfxClick();

        if (isLock)
            return;

        EventDispatcher.Instance.PostEvent(EventID.ClickStageOnWorldMap, stageNameId);

        if (GameData.isShowingTutorial && string.Compare(stageNameId, "1.1") == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepSelectStage);
        }
    }

    public void Load()
    {
        textStageName.text = stageNameId;

        if (MapUtils.IsStagePassed(stageNameId, Difficulty.Normal))
        {
            icon.image.sprite = iconUnlock;

            List<bool> progress = GameData.playerCampaignStageProgress[stageNameId];

            int numberDifficultyPassed = 0;

            for (int i = 0; i < progress.Count; i++)
            {
                if (progress[i])
                {
                    numberDifficultyPassed++;
                }
            }

            ActiveStars(numberDifficultyPassed);
            isLock = false;
        }
        else
        {
            icon.image.sprite = iconLock;
            ActiveStars(0);
            isLock = true;
        }

        // Current focus stage
        string focusStageId = MapUtils.GetCurrentProgressStageId();
        MapType focusMapType = MapUtils.GetMapType(focusStageId);
        MapType map = MapUtils.GetMapType(stageNameId);

        if (string.Compare(focusStageId, stageNameId) == 0)
        {
            if (MapUtils.IsStagePassed(focusStageId, Difficulty.Normal) == false)
            {
                icon.image.sprite = iconUnlock;
                ActiveStars(0);
                isLock = false;
            }

            focus.SetActive(true);
        }
        else if ((int)map < (int)focusMapType)
        {
            if (MapUtils.IsStagePassed(stageNameId, Difficulty.Normal) == false)
            {
                icon.image.sprite = iconUnlock;
                ActiveStars(0);
                isLock = false;
            }

            focus.SetActive(false);
        }
        else
        {
            focus.SetActive(false);
        }

        icon.image.SetNativeSize();
    }

    private void ActiveStars(int number)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = i < number ? starUnlock : starLock;
            stars[i].SetNativeSize();
        }
    }
}

