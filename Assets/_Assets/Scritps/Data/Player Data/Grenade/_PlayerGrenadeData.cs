using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerGrenadeData : Dictionary<int, PlayerGrenadeData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerGrenadeData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerGrenadeData=" + s);
    }

    public int GetGrenadeLevel(int id)
    {
        int level = 1;

        if (ContainsKey(id))
        {
            level = this[id].level;
        }
        else
        {
            DebugCustom.LogError("[GetGrenadeLevel] Key not found=" + id);
        }

        return level;
    }

    public int GetQuantityHave(int id)
    {
        int quantity = 0;

        if (ContainsKey(id))
        {
            quantity = this[id].quantity;
        }
        else
        {
            DebugCustom.LogError("[GetQuantityHave] Dont have grenade id=" + id);
        }

        return quantity;
    }

    public void Receive(int id, int quantity)
    {
        if (ContainsKey(id))
        {
            this[id].quantity += quantity;
        }
        else
        {
            if (GameData.staticGrenadeData.ContainsKey(id))
            {
                PlayerGrenadeData newGrenade = new PlayerGrenadeData(id, quantity: quantity);
                newGrenade.isNew = true;
                Add(id, newGrenade);
            }
            else
            {
                DebugCustom.LogError("[ReceiveGrenade] Invalid id=" + id);
            }
        }

        Save();
    }

    public void Consume(int id, int quantity)
    {
        if (ContainsKey(id))
        {
            this[id].quantity -= quantity;

            if (this[id].quantity < 0)
                this[id].quantity = 0;
        }
        else
        {
            DebugCustom.LogError("[ConsumeGrenade] Key not found=" + id);
        }

        Save();
    }

    public void RemoveGrenade(int id)
    {
        if (ContainsKey(id))
        {
            this.Remove(id);
            Save();
        }
        else
        {
            DebugCustom.LogError("[RemoveGrenade] Invalid id=" + id);
        }
    }

    public void IncreaseGrenadeLevel(int id)
    {
        if (ContainsKey(id))
        {
            int level = this[id].level;
            level++;
            this[id].level = level;
            Save();
        }
        else
        {
            DebugCustom.Log("[IncreaseGrenadeLevel] Key not found=" + id);
        }
    }

    public void SetNew(int id, bool isNew)
    {
        if (ContainsKey(id))
        {
            this[id].isNew = isNew;
            Save();
        }
        else
        {
            DebugCustom.Log("[SetNew] Key not found=" + id);
        }
    }
}
