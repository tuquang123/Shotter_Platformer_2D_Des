using UnityEngine;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System.Linq;
using UnityEngine.UI;

public class HudDailyQuest : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewDailyQuest;

    public Text countNotification;

    private SmallList<CellViewDailyQuestData> dailyQuestData = new SmallList<CellViewDailyQuestData>();


    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.NewDay, (sender, param) => RefreshDailyQuest());
        EventDispatcher.Instance.RegisterListener(EventID.ClaimDailyQuestReward, (sender, param) => OnClaimReward((CellViewDailyQuestData)param));

        scroller.CreateContainer();
        scroller.Delegate = this;
    }

    private void OnEnable()
    {
        CreateDailyQuestData();
        scroller.ReloadData();
    }

    public void CalculateNotification()
    {
        int count = GameData.playerDailyQuests.GetNumberReadyQuest();

        if (count > 0)
        {
            countNotification.transform.parent.gameObject.SetActive(true);
            countNotification.text = count.ToString();
        }
        else
        {
            countNotification.transform.parent.gameObject.SetActive(false);
        }
    }

    private void CreateDailyQuestData()
    {
        dailyQuestData.Clear();

        for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
        {
            PlayerDailyQuestData playerQuest = GameData.playerDailyQuests[i];
            StaticDailyQuestData staticQuest = GameData.staticDailyQuestData.GetData(playerQuest.type);

            CellViewDailyQuestData cellViewData = new CellViewDailyQuestData();
            cellViewData.type = playerQuest.type;
            cellViewData.title = staticQuest.title;
            cellViewData.description = string.Format(staticQuest.description, staticQuest.value);
            cellViewData.progress = playerQuest.progress;
            cellViewData.target = staticQuest.value;
            cellViewData.isClaimed = playerQuest.isClaimed;
            cellViewData.rewards = staticQuest.rewards;

            dailyQuestData.Add(cellViewData);
        }
    }

    private void RefreshDailyQuest()
    {
        CreateDailyQuestData();
        scroller.ReloadData();
    }

    private void OnClaimReward(CellViewDailyQuestData data)
    {
        if (data.type != DailyQuestType.COMPLETE_ALL_QUEST)
        {
            int completedQuests = GameData.playerDailyQuests.Count(x => x.isClaimed && x.type != DailyQuestType.COMPLETE_ALL_QUEST);

            if (completedQuests == GameData.playerDailyQuests.Count - 1)
            {
                EventDispatcher.Instance.PostEvent(EventID.CompleteAllDailyQuests);

                for (int i = 0; i < dailyQuestData.Count; i++)
                {
                    CellViewDailyQuestData questData = dailyQuestData[i];

                    if (questData.type == DailyQuestType.COMPLETE_ALL_QUEST)
                    {
                        questData.progress++;
                    }
                }

                FirebaseAnalyticsHelper.LogEvent("N_CompleteAllDailyQuests");
            }
        }

        scroller.RefreshActiveCellViews();
        RewardUtils.Receive(data.rewards);
        Popup.Instance.ShowToastMessage("Claim reward successfully");
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);
    }


    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return dailyQuestData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 98f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellViewDailyQuest cellView = scroller.GetCellView(cellViewDailyQuest) as CellViewDailyQuest;
        cellView.name = dailyQuestData[dataIndex].type.ToString();
        cellView.SetData(dailyQuestData[dataIndex]);
        return cellView;
    }

    #endregion
}
