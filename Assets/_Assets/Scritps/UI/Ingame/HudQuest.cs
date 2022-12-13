using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudQuest : MonoBehaviour
{
    public Text stageName;
    public Text[] descriptions;
    public Text[] progress;
    public GameObject[] completeTicks;

    private void Awake()
    {
        stageName.text = string.Format("Stage {0} - {1}", GameData.currentStage.id, GameData.currentStage.difficulty).ToUpper();
        LoadQuestDescription();
    }

    private void OnEnable()
    {
        LoadQuestProgress();
    }

    public void LoadQuestProgress()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    bool isCompleted = GameController.Instance.CampaignMap.quest.IsAlreadyCompleted(i);

        //    progress[i].text = GameController.Instance.CampaignMap.quest.GetProgress(i);
        //    progress[i].gameObject.SetActive(!isCompleted);

        //    completeTicks[i].SetActive(isCompleted);
        //}
    }

    private void LoadQuestDescription()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    descriptions[i].text = GameController.Instance.CampaignMap.quest.GetDescription(i);
        //}
    }
}
