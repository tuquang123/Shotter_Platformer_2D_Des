using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class _PlayerGunData : Dictionary<int, PlayerGunData>
{
    public void Save()
    {
        string s = JsonConvert.SerializeObject(this);

        ProfileManager.UserProfile.playerGunData.Set(s);
        ProfileManager.SaveAll();

        DebugCustom.Log("PlayerGunData=" + s);
    }

    public void ReceiveNewGun(int id)
    {
        if (GameData.staticGunData.ContainsKey(id))
        {
            if (ContainsKey(id))
            {
                if (GameData.gunValueGem.ContainsKey(id))
                {
                    int gemRefund = GameData.gunValueGem[id];
                    GameData.playerResources.ReceiveGem(gemRefund);
                    SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
                    DebugCustom.Log(string.Format("[ReceiveNewGun] Player guns key exist id={0}, refund={1}", id, gemRefund));
                }
                else
                {
                    PlayerGunData gun = this[id];
                    gun.level += 2;
                    gun.level = Mathf.Clamp(gun.level, 1, GameData.staticGunData[id].upgradeInfo.Length);
                    DebugCustom.LogError(string.Format("[ReceiveNewGun] ID not in JSON={0}, increase gun level", id));
                }
            }
            else
            {
                StaticGunData staticGunData = GameData.staticGunData.GetData(id);

                PlayerGunData gun = new PlayerGunData();
                gun.id = id;
                gun.level = 1;
                gun.isNew = true;

                if (staticGunData.isSpecialGun)
                {
                    SO_GunStats stats = GameData.staticGunData.GetBaseStats(id, gun.level);
                    gun.ammo = stats.Ammo;
                }
                else
                {
                    gun.ammo = 0;
                }

                Add(id, gun);
            }

            Save();
        }
        else
        {
            DebugCustom.LogError("[ReceiveNewGun] Key not found=" + id);
        }
    }

    public int GetGunAmmo(int id)
    {
        if (ContainsKey(id))
        {
            return this[id].ammo;
        }
        else
        {
            DebugCustom.Log("[GetGunAmmo] Key not found=" + id);
            return 0;
        }
    }

    public void SetGunAmmo(int id, int ammo)
    {
        if (ContainsKey(id))
        {
            this[id].ammo = ammo;
            Save();
        }
        else
        {
            DebugCustom.Log("[SetGunAmmo] Key not found=" + id);
        }
    }

    public int GetGunLevel(int id)
    {
        int level = 1;

        if (ContainsKey(id))
        {
            level = this[id].level;
        }
        else
        {
            DebugCustom.LogError("[GetGunLevel] Key not found=" + id);
        }

        return level;
    }

    public void IncreaseGunLevel(int id)
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
            DebugCustom.Log("[IncreaseGunLevel] Key not found=" + id);
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

    public int GetNumberOfNormalGun()
    {
        int count = 0;

        foreach (PlayerGunData gun in Values)
        {
            StaticGunData staticData = GameData.staticGunData.GetData(gun.id);

            if (staticData != null && staticData.isSpecialGun == false)
            {
                count++;
            }
        }

        return count;
    }

    public int GetNumberOfSpecialGun()
    {
        int count = 0;

        foreach (PlayerGunData gun in Values)
        {
            StaticGunData staticData = GameData.staticGunData.GetData(gun.id);

            if (staticData != null && staticData.isSpecialGun)
            {
                count++;
            }
        }

        return count;
    }
}
