using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopIAP : Singleton<ShopIAP>
{
    //public GameObject gemAndCoinBoard;
    //public GameObject starterBoard;

    public GameObject gemPanel;
    public GameObject coinPanel;
    public GameObject essentialPanel;

    public GameObject bonusGem100;
    public GameObject bonusGem300;
    public GameObject bonusGem500;
    public GameObject bonusGem1000;
    public GameObject bonusGem2500;
    public GameObject bonusGem5000;

    public GameObject btnRestorePurchase;
    public GameObject btnRemoveAds;

    public Text[] priceLabels;

    public bool IsShowing
    {
        get { return enabled; }
    }


#if UNITY_IOS
        if (!PlayerPrefs.HasKey("RestorePurchasedSuccess")) {
            btnRestorePurchase.SetActive(true);
        }
#endif




    private void BuyIapSuccessCallback(Product arg0)
    {
        throw new NotImplementedException();
    }

    public void RestorePurchased()
    {
    }


    #region COIN PACK

    public void ExchangeMedalToCoin(int medal)
    {
        int coin = 0;

        switch (medal)
        {
            case 25:
                coin = 2500;
                break;
        }

        if (GameData.playerResources.medal >= medal)
        {
            Popup.Instance.Show(
                content: string.Format(
                    "would you like to exchange <color=#ffff00ff>{0:n0}</color> medal to <color=#ffff00ff>{1:n0}</color> coins?",
                    medal, coin),
                title: "confirmation",
                type: PopupType.YesNo,
                yesCallback: () =>
                {
                    GameData.playerResources.ConsumeMedal(medal);
                    GameData.playerResources.ReceiveCoin(coin);
                    SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

                    FirebaseAnalyticsHelper.LogEvent(
                        "N_ExchangeMedalToCoin",
                        "N_Medal=" + medal);
                });
        }
        else
        {
            Popup.Instance.ShowToastMessage("not enough medals");
        }

        SoundManager.Instance.PlaySfxClick();
    }

    public void ExchangeGemToCoin(int gem)
    {
        int coin = 0;

        switch (gem)
        {
            case 50:
                // 50 gems to 5000 coins
                coin = 5000;
                break;
            case 100:
                // 100 gems to 12000 coins
                coin = 12000;
                break;
            case 250:
                // 250 gems to 32500 coins
                coin = 32500;
                break;
            case 500:
                // 500 gems to 70000 coins
                coin = 70000;
                break;
            case 1000:
                // 1000 gems to 150000 coins
                coin = 150000;
                break;
        }

        if (GameData.playerResources.gem >= gem)
        {
            Popup.Instance.Show(
                content: string.Format(
                    "would you like to exchange <color=#00ffffff>{0:n0}</color> gems to <color=#ffff00ff>{1:n0}</color> coins?",
                    gem, coin),
                title: "confirmation",
                type: PopupType.YesNo,
                yesCallback: () =>
                {
                    GameData.playerResources.ConsumeGem(gem);
                    GameData.playerResources.ReceiveCoin(coin);
                    SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

                    FirebaseAnalyticsHelper.LogEvent(
                        "N_ExchangeGemToCoin",
                        "N_Gem=" + gem);
                });
        }
    }

    #endregion



    


        


        #region PANEL CONTROL

        public void OpenGemShop()
        {
            //starterBoard.SetActive(false);
            //gemAndCoinBoard.SetActive(true);
            gemPanel.SetActive(true);
            coinPanel.SetActive(false);
            essentialPanel.SetActive(false);

            CheckLabelBonusGem();

            SoundManager.Instance.PlaySfxClick();
        }

        public void OpenCoinShop()
        {
            //starterBoard.SetActive(false);
            //gemAndCoinBoard.SetActive(true);
            gemPanel.SetActive(false);
            coinPanel.SetActive(true);
            essentialPanel.SetActive(false);

            SoundManager.Instance.PlaySfxClick();
        }

        public void OpenEssentialShop()
        {
            //gemAndCoinBoard.SetActive(false);
            //starterBoard.SetActive(false);
            gemPanel.SetActive(false);
            coinPanel.SetActive(false);
            essentialPanel.SetActive(true);

            CheckPurchased();

            SoundManager.Instance.PlaySfxClick();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion


        #region PRIVATE METHODS

        private void CheckPurchased()
        {
            //pricePackStarter1.SetActive(!ProfileManager.UserProfile.isPurchasedStarterPack1);
            //pricePackStarter2.SetActive(!ProfileManager.UserProfile.isPurchasedStarterPack2);

            //pricePackDragon.SetActive(!ProfileManager.UserProfile.isPurchasedLunarPack);
            //pricePackBoom.SetActive(!ProfileManager.UserProfile.isPurchasedBoomPack);
            //pricePackHealthCare.SetActive(!ProfileManager.UserProfile.isPurchasedPublicHealthCare);
            //pricePackFavorite.SetActive(!ProfileManager.UserProfile.isPurchasedEveryFavorite);
            //pricePackMercenaries.SetActive(!ProfileManager.UserProfile.isPurchasedMercenaries);
            //pricePackSaveUs.SetActive(!ProfileManager.UserProfile.isPurchasedSaveUsPack);
            //pricePackStranger.SetActive(!ProfileManager.UserProfile.isPurchasedStrangeLootBox);
            //pricePackDummies.SetActive(!ProfileManager.UserProfile.isPurchasedSnippingDummies);
        }

        private void CheckLabelBonusGem()
        {
            bonusGem100.SetActive(!ProfileManager.UserProfile.isFirstBuyGem100);
            bonusGem300.SetActive(!ProfileManager.UserProfile.isFirstBuyGem300);
            bonusGem500.SetActive(!ProfileManager.UserProfile.isFirstBuyGem500);
            bonusGem1000.SetActive(!ProfileManager.UserProfile.isFirstBuyGem1000);
            bonusGem2500.SetActive(!ProfileManager.UserProfile.isFirstBuyGem2500);
            bonusGem5000.SetActive(!ProfileManager.UserProfile.isFirstBuyGem5000);
        }

        private void OnInitialized()
        {
            //Set localized price text
            ProductIAP[] pros = ProductDefine.GetListProducts();



            //GEM_100
            //GEM_300
            //GEM_500
            //GEM_1000
            //GEM_2500
            //GEM_5000

            //EVERY_FAVORITE
            //DRAGON_BREATH
            //LET_THERE_BE_FIRE

            //SNIPPING_FOR_DUMMIES
            //TASER_LASER
            //SHOCKING_SALE

            //UPGRADE_ENTHUSIAST

            //BATTLE_ESSENTIALS_1
            //BATTLE_ESSENTIALS_2
            //BATTLE_ESSENTIALS_3

            //STARTER_PACK
            //REMOVE_ADS
        }

        #endregion


        #region PROCESS NEW IAP

        private void ProcessBuyGem100()
        {
            int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 100 : 200;
            GameData.playerResources.ReceiveGem(gem);
            Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
            ProfileManager.UserProfile.isFirstBuyGem100.Set(true);
            CheckLabelBonusGem();

            FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 100);
        }

        private void ProcessBuyGem300()
        {
            int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 315 : 630;
            GameData.playerResources.ReceiveGem(gem);
            Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
            ProfileManager.UserProfile.isFirstBuyGem300.Set(true);
            CheckLabelBonusGem();

            FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 300);
        }

        private void ProcessBuyGem500()
        {
            int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 550 : 1100;
            GameData.playerResources.ReceiveGem(gem);
            Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
            ProfileManager.UserProfile.isFirstBuyGem500.Set(true);
            CheckLabelBonusGem();

            FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 500);
        }

        private void ProcessBuyGem1000()
        {
            int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 1250 : 2500;
            GameData.playerResources.ReceiveGem(gem);
            Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
            ProfileManager.UserProfile.isFirstBuyGem1000.Set(true);
            CheckLabelBonusGem();

            FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 1000);
        }

        private void ProcessBuyGem2500()
        {
            int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 3750 : 7500;
            GameData.playerResources.ReceiveGem(gem);
            Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
            ProfileManager.UserProfile.isFirstBuyGem2500.Set(true);
            CheckLabelBonusGem();

            FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 2500);
        }

        private void ProcessBuyGem5000()
        {
            int gem = ProfileManager.UserProfile.isFirstBuyGem100 ? 10000 : 20000;
            GameData.playerResources.ReceiveGem(gem);
            Popup.Instance.ShowToastMessage(string.Format("Received {0} gems", gem), ToastLength.Normal);
            ProfileManager.UserProfile.isFirstBuyGem5000.Set(true);
            CheckLabelBonusGem();

            FirebaseAnalyticsHelper.LogEvent("N_BuyGem", 5000);
        }

        private void ProcessBuyEverybodyFavorite()
        {
            if (ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_everybody_favorite");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_EverybodyFavorite");
        }

        private void ProcessBuyDragonBreath()
        {
            if (ProfileManager.UserProfile.isPurchasedPackDragonBreath)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_dragon_breath");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackDragonBreath.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_DragonBreath");
        }

        private void ProcessBuyLetThereBeFire()
        {
            if (ProfileManager.UserProfile.isPurchasedPackLetThereBeFire)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_let_there_be_fire");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackLetThereBeFire.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_LetBeFire");
        }

        private void ProcessBuySnippingForDummies()
        {
            if (ProfileManager.UserProfile.isPurchasedPackSnippingForDummies)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_snipping_for_dummies");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackSnippingForDummies.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_SnippingDummies");
        }

        private void ProcessBuyTaserLaser()
        {
            if (ProfileManager.UserProfile.isPurchasedPackTaserLaser)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_taser_laser");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackTaserLaser.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_TaserLaser");
        }

        private void ProcessBuyShockingSale()
        {
            if (ProfileManager.UserProfile.isPurchasedPackShockingSale)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_shocking_sale");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackShockingSale.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_ShockingSale");
        }

        private void ProcessBuyUpgradeEnthusiast()
        {
            if (ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_upgrade_enthusiast");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_Enthusiast");
        }

        private void ProcessBuyBattleEssentials_1()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_1");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_Essential_1");
        }

        private void ProcessBuyBattleEssentials_2()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_2");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_Essential_2");
        }

        private void ProcessBuyBattleEssentials_3()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_3");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_Essential_3");
        }

        public void ProcessBuyStarterPack()
        {
            if (ProfileManager.UserProfile.isPurchasedStarterPack)
            {
                return;
            }

            TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_starter");
            List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
            RewardUtils.Receive(rewards);
            Popup.Instance.ShowReward(rewards);

            ProfileManager.UserProfile.isPurchasedStarterPack.Set(true);

            EventDispatcher.Instance.PostEvent(EventID.BuyStarterPack);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_Starter_Pack");
        }

        public void ProcessBuyRemoveAds()
        {
            if (ProfileManager.UserProfile.isRemoveAds)
            {
                return;
            }

            btnRemoveAds.SetActive(false);

            Popup.Instance.Show(
                content: "Your purchase was successful.\nYou now no longer receive ads.",
                title: "Remove ads");

            ProfileManager.UserProfile.isRemoveAds.Set(true);

            FirebaseAnalyticsHelper.LogEvent("N_Buy_RemoveAds");
        }

        #endregion

    }


internal class Product
    {
    }

