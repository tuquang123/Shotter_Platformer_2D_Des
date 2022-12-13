using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RewardUtils
{
    public static void Receive(List<RewardData> rewards)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            RewardData reward = rewards[i];

            switch (reward.type)
            {
                case RewardType.Coin:
                    GameData.playerResources.ReceiveCoin(reward.value);
                    break;

                case RewardType.Gem:
                    GameData.playerResources.ReceiveGem(reward.value);
                    break;

                case RewardType.Stamina:
                    GameData.playerResources.ReceiveStamina(reward.value);
                    break;

                case RewardType.Medal:
                    GameData.playerResources.ReceiveMedal(reward.value);
                    break;

                case RewardType.TournamentTicket:
                    GameData.playerResources.ReceiveTournamentTicket(reward.value);
                    break;

                case RewardType.Exp:
                    GameData.playerProfile.ReceiveExp(reward.value);
                    break;

                case RewardType.BoosterHp:
                    GameData.playerBoosters.Receive(BoosterType.Hp, reward.value);
                    break;

                case RewardType.BoosterDamage:
                    GameData.playerBoosters.Receive(BoosterType.Damage, reward.value);
                    break;

                case RewardType.BoosterCoinMagnet:
                    GameData.playerBoosters.Receive(BoosterType.CoinMagnet, reward.value);
                    break;

                case RewardType.BoosterSpeed:
                    GameData.playerBoosters.Receive(BoosterType.Speed, reward.value);
                    break;

                case RewardType.BoosterCritical:
                    GameData.playerBoosters.Receive(BoosterType.Critical, reward.value);
                    break;

                case RewardType.GunM4:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_M4))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_M4];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_M4);
                    }
                    break;

                case RewardType.GunSpread:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_SPREAD))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_SPREAD];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_SPREAD);
                    }
                    break;

                case RewardType.GunScarH:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_SCAR_H))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_SCAR_H];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_SCAR_H);
                    }
                    break;

                case RewardType.GunBullpup:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_BULLPUP))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_BULLPUP];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_BULLPUP);
                    }
                    break;

                case RewardType.GunKamePower:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_KAME_POWER))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_KAME_POWER];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_KAME_POWER);
                    }
                    break;

                case RewardType.GunSniperRifle:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_SNIPER_RIFLE))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_SNIPER_RIFLE];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_SNIPER_RIFLE);
                    }
                    break;

                case RewardType.GunTeslaMini:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_TESLA_MINI))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_TESLA_MINI];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_TESLA_MINI);
                    }
                    break;

                case RewardType.GunLaser:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_LASER))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_LASER];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_LASER);
                    }
                    break;

                case RewardType.GunFlame:
                    if (GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_FLAME))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameData.gunValueGem[StaticValue.GUN_ID_FLAME];
                        GameData.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameData.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_FLAME);
                    }
                    break;

                case RewardType.MeleeWeaponPan:
                    GameData.playerMeleeWeapons.ReceiveNewMeleeWeapon(StaticValue.MELEE_WEAPON_ID_PAN);
                    break;

                case RewardType.MeleeWeaponGuitar:
                    GameData.playerMeleeWeapons.ReceiveNewMeleeWeapon(StaticValue.MELEE_WEAPON_ID_GUITAR);
                    break;

                case RewardType.GrenadeF1:
                    GameData.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, reward.value);
                    break;

                case RewardType.GrenadeTet:
                    GameData.playerGrenades.Receive(StaticValue.GRENADE_ID_TET_HOLIDAY, reward.value);
                    break;
            }
        }
    }
}
