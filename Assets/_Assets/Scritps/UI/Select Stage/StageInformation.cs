using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageInformation : MonoBehaviour
{
    public Text textStageNameId;
    public GameObject btnStartEnable;
    public GameObject btnStartDisable;
    public RewardElement[] rewardCells;
    public GameObject[] stars;
    public GameObject[] highlights;
    public GameObject[] locks;
    public GameObject[] ticks;

    private string stageId;
    private Difficulty selectingDifficulty;
    private Difficulty highestPlayableDifficulty;

    public void Open(string stageId)
    {
        this.stageId = stageId;

        textStageNameId.text = string.Format("STAGE {0}", stageId);
        highestPlayableDifficulty = MapUtils.GetHighestPlayableDifficulty(stageId);
        selectingDifficulty = highestPlayableDifficulty;

        List<bool> progress = GameData.playerCampaignStageProgress.GetProgress(stageId);

        for (int i = 0; i < 3; i++)
        {
            locks[i].SetActive(i > (int)highestPlayableDifficulty);
            ticks[i].SetActive(progress[i]);
        }

        int numberStar = MapUtils.GetNumberOfStar(stageId);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < numberStar);
        }

        SetInformation();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SelectDifficulty(int difficulty)
    {
        if ((int)selectingDifficulty == difficulty)
            return;

        selectingDifficulty = (Difficulty)difficulty;
        SetInformation();
    }

    public void Play()
    {
        GameData.mode = GameMode.Campaign;
        StartMission();

        if (GameData.isShowingTutorial && string.Compare(stageId, "1.1") == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.CompleteStep, TutorialType.WorldMap);
        }
    }

    private void StartMission()
    {
        GameData.currentStage = new StageData(stageId, selectingDifficulty);
        DebugCustom.Log(string.Format("Start stage={0}, difficulty={1}", GameData.currentStage.id, GameData.currentStage.difficulty));
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_START_MISSION);
        Loading.nextScene = StaticValue.SCENE_GAME_PLAY;
        Popup.Instance.loading.Show();
    }

    private void SetInformation()
    {
        // Highlight
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].SetActive(i == (int)selectingDifficulty);
        }

        // Rewards
        List<RewardData> rewards = new List<RewardData>();
        if (MapUtils.IsStagePassed(stageId, selectingDifficulty))
        {
            rewards = MapUtils.GetStaticRewards(stageId, selectingDifficulty);
        }
        else
        {
            rewards = MapUtils.GetFirstTimeRewards(stageId, selectingDifficulty);
        }

        for (int i = 0; i < rewardCells.Length; i++)
        {
            RewardElement cell = rewardCells[i];

            cell.gameObject.SetActive(false);
            cell.gameObject.SetActive(i < rewards.Count);

            if (i < rewards.Count)
            {
                RewardData rw = rewards[i];
                cell.SetInformation(rw);
            }
        }

        btnStartEnable.SetActive(selectingDifficulty <= highestPlayableDifficulty);
        btnStartDisable.SetActive(selectingDifficulty > highestPlayableDifficulty);
    }
}
