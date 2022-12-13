using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameResultQuestInfo : MonoBehaviour
{
    //public HorizontalScrollText description;
    public Text description;
    public Text progress;
    public Text gem;
    public Text coin;
    public Text exp;
    public Image star;
    public Sprite starComplete;
    public Sprite starIncomplete;
    public Color32 colorTextComplete;
    public Color32 colorTextIncomplete;

    public void ShowInfo(int index)
    {
        //description.text = GameController.Instance.CampaignMap.quest.GetDescription(index).ToUpper();
        ////description.Active(s);

        //progress.text = GameController.Instance.CampaignMap.quest.GetProgress(index);

        //if (MapUtils.IsAlreadyCompleteQuest(GameData.currentStage, index))
        //{
        //    coin.gameObject.SetActive(false);
        //    gem.gameObject.SetActive(false);
        //    exp.gameObject.SetActive(false);
        //}
        //else
        //{
        //    if (GameController.Instance.CampaignMap.quest.result[index])
        //    {
        //        coin.gameObject.SetActive(true);
        //        int coinReward = MapUtils.GetCoinCompleteQuest(GameData.currentStage.id, GameData.currentStage.difficulty, index);
        //        coin.text = coinReward.ToString("n0");
        //        EventDispatcher.Instance.PostEvent(EventID.GetCoinCompleteQuest, coinReward);

        //        gem.gameObject.SetActive(true);
        //        int gemReward = MapUtils.GetGemCompleteQuest(GameData.currentStage.id, GameData.currentStage.difficulty, index);
        //        gem.text = gemReward.ToString("n0");
        //        EventDispatcher.Instance.PostEvent(EventID.GetGemCompleteQuest, gemReward);

        //        exp.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        coin.gameObject.SetActive(false);
        //        gem.gameObject.SetActive(false);
        //        exp.gameObject.SetActive(false);
        //    }
        //}

        //bool isComplete = GameController.Instance.CampaignMap.quest.result[index];

        //progress.gameObject.SetActive(!isComplete);
        //star.sprite = isComplete ? starComplete : starIncomplete;
        //star.SetNativeSize();
        //description.color = isComplete ? colorTextComplete : colorTextIncomplete;
    }
}
