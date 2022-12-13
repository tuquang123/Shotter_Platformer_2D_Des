using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TournamentRankRewardDisplay : MonoBehaviour
{
    public Image icon;
    public Text value;

    public void SetInformation(RewardData data)
    {
        icon.sprite = GameResourcesUtils.GetRewardImage(data.type);
        //icon.SetNativeSize();
        value.text = string.Format("{0:n0}", data.value);
    }
}
