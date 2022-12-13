using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerMeleeWeaponData : Dictionary<int, PlayerMeleeWeaponData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerMeleeWeaponData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerMeleeWeaponData=" + s);
    }

    public void ReceiveNewMeleeWeapon(int id)
    {
        if (GameData.staticMeleeWeaponData.ContainsKey(id))
        {
            if (ContainsKey(id))
            {
                DebugCustom.Log(string.Format("[ReceiveNewMeleeWeapon] Player melee weapons key exist id={0}, increase level", id));
                PlayerMeleeWeaponData weapon = this[id];
                weapon.level += 2;
                weapon.level = Mathf.Clamp(weapon.level, 1, GameData.staticMeleeWeaponData[id].upgradeInfo.Length);
            }
            else
            {
                StaticMeleeWeaponData staticData = GameData.staticMeleeWeaponData.GetData(id);

                PlayerMeleeWeaponData weapon = new PlayerMeleeWeaponData();
                weapon.id = id;
                weapon.level = 1;
                weapon.isNew = true;

                Add(id, weapon);
            }

            Save();
        }
        else
        {
            DebugCustom.LogError("[ReceiveNewMeleeWeapon] Key not found=" + id);
        }
    }

    public int GetMeleeWeaponLevel(int id)
    {
        int level = 1;

        if (ContainsKey(id))
        {
            level = this[id].level;
        }
        else
        {
            DebugCustom.LogError("[GetMeleeWeaponLevel] Key not found=" + id);
        }

        return level;
    }

    public void IncreaseMeleeWeaponLevel(int id)
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
            DebugCustom.Log("[IncreaseMeleeWeaponLevel] Key not found=" + id);
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
