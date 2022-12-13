using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RewardElement : MonoBehaviour
{
    public Image icon;
    public TMP_Text value;

    public void SetInformation(RewardData data, bool isBonus = false)
    {
        icon.sprite = GameResourcesUtils.GetRewardImage(data.type);
        //icon.SetNativeSize();
         
        //if (data.type == RewardType.Exp)
        //{
//        value.text = string.Format("{0:n0}", data.value);
        //}
        //else
        //{
        //    value.text = string.Format("x{0:n0}", data.value);
        //}
    }
}
