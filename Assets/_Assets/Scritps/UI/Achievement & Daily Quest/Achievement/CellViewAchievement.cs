using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;

public class CellViewAchievement : EnhancedScrollerCellView
{
    public Text textTitle;
    public Text textDescription;
    public Text textProgress;
    public Text textTarget;
    public Image imageProgress;
    public Button btnClaim;
    public GameObject labelAchieved;
    public RewardElement[] rewardCells;

    private CellViewAchievementData _data;


    public void SetData(CellViewAchievementData data)
    {
        _data = data;

        UpdateInformation();
    }

    private void UpdateInformation()
    {
        textTitle.text = _data.title.ToUpper();
        textDescription.text = _data.description;

        for (int i = 0; i < rewardCells.Length; i++)
        {
            RewardElement cell = rewardCells[i];

            cell.gameObject.SetActive(i < _data.rewards.Count);

            if (i < _data.rewards.Count)
            {
                RewardData rw = _data.rewards[i];
                cell.SetInformation(rw);
            }
        }

        labelAchieved.SetActive(_data.isCompleted);

        if (_data.isCompleted)
        {
            btnClaim.gameObject.SetActive(false);
            imageProgress.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            if (_data.progress >= _data.target)
            {
                btnClaim.gameObject.SetActive(true);
                imageProgress.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                btnClaim.gameObject.SetActive(false);
                imageProgress.transform.parent.gameObject.SetActive(true);

                imageProgress.fillAmount = Mathf.Clamp01((float)_data.progress / (float)_data.target);
                textProgress.text = _data.progress.ToString("n0");
                textTarget.text = _data.target.ToString("n0");
            }
        }
    }

    public override void RefreshCellView()
    {
        base.RefreshCellView();

        UpdateInformation();
    }

    public void ClaimReward()
    {
        StaticAchievementData staticData = GameData.staticAchievementData.GetData(_data.type);

        int curMilestoneIndex = GameData.playerAchievements.ContainsKey(_data.type) ?
                GameData.playerAchievements[_data.type].claimTimes : 0;

        if (curMilestoneIndex >= staticData.milestones.Count - 1)
        {
            _data.isCompleted = true;
        }
        else
        {
            curMilestoneIndex++;
            AchievementMilestone milestone = staticData.milestones[curMilestoneIndex];
            _data.description = string.Format(staticData.description, milestone.requirement.ToString("n0"));
            _data.progress = GameData.playerAchievements.ContainsKey(staticData.type) ?
                GameData.playerAchievements[staticData.type].progress : 0;
            _data.target = milestone.requirement;
            _data.rewards = milestone.rewards;
            _data.isCompleted = false;
        }

        if (GameData.playerAchievements.ContainsKey(_data.type))
        {
            GameData.playerAchievements[_data.type].claimTimes++;
            GameData.playerAchievements.Save();
        }
        else
        {
            DebugCustom.LogError("[ClaimReward] PlayerAchievement key not found=" + _data.type);
        }

        EventDispatcher.Instance.PostEvent(EventID.ClaimAchievementReward, _data);
    }
}
