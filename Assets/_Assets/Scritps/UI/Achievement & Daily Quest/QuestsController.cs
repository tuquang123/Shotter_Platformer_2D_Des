using UnityEngine;
using System.Collections;

public class QuestsController : MonoBehaviour
{
    public HudDailyQuest dailyQuest;
    public HudAchievement achievement;

    public QuestTab tabAchievement;
    public QuestTab tabDailyQuest;


    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ClaimDailyQuestReward, (sender, param) => CalculateNotiDailyQuest());
        EventDispatcher.Instance.RegisterListener(EventID.ClaimAchievementReward, (sender, param) => CalculateNotiAchievement());
    }

    private void OnEnable()
    {
        SwitchDailyQuest();
        CalculateNotiAchievement();
        CalculateNotiDailyQuest();
    }

    public void SwitchAchievement()
    {
        tabAchievement.Highlight(true);
        achievement.gameObject.SetActive(true);

        tabDailyQuest.Highlight(false);
        dailyQuest.gameObject.SetActive(false);

        SoundManager.Instance.PlaySfxClick();

        if (GameData.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepClickAchievement);
        }
    }

    public void SwitchDailyQuest()
    {
        tabAchievement.Highlight(false);
        achievement.gameObject.SetActive(false);

        tabDailyQuest.Highlight(true);
        dailyQuest.gameObject.SetActive(true);

        SoundManager.Instance.PlaySfxClick();
    }

    private void CalculateNotiAchievement()
    {
        achievement.CalculateNotification();
    }

    private void CalculateNotiDailyQuest()
    {
        dailyQuest.CalculateNotification();
    }
}
