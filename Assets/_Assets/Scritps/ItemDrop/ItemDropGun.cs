using UnityEngine;
using System.Collections;

public class ItemDropGun : BaseItemDrop
{
    public SpriteRenderer gunImage;

    protected override void Awake()
    {
        base.Awake();

        EventDispatcher.Instance.RegisterListener(EventID.CompleteWave, (sender, param) =>
        {
            if (gameObject.activeInHierarchy)
                Deactive();
        });
    }

    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolItemDropGun.Store(this);
    }

    public override void Active(ItemDropData data, Vector2 position)
    {
        base.Active(data, position);

        switch (data.type)
        {
            case ItemDropType.GunSpread:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_SPREAD);
                break;

            case ItemDropType.GunRocketChaser:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_ROCKET_CHASER);
                break;

            case ItemDropType.GunFamas:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_FAMAS);
                break;

            case ItemDropType.GunLaser:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_LASER);
                break;

            case ItemDropType.GunSplit:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_SPLIT);
                break;

            case ItemDropType.GunFireBall:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_FIRE_BALL);
                break;

            case ItemDropType.GunTesla:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_TESLA);
                break;

            case ItemDropType.GunKamePower:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_KAME_POWER);
                break;

            case ItemDropType.GunFlame:
                gunImage.sprite = GameResourcesUtils.GetGunImage(StaticValue.GUN_ID_FLAME);
                break;
        }
    }
}
