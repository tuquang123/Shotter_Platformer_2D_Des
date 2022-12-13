using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellViewTournamentRankData
{
    public int indexRank;
    public Sprite sprRankIcon;
    public string rankName;
    public string playerName;
    public Sprite sprAvatar;
    public Sprite sprGunId;
    public int score;
    public List<RewardData> rewards;

    public void SetAvatar(Sprite spr)
    {
        sprAvatar = spr;
        EventDispatcher.Instance.PostEvent(EventID.GetFacebookAvatarDone);
    }

    public void SetUserInfo(UserInfo info)
    {
        if (info != null)
        {
            playerName = info.name;
            EventDispatcher.Instance.PostEvent(EventID.GetFacebookNameDone);
        }
    }
}
