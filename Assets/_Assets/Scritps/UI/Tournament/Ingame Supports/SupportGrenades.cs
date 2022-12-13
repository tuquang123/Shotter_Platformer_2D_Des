using UnityEngine;
using System.Collections;

public class SupportGrenades : BaseSupportItem
{

    public override void Init()
    {
        base.Init();

        groupFree.SetActive(false);
        groupPrice.SetActive(true);
        priceUse = StaticValue.COST_SUPPORT_ITEM_GRENADE;
        textPrice.text = priceUse.ToString();
        textPrice.color = GameData.playerResources.coin >= priceUse ? Color.yellow : StaticValue.colorNotEnoughMoney;
    }

    protected override void Consume()
    {
        if (GameData.playerResources.coin >= priceUse)
        {
            GameData.playerResources.ConsumeCoin(priceUse);
            GameData.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, StaticValue.SUPPORT_ITEM_GRENADE_INCREASE);

            base.Consume();

            if (countUsed >= 2)
            {
                Active(false);
            }
            else
            {
                textPrice.color = GameData.playerResources.coin >= priceUse ? Color.yellow : StaticValue.colorNotEnoughMoney;
            }

            EventDispatcher.Instance.PostEvent(EventID.UseSupportItemGrenade, StaticValue.SUPPORT_ITEM_GRENADE_INCREASE);

            FirebaseAnalyticsHelper.LogEvent("N_UseSurvivalSupportItem", "Grenades");
        }
    }
}
