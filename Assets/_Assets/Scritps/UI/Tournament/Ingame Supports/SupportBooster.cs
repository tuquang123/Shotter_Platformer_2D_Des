using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SupportBooster : BaseSupportItem
{
    public Button boosterImage;

    private BoosterType type = BoosterType.Damage;
    private bool isUsedCurrentWave;

    public override void Init()
    {
        base.Init();

        groupFree.SetActive(true);
        groupPrice.SetActive(false);
        priceUse = 0;
        isUsedCurrentWave = false;

        RandomBooster();
    }

    protected override void Consume()
    {
        if (isUsedCurrentWave)
            return;

        if (GameData.playerResources.gem >= priceUse)
        {
            isUsedCurrentWave = true;

            GameData.playerResources.ConsumeGem(priceUse);

            base.Consume();

            if (countUsed >= 5)
            {
                Active(false);
            }
            else if (countUsed >= 1)
            {
                groupFree.SetActive(false);
                groupPrice.SetActive(true);
                priceUse = StaticValue.COST_SUPPORT_ITEM_BOMB;
                textPrice.text = priceUse.ToString();
                textPrice.color = GameData.playerResources.gem >= priceUse ? Color.yellow : StaticValue.colorNotEnoughMoney;
            }

            EventDispatcher.Instance.PostEvent(EventID.UseSupportItemBooster, type);

            FirebaseAnalyticsHelper.LogEvent("N_UseSurvivalSupportItem", "Booster");
        }
    }

    protected override void Active(bool isActive)
    {
        base.Active(isActive);

        boosterImage.image.raycastTarget = false;
        boosterImage.interactable = isActive;
    }

    protected override void OnCompleteWave()
    {
        isUsedCurrentWave = false;

        if (countUsed < 5)
        {
            RandomBooster();
        }
    }

    private void RandomBooster()
    {
        int random = Random.Range(0, 3);

        if (random == 0)
        {
            type = BoosterType.Damage;
            boosterImage.image.sprite = GameResourcesUtils.GetRewardImage(RewardType.BoosterDamage);
        }
        else if (random == 1)
        {
            type = BoosterType.Critical;
            boosterImage.image.sprite = GameResourcesUtils.GetRewardImage(RewardType.BoosterCritical);
        }
        else if (random == 2)
        {
            type = BoosterType.Speed;
            boosterImage.image.sprite = GameResourcesUtils.GetRewardImage(RewardType.BoosterSpeed);
        }

        boosterImage.image.SetNativeSize();
    }
}
