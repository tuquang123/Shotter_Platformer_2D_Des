using UnityEngine;
using System.Collections;

public class SupportRestoreHp : BaseSupportItem
{

    public override void Init()
    {
        base.Init();

        groupFree.SetActive(true);
        groupPrice.SetActive(false);
        priceUse = 0;
    }

    protected override void Consume()
    {
        if (GameData.playerResources.gem >= priceUse)
        {
            GameData.playerResources.ConsumeGem(priceUse);

            base.Consume();

            if (countUsed >= 2)
            {
                Active(false);
            }
            else if (countUsed >= 1)
            {
                groupFree.SetActive(false);
                groupPrice.SetActive(true);
                priceUse = StaticValue.COST_SUPPORT_ITEM_HP;
                textPrice.text = priceUse.ToString();
                textPrice.color = GameData.playerResources.gem >= priceUse ? Color.yellow : StaticValue.colorNotEnoughMoney;
            }

            EventDispatcher.Instance.PostEvent(EventID.UseSupportItemHP);

            FirebaseAnalyticsHelper.LogEvent("N_UseSurvivalSupportItem", "FullHP");
        }
    }
}
