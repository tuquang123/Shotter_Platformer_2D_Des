using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;

public class CellViewDailyQuest : EnhancedScrollerCellView
{
    public Text textTitle;
    public Text textDescription;
    public Text textProgress;
    public Text textTarget;
    public Image imageProgress;
    public Button btnClaim;
    public GameObject labelAchieved;
    public RewardElement[] rewardCells;

    private CellViewDailyQuestData _data;


    public void SetData(CellViewDailyQuestData data)
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

        labelAchieved.SetActive(_data.isClaimed);

        if (_data.isClaimed)
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
        _data.isClaimed = true;

        for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
        {
            PlayerDailyQuestData playerQuest = GameData.playerDailyQuests[i];

            if (playerQuest.type == _data.type)
            {
                playerQuest.isClaimed = true;
                break;
            }
        }

        GameData.playerDailyQuests.Save();

        EventDispatcher.Instance.PostEvent(EventID.ClaimDailyQuestReward, _data);
    }
}
