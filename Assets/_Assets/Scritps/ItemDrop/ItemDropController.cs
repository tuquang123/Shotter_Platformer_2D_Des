using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDropController : MonoBehaviour
{
    public ItemDropHealth itemHealthPrefab;
    public ItemDropCoin itemCoinPrefab;
    public ItemDropGun itemDropGunPrefab;

    public void Spawn(List<ItemDropData> items, Vector2 position, BaseEnemy unitDrop = null)
    {
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ItemDropData item = items[i];

                if (item.type == ItemDropType.Health)
                {
                    SpawnHealth(item, position);
                }
                else if (item.type == ItemDropType.Coin)
                {
                    SpawnCoin(item, position);
                }
                else if (item.type == ItemDropType.Ammo)
                {

                }
                else
                {
                    bool isSurvivalMode = GameData.mode == GameMode.Survival;
                    bool isCampaignNormal = GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Normal;

                    if (isSurvivalMode || isCampaignNormal)
                        SpawnGun(item, position);
                }
            }
        }
    }

    private void SpawnHealth(ItemDropData data, Vector2 position)
    {
        ItemDropHealth health = PoolingController.Instance.poolItemDropHealth.New();

        if (health == null)
        {
            health = Instantiate(itemHealthPrefab);
        }

        health.Active(data, position);
    }

    private void SpawnCoin(ItemDropData data, Vector2 position)
    {
        int numberCoinSpawn = Random.Range(2, 5);

        if (numberCoinSpawn > data.value)
            numberCoinSpawn = (int)data.value;

        int perCoinValue = Mathf.RoundToInt(data.value / numberCoinSpawn);

        for (int i = 0; i < numberCoinSpawn; i++)
        {
            ItemDropCoin coin = PoolingController.Instance.poolItemDropCoin.New();

            if (coin == null)
            {
                coin = Instantiate(itemCoinPrefab);
            }

            ItemDropData perCoinData = new ItemDropData(data.type, perCoinValue);
            coin.Active(perCoinData, position);
        }
    }

    private void SpawnGun(ItemDropData data, Vector2 position)
    {
        ItemDropGun gun = PoolingController.Instance.poolItemDropGun.New();

        if (gun == null)
        {
            gun = Instantiate(itemDropGunPrefab);
        }

        gun.Active(data, position);
    }
}
