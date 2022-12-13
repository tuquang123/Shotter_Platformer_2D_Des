using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class _PlayerProfile
{
    public int level;
    public int exp;

    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerProfile.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerProfile=" + s);
    }

    public void ReceiveExp(int value)
    {
        if (value > 0)
        {
            exp += value;

            int curLevel = GameData.staticRankData.GetLevelByExp(exp);

            if (curLevel > level)
            {
                level = curLevel;
                EventDispatcher.Instance.PostEvent(EventID.LevelUp, value);

                StaticRankData newRankData = GameData.staticRankData.GetData(level);
                string msg = string.Format("RANK UP TO LEVEL {0}\n<color=yellow>{1}</color>",
                    newRankData.level, GameData.staticRankData.GetRankName(newRankData.level));
                List<RewardData> rewards = newRankData.rewards;
                Popup.Instance.ShowReward(rewards, msg);

                //FirebaseAnalyticsHelper.LogEvent("LevelUp", newRankData.level);
            }

            Save();
            EventDispatcher.Instance.PostEvent(EventID.ReceiveExp, value);
        }
    }
}
