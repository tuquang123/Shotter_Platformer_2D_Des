using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerBoosterData : Dictionary<BoosterType, int>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerBoosterData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerBoosterData=" + s);
    }

    public int GetQuantityHave(BoosterType type)
    {
        if (ContainsKey(type))
        {
            return this[type];
        }
        else
        {
            DebugCustom.LogError("[BoosterGetQuantityHave] Key not found=" + type);
            return 0;
        }
    }

    public void Receive(BoosterType type, int value)
    {
        if (ContainsKey(type))
        {
            this[type] += value;
        }
        else
        {
            DebugCustom.LogError("[ReceiveBooster] Key not found=" + type);
            Add(type, value);
        }

        Save();
    }

    public void Consume(BoosterType type, int value)
    {
        if (ContainsKey(type))
        {
            this[type] -= value;

            if (this[type] < 0)
                this[type] = 0;
        }
        else
        {
            DebugCustom.LogError("[ConsumeBooster] Key not found=" + type);
        }

        Save();
    }
}
