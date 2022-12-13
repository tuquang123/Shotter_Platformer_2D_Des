using UnityEngine;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using UnityEngine.UI;

public class HudAchievement : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewAchievement;

    public Text countNotification;

    private SmallList<CellViewAchievementData> achievementData = new SmallList<CellViewAchievementData>();


    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClaimAchievementReward, (sender, param) => OnClaimReward((CellViewAchievementData)param));

        scroller.CreateContainer();
        scroller.Delegate = this;
    }

    private void OnEnable()
    {
        GameData.staticAchievementData.SortByState();
        CreateAchievementData();
        scroller.ReloadData();
    }

    public void CalculateNotification()
    {
        int count = GameData.playerAchievements.GetNumberReadyAchievement();

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

    private void CreateAchievementData()
    {
        achievementData.Clear();

        for (int i = 0; i < GameData.staticAchievementData.Count; i++)
        {
            StaticAchievementData staticData = GameData.staticAchievementData[i];

            int curMilestoneIndex = 0;

            if (GameData.playerAchievements.ContainsKey(staticData.type))
            {
                curMilestoneIndex = Mathf.Clamp(GameData.playerAchievements[staticData.type].claimTimes, 0, staticData.milestones.Count - 1);
            }

            AchievementMilestone milestone = staticData.milestones[curMilestoneIndex];

            CellViewAchievementData cell = new CellViewAchievementData();
            cell.type = staticData.type;
            cell.title = staticData.title.ToUpper();
            cell.description = string.Format(staticData.description, milestone.requirement.ToString("n0"));

            cell.progress = GameData.playerAchievements.ContainsKey(staticData.type) ?
                GameData.playerAchievements[staticData.type].progress : 0;

            cell.target = milestone.requirement;
            cell.rewards = milestone.rewards;
            cell.isCompleted = staticData.isCompleted;

            //cell.isCompleted = GameData.playerAchievements.ContainsKey(staticData.type) ?
            //    GameData.playerAchievements[staticData.type].claimTimes >= staticData.milestones.Count : false;

            achievementData.Add(cell);
        }
    }

    private void OnClaimReward(CellViewAchievementData data)
    {
        scroller.RefreshActiveCellViews();
        RewardUtils.Receive(data.rewards);
        Popup.Instance.ShowToastMessage("Claim reward successfully");
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);
    }


    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return achievementData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 98f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellViewAchievement cellView = scroller.GetCellView(cellViewAchievement) as CellViewAchievement;
        cellView.name = achievementData[dataIndex].type.ToString();
        cellView.SetData(achievementData[dataIndex]);
        return cellView;
    }

    #endregion
}
