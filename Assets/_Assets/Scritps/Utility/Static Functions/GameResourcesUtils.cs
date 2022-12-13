using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameResourcesUtils
{

    #region Prefabs

    public static Rambo GetRamboPrefab(int id)
    {
        string resourcesName = string.Format("rambo_{0}", id);

        return Resources.Load<Rambo>(StaticValue.PATH_RAMBO_PREFAB + resourcesName);
    }

    public static BaseGun GetGunPrefab(int id)
    {
        string resourcesName = string.Format("gun_{0}", id);

        return Resources.Load<BaseGun>(StaticValue.PATH_GUN_PREFAB + resourcesName);
    }

    public static BaseGrenade GetGrenadePrefab(int id)
    {
        string resourcesName = string.Format("grenade_{0}", id);

        return Resources.Load<BaseGrenade>(StaticValue.PATH_GRENADE_PREFAB + resourcesName);
    }

    public static BaseMeleeWeapon GetMeleeWeaponPrefab(int id)
    {
        string resourcesName = string.Format("melee_weapon_{0}", id);

        return Resources.Load<BaseMeleeWeapon>(StaticValue.PATH_MELEE_WEAPON_PREFAB + resourcesName);
    }

    #endregion


    #region Images

    private static Dictionary<int, Sprite> gunImages = new Dictionary<int, Sprite>();
    private static Dictionary<int, Sprite> grenadeImages = new Dictionary<int, Sprite>();
    private static Dictionary<int, Sprite> meleeWeaponImages = new Dictionary<int, Sprite>();
    private static Dictionary<int, Sprite> rankImages = new Dictionary<int, Sprite>();
    private static Dictionary<int, Sprite> tournamentRankImages = new Dictionary<int, Sprite>();
    private static Dictionary<string, Sprite> enemyImages = new Dictionary<string, Sprite>();
    private static Dictionary<int, Sprite> skillLockImages = new Dictionary<int, Sprite>();
    private static Dictionary<int, Sprite> skillUnlockImages = new Dictionary<int, Sprite>();
    private static Dictionary<RewardType, Sprite> rewardImages = new Dictionary<RewardType, Sprite>();


    public static Sprite GetGunImage(int id)
    {
        if (gunImages.ContainsKey(id))
        {
            return gunImages[id];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_GUN + id);

            if (sprite != null)
            {
                gunImages.Add(id, sprite);
                return sprite;
            }

            return null;
        }
    }

    public static Sprite GetGrenadeImage(int id)
    {
        if (grenadeImages.ContainsKey(id))
        {
            return grenadeImages[id];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_GRENADE + id);

            if (sprite != null)
            {
                grenadeImages.Add(id, sprite);
                return sprite;
            }

            return null;
        }
    }

    public static Sprite GetMeleeWeaponImage(int id)
    {
        if (meleeWeaponImages.ContainsKey(id))
        {
            return meleeWeaponImages[id];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_MELEE_WEAPON + id);

            if (sprite != null)
            {
                meleeWeaponImages.Add(id, sprite);
                return sprite;
            }

            return null;
        }
    }

    public static Sprite GetEnemyImage(int id, MapType map)
    {

        string idDefault = id.ToString();
        string idWithMap = string.Format("{0}-{1}", id, (int)map);

        if (enemyImages.ContainsKey(idWithMap))
        {
            return enemyImages[idWithMap];
        }
        else
        {
            Sprite spr = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_ENEMY_ICON + idWithMap);

            if (spr != null)
            {
                enemyImages.Add(idWithMap, spr);
                return spr;
            }
            else
            {
                spr = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_ENEMY_ICON + idDefault);

                if (spr != null)
                {
                    enemyImages.Add(idWithMap, spr);
                    return spr;
                }
                else
                {
                    DebugCustom.Log(string.Format("Enemy {0} has no icon in path {1}", id, StaticValue.PATH_IMAGE_ENEMY_ICON));
                    return null;
                }
            }
        }
    }

    public static Sprite GetRewardImage(RewardType type)
    {
        if (rewardImages.ContainsKey(type))
        {
            return rewardImages[type];
        }
        else
        {
            Sprite spr = null;

            switch (type)
            {
                case RewardType.Coin:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_coin");
                    break;

                case RewardType.Gem:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gem");
                    break;

                case RewardType.TournamentTicket:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_tournament_ticket");
                    break;

                case RewardType.Exp:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_exp");
                    break;

                case RewardType.Medal:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_medal");
                    break;

                case RewardType.BoosterHp:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_hp");
                    break;

                case RewardType.BoosterDamage:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_damage");
                    break;

                case RewardType.BoosterCritical:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_critical");
                    break;

                case RewardType.BoosterSpeed:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_speed");
                    break;

                case RewardType.BoosterCoinMagnet:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_coin_magnet");
                    break;

                case RewardType.GunScarH:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_scar_H");
                    break;

                case RewardType.GunM4:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_m4");
                    break;

                case RewardType.GunSpread:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_spread");
                    break;

                case RewardType.GunBullpup:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_bullpup");
                    break;

                case RewardType.GunKamePower:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_kame_power");
                    break;

                case RewardType.GunTeslaMini:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_tesla_mini");
                    break;

                case RewardType.GunLaser:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_laser");
                    break;

                case RewardType.GunFlame:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_flame");
                    break;

                case RewardType.GunSniperRifle:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gun_sniper_rifle");
                    break;

                case RewardType.MeleeWeaponPan:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_melee_weapon_pan");
                    break;

                case RewardType.MeleeWeaponGuitar:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_melee_weapon_guitar");
                    break;

                case RewardType.GrenadeF1:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_grenade_f1");
                    break;

                case RewardType.GrenadeTet:
                    spr = Resources.Load<Sprite>("Sprites/Reward Icon/reward_grenade_tet");
                    break;
            }

            rewardImages.Add(type, spr);

            return spr;
        }
    }

    public static Sprite GetRankImage(int level)
    {
        if (rankImages.ContainsKey(level))
        {
            return rankImages[level];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_RANK_ICON + level);

            if (sprite != null)
            {
                rankImages.Add(level, sprite);
                return sprite;
            }

            return null;
        }
    }

    public static Sprite GetTournamentRankImage(int index)
    {
        if (tournamentRankImages.ContainsKey(index))
        {
            return tournamentRankImages[index];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_TOURNAMENT_RANK_ICON + index);

            if (sprite != null)
            {
                tournamentRankImages.Add(index, sprite);
                return sprite;
            }

            return null;
        }
    }

    public static Sprite GetSkillLockImage(int id)
    {
        if (skillLockImages.ContainsKey(id))
        {
            return skillLockImages[id];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_SKILL_LOCK + id);

            if (sprite != null)
            {
                skillLockImages.Add(id, sprite);
                return sprite;
            }

            return null;
        }
    }

    public static Sprite GetSkillUnlockImage(int id)
    {
        if (skillUnlockImages.ContainsKey(id))
        {
            return skillUnlockImages[id];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(StaticValue.PATH_IMAGE_SKILL_UNLOCK + id);

            if (sprite != null)
            {
                skillUnlockImages.Add(id, sprite);
                return sprite;
            }

            return null;
        }
    }

    #endregion
}
