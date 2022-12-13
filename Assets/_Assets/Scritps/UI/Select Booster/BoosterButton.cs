using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoosterButton : MonoBehaviour
{
    public BoosterType type;
    public Button selectButton;
    public Text textPrice;
    public Text textRemaining;
    public GameObject highlight;
    public Image labelEquip;
    public Sprite sprEquip;
    public Sprite sprUnequip;

    private StaticBoosterData data;

    void Awake()
    {
        data = GameData.staticBoosterData.GetData(type);

        EventDispatcher.Instance.RegisterListener(EventID.ConsumeCoin, (sender, param) => SetPriceTextColor());

        Load();
    }

    private void Load()
    {
        int remainingQuantity = GameData.playerBoosters.GetQuantityHave(type);

        if (type != BoosterType.Grenade)
        {
            textRemaining.text = remainingQuantity.ToString();
            textRemaining.color = remainingQuantity > 0 ? Color.white : StaticValue.colorNotEnoughMoney;
            labelEquip.transform.parent.gameObject.SetActive(remainingQuantity > 0);

            if (remainingQuantity <= 0 && GameData.selectingBoosters.Contains(type))
            {
                GameData.selectingBoosters.Remove(type);
            }
        }
        else
        {
            textRemaining.text = string.Empty;
        }

        textPrice.text = data.price.ToString("n0");
        SetPriceTextColor();
        Highlight();
    }

    public void Select()
    {
        SoundManager.Instance.PlaySfxClick();

        if (type != BoosterType.Grenade)
        {
            int remainingQuantity = GameData.playerBoosters.GetQuantityHave(type);
            if (remainingQuantity > 0)
            {
                if (GameData.selectingBoosters.Contains(type))
                {
                    GameData.selectingBoosters.Remove(type);
                }
                else
                {
                    GameData.selectingBoosters.Add(type);
                }

                Highlight();
            }
        }

        EventDispatcher.Instance.PostEvent(EventID.SelectBooster, data);

        if (GameData.isShowingTutorial && type == BoosterType.CoinMagnet)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepSelectBooster);
        }
    }

    public void Buy()
    {
        if (GameData.playerResources.coin < data.price)
        {
            SoundManager.Instance.PlaySfxClick();
            return;
        }

        if (type == BoosterType.Grenade)
        {
            GameData.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, 1);
            EventDispatcher.Instance.PostEvent(EventID.BuyGrenade);
        }
        else
        {
            GameData.playerBoosters.Receive(type, 1);

            if (GameData.selectingBoosters.Contains(type) == false)
                GameData.selectingBoosters.Add(type);

            FirebaseAnalyticsHelper.LogEvent("N_BuyBooster", type.ToString());
        }

        Load();

        GameData.playerResources.ConsumeCoin(data.price);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
        EventDispatcher.Instance.PostEvent(EventID.BuyBooster, type);

        if (GameData.isShowingTutorial && type == BoosterType.CoinMagnet)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepBuyBooster);
        }
    }

    private void Highlight()
    {
        highlight.SetActive(GameData.selectingBoosters.Contains(type));
        labelEquip.sprite = GameData.selectingBoosters.Contains(type) ? sprUnequip : sprEquip;
        labelEquip.SetNativeSize();
    }

    private void SetPriceTextColor()
    {
        textPrice.color = GameData.playerResources.coin >= data.price ? Color.white : StaticValue.colorNotEnoughMoney;
    }
}
