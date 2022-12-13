using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using UnityEngine.UI;
using TMPro;

public enum WeaponTab
{
    None = 0,
    Rifle,
    Special,
    Grenade,
    MeleeWeapon
}

public class UpgradeWeaponController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public EnhancedScrollerCellView cellViewUpgradeWeapon;

    [Header("ATTRIBUTE")]
    public TMP_Text nameStats_1;
    public Text curStats_1;
    public Text maxStats_1;
    public GameObject crossStats_1;
    public Image gradeStats_1;

    public TMP_Text nameStats_2;
    public Text curStats_2;
    public Text maxStats_2;
    public GameObject crossStats_2;
    public Image gradeStats_2;

    public TMP_Text nameStats_3;
    public Text curStats_3;
    public Text maxStats_3;
    public GameObject crossStats_3;
    public Image gradeStats_3;

    public TMP_Text nameStats_4;
    public Text curStats_4;
    public Text maxStats_4;
    public GameObject crossStats_4;
    public Image gradeStats_4;

    public Text remainingAmmo;
    public Text priceBuyFullAmmo;
    public Button btnBuyGrenade;
    public Text quantityGrenade;
    public Text pricePerGrenade;
    public Color32 colorStatsNormal;
    public Color32 colorStatsMax;
    public TMP_Text textBattlePower;
    public Sprite[] sprGrades;


    [Header("BUTTONS")]
    public GameObject btnUpgrade;
    public GameObject btnEquip;
    public GameObject btnBuyByBigSalePacks;
    public GameObject btnBuyByStarterPack;
    public GameObject btnGetFromDailyGift;
    public Text priceBuyByCoin;
    public Text priceBuyByGem;
    public Text priceBuyByMedal;
    public Text priceUpgrade;

    [Header("PREVIEW WEAPONS")]
    public RamboPreview rambo;

    [Space(20f)]
    public WeaponTab currentWeaponTab = WeaponTab.None;
    public UpgradeTab[] tabs;

    private int requireMedalBuyWeapon;
    private int requireCoinBuyWeapon;
    private int requireGemBuyWeapon;
    private int requireCoinUpgrade;
    private int requireCoinBuyFullAmmo;
    private int requireCoinBuyPerGrenade;

    private SmallList<CellViewWeaponData> gunData = new SmallList<CellViewWeaponData>();
    private SmallList<CellViewWeaponData> normalGunData = new SmallList<CellViewWeaponData>();
    private SmallList<CellViewWeaponData> specialGunData = new SmallList<CellViewWeaponData>();
    private SmallList<CellViewWeaponData> grenadeData = new SmallList<CellViewWeaponData>();
    private SmallList<CellViewWeaponData> meleeWeaponData = new SmallList<CellViewWeaponData>();

    private SmallList<CellViewWeaponData> currentWeaponData = new SmallList<CellViewWeaponData>();

    public int SelectingGunId { get; set; }
    public int SelectingGrenadeId { get; set; }
    public int SelectingMeleeWeaponId { get; set; }

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SelectWeaponCellView, OnSelectWeaponCellView);
        EventDispatcher.Instance.RegisterListener(EventID.SwichTabUpgradeWeapon, (sender, param) => OnSwitchTab((WeaponTab)param));

        scroller.CreateContainer();
        scroller.Delegate = this;
    }

    private void OnEnable()
    {
        SelectingGunId = ProfileManager.UserProfile.gunNormalId;
        SelectingGrenadeId = ProfileManager.UserProfile.grenadeId;
        SelectingMeleeWeaponId = ProfileManager.UserProfile.meleeWeaponId;

        CreateGunData();
        CreateGrenadeData();
        CreateMeleeWeaponData();

        currentWeaponData = normalGunData;

        OnSwitchTab(WeaponTab.Rifle);
        UpdateTabNotification();
        CheckTutorial();
    }

    private void OnDisable()
    {
        currentWeaponTab = WeaponTab.None;
    }

    private void CheckTutorial()
    {
        if (GameData.playerTutorials.IsCompletedStep(TutorialType.Weapon) == false)
        {
            bool isUziLevel1 = GameData.playerGuns.GetGunLevel(StaticValue.GUN_ID_UZI) == 1;
            bool isNoHaveKamePower = GameData.playerGuns.ContainsKey(StaticValue.GUN_ID_KAME_POWER) == false;

            StaticGunData gunData = GameData.staticGunData.GetData(StaticValue.GUN_ID_UZI);
            int coinUpgradeUziLevel2 = gunData.upgradeInfo[1];
            int coinUpgradeUziLevel3 = gunData.upgradeInfo[2];
            bool isEnoughCoinUpgrade = GameData.playerResources.coin >= (coinUpgradeUziLevel2 + coinUpgradeUziLevel3);

            if (isUziLevel1 && isNoHaveKamePower && isEnoughCoinUpgrade)
            {
                TutorialMenuController.Instance.ShowTutorial(TutorialType.Weapon);
            }
            else
            {
                GameData.playerTutorials.SetComplete(TutorialType.Weapon);
            }
        }
    }

    public void EquipWeapon()
    {
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            EquipGun();
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            EquipGrenade();
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            EquipMeleeWeapon();
        }

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EQUIP_WEAPON);
    }

    public void UnlockWeaponByCoin()
    {
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            UnlockGunByCoin();
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            UnlockGrenadeBuyCoin();
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            UnlockMeleeWeaponByCoin();
        }
    }

    public void UnlockWeaponByGem()
    {
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            UnlockGunByGem();
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            UnlockGrenadeByGem();
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            UnlockMeleeWeaponByGem();
        }
    }

    public void UnlockWeaponByMedal()
    {
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            UnlockGunByMedal();
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            UnlockGrenadeByMedal();
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            UnlockMeleeWeaponByMedal();
        }
    }

    public void UpgradeWeapon()
    {
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            UpgradeGun();
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            UpgradeGrenade();
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            UpgradeMeleeWeapon();
        }
    }

    public void OnSwitchTab(WeaponTab tab)
    {
        SoundManager.Instance.PlaySfxClick();

        if (tab == currentWeaponTab)
            return;

        switch (tab)
        {
            case WeaponTab.Rifle:
                SelectingGunId = ProfileManager.UserProfile.gunNormalId;
                currentWeaponData = normalGunData;
                for (int i = 0; i < gunData.Count; i++)
                {
                    gunData[i].isSelected = gunData[i].id == SelectingGunId;
                }
                rambo.EquipGun(SelectingGunId);
                break;

            case WeaponTab.Special:
                SelectingGunId = ProfileManager.UserProfile.gunSpecialId == -1 ? 100 : ProfileManager.UserProfile.gunSpecialId;
                currentWeaponData = specialGunData;
                for (int i = 0; i < gunData.Count; i++)
                {
                    gunData[i].isSelected = gunData[i].id == SelectingGunId;
                }
                rambo.EquipGun(SelectingGunId);

                if (GameData.isShowingTutorial)
                {
                    EventDispatcher.Instance.PostEvent(EventID.SubStepSwitchSpecialTab);
                }

                break;

            case WeaponTab.Grenade:
                SelectingGrenadeId = ProfileManager.UserProfile.grenadeId;
                currentWeaponData = grenadeData;
                for (int i = 0; i < grenadeData.Count; i++)
                {
                    grenadeData[i].isSelected = grenadeData[i].id == SelectingGrenadeId;
                }
                rambo.EquipGrenade(SelectingGrenadeId);
                break;

            case WeaponTab.MeleeWeapon:
                SelectingMeleeWeaponId = ProfileManager.UserProfile.meleeWeaponId;
                currentWeaponData = meleeWeaponData;
                for (int i = 0; i < meleeWeaponData.Count; i++)
                {
                    meleeWeaponData[i].isSelected = meleeWeaponData[i].id == SelectingMeleeWeaponId;
                }
                rambo.EquipMeleeWeapon(SelectingMeleeWeaponId);
                break;
        }

        currentWeaponTab = tab;
        HighLightCurrentTab();
        scroller.ReloadData();
        UpdateWeaponInformation();
    }

    private void UpdateTabNotification()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].UpdateNotification();
        }
    }

    private void HighLightCurrentTab()
    {
        int tab = (int)currentWeaponTab;

        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].Highlight(i == tab - 1);
        }
    }

    private void UpdateWeaponInformation()
    {
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            CellViewWeaponData data = GetGunData(SelectingGunId);
            UpdateGunLayout();
            UpdateGunAttribute(data);
            UpdateGunPrice(data);
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            CellViewWeaponData data = GetGrenadeData(SelectingGrenadeId);
            UpdateGrenadeLayout();
            UpdateGrenadeAttribute(data);
            UpdateGrenadePrice(data);
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            CellViewWeaponData data = GetMeleeWeaponData(SelectingMeleeWeaponId);
            UpdateMeleeWeaponLayout();
            UpdateMeleeWeaponAttribute(data);
            UpdateMeleeWeaponPrice(data);
        }
    }

    private void HideAllButtonsBuy()
    {
        priceBuyByCoin.transform.parent.gameObject.SetActive(false);
        priceBuyByGem.transform.parent.gameObject.SetActive(false);
        priceBuyByMedal.transform.parent.gameObject.SetActive(false);
        btnGetFromDailyGift.gameObject.SetActive(false);
        btnBuyByBigSalePacks.gameObject.SetActive(false);
        btnBuyByStarterPack.gameObject.SetActive(false);
    }

    private void OnSelectWeaponCellView(Component sender, object param)
    {
        SoundManager.Instance.PlaySfxClick();

        CellViewWeaponData data = (CellViewWeaponData)param;

        // Disable new label
        if (GameData.playerGuns.ContainsKey(data.id) && GameData.playerGuns[data.id].isNew)
        {
            GameData.playerGuns.SetNew(data.id, false);

            for (int i = 0; i < gunData.Count; i++)
            {
                if (gunData[i].id == data.id)
                {
                    if (gunData[i].isNew)
                    {
                        gunData[i].isNew = false;
                        break;
                    }
                }
            }
        }

        if (GameData.playerGrenades.ContainsKey(data.id) && GameData.playerGrenades[data.id].isNew)
        {
            GameData.playerGrenades.SetNew(data.id, false);

            for (int i = 0; i < grenadeData.Count; i++)
            {
                if (grenadeData[i].id == data.id)
                {
                    if (grenadeData[i].isNew)
                    {
                        grenadeData[i].isNew = false;
                        break;
                    }
                }
            }
        }

        if (GameData.playerMeleeWeapons.ContainsKey(data.id) && GameData.playerMeleeWeapons[data.id].isNew)
        {
            GameData.playerMeleeWeapons.SetNew(data.id, false);

            for (int i = 0; i < meleeWeaponData.Count; i++)
            {
                if (meleeWeaponData[i].id == data.id)
                {
                    if (meleeWeaponData[i].isNew)
                    {
                        meleeWeaponData[i].isNew = false;
                        break;
                    }
                }
            }
        }

        UpdateTabNotification();

        // Reload information
        if (currentWeaponTab == WeaponTab.Rifle || currentWeaponTab == WeaponTab.Special)
        {
            if (SelectingGunId == data.id)
                return;

            SelectingGunId = data.id;

            for (int i = 0; i < gunData.Count; i++)
            {
                gunData[i].isSelected = gunData[i].id == data.id;
            }

            rambo.EquipGun(SelectingGunId);
        }
        else if (currentWeaponTab == WeaponTab.Grenade)
        {
            if (SelectingGrenadeId == data.id)
                return;

            SelectingGrenadeId = data.id;

            for (int i = 0; i < grenadeData.Count; i++)
            {
                grenadeData[i].isSelected = grenadeData[i].id == data.id;
            }

            rambo.EquipGrenade(SelectingGrenadeId);
        }
        else if (currentWeaponTab == WeaponTab.MeleeWeapon)
        {
            if (SelectingMeleeWeaponId == data.id)
                return;

            SelectingMeleeWeaponId = data.id;

            for (int i = 0; i < meleeWeaponData.Count; i++)
            {
                meleeWeaponData[i].isSelected = meleeWeaponData[i].id == data.id;
            }

            rambo.EquipMeleeWeapon(SelectingMeleeWeaponId);
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void Refresh()
    {
        if (gameObject.activeInHierarchy)
        {
            CreateGunData();
            CreateGrenadeData();
            CreateMeleeWeaponData();

            if (currentWeaponTab == WeaponTab.Rifle)
                currentWeaponData = normalGunData;
            else if (currentWeaponTab == WeaponTab.Special)
                currentWeaponData = specialGunData;
            else if (currentWeaponTab == WeaponTab.Grenade)
                currentWeaponData = grenadeData;
            else if (currentWeaponTab == WeaponTab.MeleeWeapon)
                currentWeaponData = meleeWeaponData;

            scroller.ReloadData();
            UpdateWeaponInformation();
            UpdateTabNotification();
        }
    }


    #region Gun

    public void BuyFullAmmo()
    {
        if (GameData.playerResources.coin < requireCoinBuyFullAmmo)
        {
            Popup.Instance.ShowToastMessage("not enough gems");
            SoundManager.Instance.PlaySfxClick();
        }
        else if (requireCoinBuyFullAmmo <= 0)
        {
            Popup.Instance.ShowToastMessage("gun has max ammo");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinBuyFullAmmo);
            BuyFullAmmoSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
            EventDispatcher.Instance.PostEvent(EventID.BuyAmmo);
        }
    }

    private void UnlockGunByCoin()
    {
        if (GameData.playerResources.coin < requireCoinBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("Not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinBuyWeapon);
            UnlockGunSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

            FirebaseAnalyticsHelper.LogEvent(
            "N_UnlockGunByCoin",
            GameData.staticGunData.GetData(SelectingGunId).gunName.Replace(" ", string.Empty)
            );
        }
    }

    private void UnlockGunByGem()
    {
        if (GameData.playerResources.gem < requireGemBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("not enough gems");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeGem(requireGemBuyWeapon);
            UnlockGunSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

            FirebaseAnalyticsHelper.LogEvent(
            "N_UnlockGunByGem",
            GameData.staticGunData.GetData(SelectingGunId).gunName.Replace(" ", string.Empty)
            );
        }
    }

    private void UnlockGunByMedal()
    {
        if (GameData.playerResources.medal < requireMedalBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("not enough medals");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeMedal(requireMedalBuyWeapon);
            UnlockGunSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

            FirebaseAnalyticsHelper.LogEvent(
            "N_UnlockGunByMedal",
            GameData.staticGunData.GetData(SelectingGunId).gunName.Replace(" ", string.Empty)
            );
        }
    }

    private void UpgradeGun()
    {
        if (GameData.playerResources.coin < requireCoinUpgrade)
        {
            Popup.Instance.ShowToastMessage("not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinUpgrade);
            UpgradeGunSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_UPGRADE_SUCCESS);
        }
    }

    private void EquipGun()
    {
        if (currentWeaponTab == WeaponTab.Rifle)
        {
            ProfileManager.UserProfile.gunNormalId.Set(SelectingGunId);
        }
        else if (currentWeaponTab == WeaponTab.Special)
        {
            ProfileManager.UserProfile.gunSpecialId.Set(SelectingGunId);
        }

        for (int i = 0; i < gunData.Count; i++)
        {
            CellViewWeaponData data = gunData[i];

            data.isEquipped = (data.id == ProfileManager.UserProfile.gunNormalId || data.id == ProfileManager.UserProfile.gunSpecialId);
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void UnlockGunSuccess()
    {
        if (currentWeaponTab == WeaponTab.Rifle)
        {
            ProfileManager.UserProfile.gunNormalId.Set(SelectingGunId);
        }
        else if (currentWeaponTab == WeaponTab.Special)
        {
            ProfileManager.UserProfile.gunSpecialId.Set(SelectingGunId);
        }

        for (int i = 0; i < gunData.Count; i++)
        {
            CellViewWeaponData data = gunData[i];

            if (data.id == SelectingGunId)
            {
                SelectingGunId = data.id;
                data.isLock = false;
                data.level = 1;

                GameData.playerGuns.ReceiveNewGun(SelectingGunId);
            }

            data.isEquipped = (data.id == ProfileManager.UserProfile.gunNormalId || data.id == ProfileManager.UserProfile.gunSpecialId);
            data.isSelected = data.id == SelectingGunId;
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void UpgradeGunSuccess()
    {
        for (int i = 0; i < gunData.Count; i++)
        {
            CellViewWeaponData data = gunData[i];

            if (data.id == SelectingGunId)
            {
                StaticGunData staticGunData = GameData.staticGunData.GetData(data.id);
                data.level++;
                data.level = Mathf.Clamp(data.level, 1, staticGunData.upgradeInfo.Length);
                data.isUpgrading = true;
            }
        }

        GameData.playerGuns.IncreaseGunLevel(SelectingGunId);

        FirebaseAnalyticsHelper.LogEvent("N_UpgradeGun",
            GameData.staticGunData[SelectingGunId].gunName.Replace(" ", string.Empty),
            "ToLevel=" + GameData.playerGuns[SelectingGunId].level);

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();

        if (GameData.isShowingTutorial && SelectingGunId == StaticValue.GUN_ID_UZI)
        {
            if (GameData.playerGuns[SelectingGunId].level == 2)
            {
                EventDispatcher.Instance.PostEvent(EventID.SubStepUpgradeUziTolevel2);
            }
            else if (GameData.playerGuns[SelectingGunId].level == 3)
            {
                EventDispatcher.Instance.PostEvent(EventID.SubStepUpgradeUziTolevel3);
            }
        }
    }

    private void BuyFullAmmoSuccess()
    {
        StaticGunData data = GameData.staticGunData.GetData(SelectingGunId);

        int level = GameData.playerGuns[data.id].level;
        string path = string.Format(data.statsPath, level);
        SO_GunStats curLevelStats = Resources.Load<SO_GunStats>(path);

        GameData.playerGuns.SetGunAmmo(data.id, curLevelStats.Ammo);

        requireCoinBuyFullAmmo = 0;
        priceBuyFullAmmo.text = requireCoinBuyFullAmmo.ToString("n0");
        remainingAmmo.text = string.Format("{0}/{1}", curLevelStats.Ammo, curLevelStats.Ammo);
        remainingAmmo.color = Color.white;
    }

    private void CreateGunData()
    {
        gunData.Clear();
        normalGunData.Clear();
        specialGunData.Clear();

        foreach (StaticGunData staticData in GameData.staticGunData.Values)
        {
            CellViewWeaponData cellViewData = new CellViewWeaponData();
            cellViewData.id = staticData.id;
            cellViewData.weaponName = staticData.gunName;
            cellViewData.statsPath = staticData.statsPath;
            cellViewData.weaponImage = GameResourcesUtils.GetGunImage(staticData.id);
            cellViewData.isLock = !GameData.playerGuns.ContainsKey(staticData.id);
            cellViewData.level = cellViewData.isLock ? 0 : GameData.playerGuns[staticData.id].level;
            cellViewData.isSelected = staticData.id == SelectingGunId;
            cellViewData.isNew = GameData.playerGuns.ContainsKey(staticData.id) && GameData.playerGuns[staticData.id].isNew;

            if (staticData.isSpecialGun)
            {
                cellViewData.isEquipped = staticData.id == ProfileManager.UserProfile.gunSpecialId;
                specialGunData.Add(cellViewData);
            }
            else
            {
                cellViewData.isEquipped = staticData.id == ProfileManager.UserProfile.gunNormalId;
                normalGunData.Add(cellViewData);
            }

            gunData.Add(cellViewData);
        }
    }

    private void UpdateGunLayout()
    {
        nameStats_1.transform.parent.gameObject.SetActive(true);
        nameStats_2.transform.parent.gameObject.SetActive(true);
        nameStats_3.transform.parent.gameObject.SetActive(true);
        nameStats_4.transform.parent.gameObject.SetActive(true);

        crossStats_1.SetActive(true);
        crossStats_2.SetActive(false);
        crossStats_3.SetActive(false);
        crossStats_4.SetActive(false);

        nameStats_1.text = "DAMAGE";
        nameStats_2.text = "FIRE RATE";
        nameStats_3.text = "CRIT RATE";
        nameStats_4.text = "CRIT DMG";

        btnBuyGrenade.gameObject.SetActive(false);
        remainingAmmo.transform.parent.gameObject.SetActive(true);
    }

    private void UpdateGunAttribute(CellViewWeaponData data)
    {
        StaticGunData staticGunData = GameData.staticGunData.GetData(data.id);

        int level = Mathf.Clamp(data.level, 1, staticGunData.upgradeInfo.Length);
        bool isMaxLevel = level >= staticGunData.upgradeInfo.Length;

        string path = string.Format(data.statsPath, level);
        SO_GunStats curLevelStats = Resources.Load<SO_GunStats>(path);

        path = string.Format(data.statsPath, staticGunData.upgradeInfo.Length);
        SO_GunStats maxLevelStats = Resources.Load<SO_GunStats>(path);

        curStats_1.text = string.Format("{0}", Mathf.RoundToInt(curLevelStats.Damage * 10));
        curStats_1.color = isMaxLevel ? colorStatsMax : colorStatsNormal;
        maxStats_1.text = string.Format("{0}", Mathf.RoundToInt(maxLevelStats.Damage * 10));
        WeaponStatsGrade gradeDamage = GameData.staticGunData.GetGradeDamage(curLevelStats.Damage);
        gradeStats_1.sprite = sprGrades[(int)gradeDamage];
        gradeStats_1.SetNativeSize();

        float fireRate = GameData.staticGunData.GetFireRate(data.id, level);
        curStats_2.text = string.Format("{0}", Mathf.RoundToInt(fireRate * 100));
        curStats_2.color = colorStatsNormal;
        maxStats_2.text = string.Empty;
        WeaponStatsGrade gradeFireRate = GameData.staticGunData.GetGradeFireRate(fireRate);
        gradeStats_2.sprite = sprGrades[(int)gradeFireRate];
        gradeStats_2.SetNativeSize();

        curStats_3.text = string.Format("{0}%", curLevelStats.CriticalRate);
        curStats_3.color = colorStatsNormal;
        maxStats_3.text = string.Empty;
        WeaponStatsGrade gradeCritRate = GameData.staticGunData.GetGradeCritRate(curLevelStats.CriticalRate);
        gradeStats_3.sprite = sprGrades[(int)gradeCritRate];
        gradeStats_3.SetNativeSize();

        curStats_4.text = string.Format("{0}%", curLevelStats.CriticalDamageBonus + 100);
        curStats_4.color = colorStatsNormal;
        maxStats_4.text = string.Empty;
        WeaponStatsGrade gradeCritDamage = GameData.staticGunData.GetGradeCritDamage(curLevelStats.CriticalDamageBonus);
        gradeStats_4.sprite = sprGrades[(int)gradeCritDamage];
        gradeStats_4.SetNativeSize();

        int power = Mathf.RoundToInt(GameData.staticGunData.GetBattlePower(data.id, level) * 100);
        textBattlePower.text = power.ToString();
    }

    private void UpdateGunPrice(CellViewWeaponData data)
    {
        StaticGunData staticGunData = GameData.staticGunData.GetData(data.id);

        if (GameData.playerGuns.ContainsKey(data.id))
        {
            HideAllButtonsBuy();

            int level = Mathf.Clamp(data.level, 1, staticGunData.upgradeInfo.Length);
            bool isMaxLevel = level >= staticGunData.upgradeInfo.Length;

            if (!isMaxLevel)
            {
                requireCoinUpgrade = staticGunData.upgradeInfo[level];
                priceUpgrade.transform.parent.gameObject.SetActive(true);
                priceUpgrade.text = requireCoinUpgrade.ToString("n0");
                priceUpgrade.color = GameData.playerResources.coin >= requireCoinUpgrade ? Color.white : StaticValue.colorNotEnoughMoney;
            }
            else
            {
                priceUpgrade.transform.parent.gameObject.SetActive(false);
            }

            bool isEquippedGun = (data.id == ProfileManager.UserProfile.gunNormalId || data.id == ProfileManager.UserProfile.gunSpecialId);
            btnEquip.gameObject.SetActive(!isEquippedGun);

            // Ammo
            if (staticGunData.isSpecialGun)
            {
                remainingAmmo.transform.parent.gameObject.SetActive(true);

                string path = string.Format(data.statsPath, level);
                SO_GunStats curLevelStats = Resources.Load<SO_GunStats>(path);

                int ammo = Mathf.Clamp(GameData.playerGuns[data.id].ammo, 0, curLevelStats.Ammo);
                remainingAmmo.text = string.Format("{0}/{1}", ammo, curLevelStats.Ammo);
                remainingAmmo.color = (float)ammo / (float)curLevelStats.Ammo <= 0.1f ? StaticValue.colorNotEnoughMoney : Color.white;

                int ammoToMax = curLevelStats.Ammo - ammo;
                requireCoinBuyFullAmmo = ammoToMax * staticGunData.ammoPrice;
                priceBuyFullAmmo.text = requireCoinBuyFullAmmo.ToString("n0");
                priceBuyFullAmmo.color = GameData.playerResources.coin >= requireCoinBuyFullAmmo ? Color.white : StaticValue.colorNotEnoughMoney;
            }
            else
            {
                remainingAmmo.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            priceUpgrade.transform.parent.gameObject.SetActive(false);
            remainingAmmo.transform.parent.gameObject.SetActive(false);
            btnEquip.gameObject.SetActive(false);

            priceBuyByCoin.transform.parent.gameObject.SetActive(staticGunData.coinUnlock > 0);
            priceBuyByCoin.text = staticGunData.coinUnlock.ToString("n0");
            requireCoinBuyWeapon = staticGunData.coinUnlock;
            priceBuyByCoin.color = GameData.playerResources.coin >= requireCoinBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            priceBuyByGem.transform.parent.gameObject.SetActive(staticGunData.gemUnlock > 0);
            priceBuyByGem.text = staticGunData.gemUnlock.ToString("n0");
            requireGemBuyWeapon = staticGunData.gemUnlock;
            priceBuyByGem.color = GameData.playerResources.gem >= requireGemBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            priceBuyByMedal.transform.parent.gameObject.SetActive(staticGunData.medalUnlock > 0);
            priceBuyByMedal.text = staticGunData.medalUnlock.ToString("n0");
            requireMedalBuyWeapon = staticGunData.medalUnlock;
            priceBuyByMedal.color = GameData.playerResources.medal >= requireMedalBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            btnGetFromDailyGift.gameObject.SetActive(staticGunData.otherWayObtain.Contains(WayToObtain.DailyQuest));
            btnBuyByBigSalePacks.gameObject.SetActive(staticGunData.otherWayObtain.Contains(WayToObtain.BigSalePacks));
            btnBuyByStarterPack.gameObject.SetActive(staticGunData.otherWayObtain.Contains(WayToObtain.StarterPack));
        }
    }

    private CellViewWeaponData GetGunData(int gunId)
    {
        for (int i = 0; i < gunData.Count; i++)
        {
            CellViewWeaponData data = gunData[i];

            if (data.id == gunId)
            {
                return data;
            }
        }

        return null;
    }

    #endregion

    #region Grenade

    public void BuyGrenade()
    {
        if (GameData.playerResources.coin < requireCoinBuyPerGrenade)
        {
            Popup.Instance.ShowToastMessage("not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinBuyPerGrenade);
            BuyGrenadeSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
        }
    }

    private void UnlockGrenadeBuyCoin()
    {
        if (GameData.playerResources.coin < requireCoinBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinBuyWeapon);
            UnlockGrenadeSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
        }
    }

    private void UnlockGrenadeByGem()
    {
        if (GameData.playerResources.gem < requireGemBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("not enough gems");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeGem(requireGemBuyWeapon);
            UnlockGrenadeSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);
        }
    }

    private void UnlockGrenadeByMedal()
    {

    }

    private void UpgradeGrenade()
    {
        if (GameData.playerResources.coin < requireCoinUpgrade)
        {
            Popup.Instance.ShowToastMessage("not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinUpgrade);
            UpgradeGrenadeSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_UPGRADE_SUCCESS);
        }
    }

    private void EquipGrenade()
    {
        ProfileManager.UserProfile.grenadeId.Set(SelectingGrenadeId);

        for (int i = 0; i < grenadeData.Count; i++)
        {
            CellViewWeaponData data = grenadeData[i];

            data.isEquipped = data.id == ProfileManager.UserProfile.grenadeId;
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void UnlockGrenadeSuccess(int quantity = 1)
    {
        ProfileManager.UserProfile.grenadeId.Set(SelectingGrenadeId);

        for (int i = 0; i < grenadeData.Count; i++)
        {
            CellViewWeaponData data = grenadeData[i];

            if (data.id == SelectingGrenadeId)
            {
                SelectingGrenadeId = data.id;
                data.isLock = false;
                data.level = 1;

                GameData.playerGrenades.Receive(data.id, quantity);
            }

            data.isEquipped = data.id == ProfileManager.UserProfile.grenadeId;
            data.isSelected = data.id == SelectingGrenadeId;
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void UpgradeGrenadeSuccess()
    {
        for (int i = 0; i < grenadeData.Count; i++)
        {
            CellViewWeaponData data = grenadeData[i];

            if (data.id == SelectingGrenadeId)
            {
                StaticGrenadeData staticGrenadeData = GameData.staticGrenadeData.GetData(data.id);
                data.level++;
                data.level = Mathf.Clamp(data.level, 1, staticGrenadeData.upgradeInfo.Length);
            }
        }

        GameData.playerGrenades.IncreaseGrenadeLevel(SelectingGrenadeId);

        FirebaseAnalyticsHelper.LogEvent("N_UpgradeGrenade",
            GameData.staticGrenadeData[SelectingGrenadeId].grenadeName,
            "ToLevel=" + GameData.playerGrenades[SelectingGrenadeId].level);

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void BuyGrenadeSuccess()
    {
        if (GameData.playerGrenades.ContainsKey(SelectingGrenadeId))
        {
            GameData.playerGrenades.Receive(SelectingGrenadeId, 1);

            quantityGrenade.text = GameData.playerGrenades[SelectingGrenadeId].quantity.ToString("n0");
            quantityGrenade.color = GameData.playerGrenades[SelectingGrenadeId].quantity > 0 ? Color.white : StaticValue.colorNotEnoughMoney;

            EventDispatcher.Instance.PostEvent(EventID.BuyGrenade);
        }
        else
        {
            DebugCustom.LogError("[BuyGrenadeSuccess] Key not found=" + SelectingGrenadeId);
        }
    }

    private void CreateGrenadeData()
    {
        grenadeData.Clear();

        foreach (StaticGrenadeData staticData in GameData.staticGrenadeData.Values)
        {
            CellViewWeaponData cellViewData = new CellViewWeaponData();
            cellViewData.id = staticData.id;
            cellViewData.weaponName = staticData.grenadeName;
            cellViewData.statsPath = staticData.statsPath;
            //cellViewData.scaleRatioInShop = staticData.scaleRatioInShop;
            cellViewData.weaponImage = GameResourcesUtils.GetGrenadeImage(staticData.id);
            cellViewData.isLock = !GameData.playerGrenades.ContainsKey(staticData.id);
            cellViewData.level = cellViewData.isLock ? 0 : GameData.playerGrenades[staticData.id].level;
            cellViewData.isNew = GameData.playerGrenades.ContainsKey(staticData.id) && GameData.playerGrenades[staticData.id].isNew;
            cellViewData.isSelected = staticData.id == SelectingGrenadeId;
            cellViewData.isEquipped = staticData.id == ProfileManager.UserProfile.grenadeId;

            grenadeData.Add(cellViewData);
        }
    }

    private void UpdateGrenadeLayout()
    {
        nameStats_1.transform.parent.gameObject.SetActive(true);
        nameStats_2.transform.parent.gameObject.SetActive(true);
        nameStats_3.transform.parent.gameObject.SetActive(true);
        nameStats_4.transform.parent.gameObject.SetActive(false);

        crossStats_1.SetActive(true);
        crossStats_2.SetActive(true);
        crossStats_3.SetActive(true);
        crossStats_4.SetActive(false);

        nameStats_1.text = "DAMAGE";
        nameStats_2.text = "RADIUS";
        nameStats_3.text = "COOLDOWN";
        nameStats_4.text = string.Empty;

        btnBuyGrenade.gameObject.SetActive(true);
        remainingAmmo.transform.parent.gameObject.SetActive(false);
    }

    private void UpdateGrenadeAttribute(CellViewWeaponData data)
    {
        StaticGrenadeData staticGrenadeData = GameData.staticGrenadeData.GetData(data.id);

        int level = Mathf.Clamp(data.level, 1, staticGrenadeData.upgradeInfo.Length);
        bool isMaxLevel = level >= staticGrenadeData.upgradeInfo.Length;

        string path = string.Format(data.statsPath, level);
        SO_GrenadeStats curLevelStats = Resources.Load<SO_GrenadeStats>(path);

        path = string.Format(data.statsPath, staticGrenadeData.upgradeInfo.Length);
        SO_GrenadeStats maxLevelStats = Resources.Load<SO_GrenadeStats>(path);

        curStats_1.text = string.Format("{0}", Mathf.RoundToInt(curLevelStats.Damage * 10));
        curStats_1.color = isMaxLevel ? colorStatsMax : colorStatsNormal;
        maxStats_1.text = string.Format("{0}", Mathf.RoundToInt(maxLevelStats.Damage * 10));
        WeaponStatsGrade gradeDamage = GameData.staticGrenadeData.GetGradeDamage(curLevelStats.Damage);
        gradeStats_1.sprite = sprGrades[(int)gradeDamage];
        gradeStats_1.SetNativeSize();

        curStats_2.text = string.Format("{0}", Mathf.RoundToInt(curLevelStats.Radius * 100));
        curStats_2.color = isMaxLevel ? colorStatsMax : colorStatsNormal;
        maxStats_2.text = string.Format("{0}", Mathf.RoundToInt(maxLevelStats.Radius * 100));
        WeaponStatsGrade gradeRadius = GameData.staticGrenadeData.GetGradeRadius(curLevelStats.Radius);
        gradeStats_2.sprite = sprGrades[(int)gradeRadius];
        gradeStats_2.SetNativeSize();

        curStats_3.text = string.Format("{0:f2}s", curLevelStats.Cooldown);
        curStats_3.color = isMaxLevel ? colorStatsMax : colorStatsNormal;
        maxStats_3.text = string.Format("{0:f2}s", maxLevelStats.Cooldown);
        WeaponStatsGrade gradeCooldown = GameData.staticGrenadeData.GetGradeCooldown(curLevelStats.Cooldown);
        gradeStats_3.sprite = sprGrades[(int)gradeCooldown];
        gradeStats_3.SetNativeSize();

        int power = Mathf.RoundToInt(GameData.staticGrenadeData.GetBattlePower(data.id, level) * 100);
        textBattlePower.text = power.ToString();
    }

    private void UpdateGrenadePrice(CellViewWeaponData data)
    {
        StaticGrenadeData staticGrenadeData = GameData.staticGrenadeData.GetData(data.id);

        if (GameData.playerGrenades.ContainsKey(data.id))
        {
            HideAllButtonsBuy();

            int level = Mathf.Clamp(data.level, 1, staticGrenadeData.upgradeInfo.Length);
            bool isMaxLevel = level >= staticGrenadeData.upgradeInfo.Length;

            if (!isMaxLevel)
            {
                requireCoinUpgrade = staticGrenadeData.upgradeInfo[level];
                priceUpgrade.transform.parent.gameObject.SetActive(true);
                priceUpgrade.text = requireCoinUpgrade.ToString("n0");
                priceUpgrade.color = GameData.playerResources.coin >= requireCoinUpgrade ? Color.white : StaticValue.colorNotEnoughMoney;
            }
            else
            {
                priceUpgrade.transform.parent.gameObject.SetActive(false);
            }

            bool isEquippedGrenade = data.id == ProfileManager.UserProfile.grenadeId;
            btnEquip.gameObject.SetActive(!isEquippedGrenade);

            // Quantity
            btnBuyGrenade.gameObject.SetActive(true);
            quantityGrenade.text = GameData.playerGrenades[data.id].quantity.ToString("n0");
            quantityGrenade.color = GameData.playerGrenades[data.id].quantity > 0 ? Color.white : StaticValue.colorNotEnoughMoney;

            requireCoinBuyPerGrenade = staticGrenadeData.pricePerUnit;
            pricePerGrenade.text = requireCoinBuyPerGrenade.ToString("n0");
            pricePerGrenade.color = GameData.playerResources.coin >= requireCoinBuyPerGrenade ? Color.white : StaticValue.colorNotEnoughMoney;
            btnBuyGrenade.enabled = true;

        }
        else
        {
            priceUpgrade.transform.parent.gameObject.SetActive(false);
            btnBuyGrenade.gameObject.SetActive(false);
            btnEquip.gameObject.SetActive(false);

            priceBuyByCoin.transform.parent.gameObject.SetActive(staticGrenadeData.coinUnlock > 0);
            priceBuyByCoin.text = staticGrenadeData.coinUnlock.ToString("n0");
            requireCoinBuyWeapon = staticGrenadeData.coinUnlock;
            priceBuyByCoin.color = GameData.playerResources.coin >= requireCoinBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            priceBuyByGem.transform.parent.gameObject.SetActive(staticGrenadeData.gemUnlock > 0);
            priceBuyByGem.text = staticGrenadeData.gemUnlock.ToString("n0");
            requireGemBuyWeapon = staticGrenadeData.gemUnlock;
            priceBuyByGem.color = GameData.playerResources.gem >= requireGemBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            priceBuyByMedal.transform.parent.gameObject.SetActive(staticGrenadeData.medalUnlock > 0);
            priceBuyByMedal.text = staticGrenadeData.medalUnlock.ToString("n0");
            requireMedalBuyWeapon = staticGrenadeData.medalUnlock;
            priceBuyByMedal.color = GameData.playerResources.medal >= requireMedalBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            btnGetFromDailyGift.gameObject.SetActive(staticGrenadeData.otherWayObtain.Contains(WayToObtain.DailyQuest));
            btnBuyByBigSalePacks.gameObject.SetActive(staticGrenadeData.otherWayObtain.Contains(WayToObtain.BigSalePacks));
            btnBuyByStarterPack.gameObject.SetActive(staticGrenadeData.otherWayObtain.Contains(WayToObtain.StarterPack));
        }
    }

    private CellViewWeaponData GetGrenadeData(int gunId)
    {
        for (int i = 0; i < grenadeData.Count; i++)
        {
            CellViewWeaponData data = grenadeData[i];

            if (data.id == gunId)
            {
                return data;
            }
        }

        return null;
    }

    #endregion

    #region MeleeWeapon

    private void UnlockMeleeWeaponByCoin()
    {
        if (GameData.playerResources.coin < requireCoinBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("Not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinBuyWeapon);
            UnlockMeleeWeaponSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

            FirebaseAnalyticsHelper.LogEvent(
            "N_UnlockMeleeWeaponByCoin",
            GameData.staticMeleeWeaponData.GetData(SelectingMeleeWeaponId).weaponName
            );
        }
    }

    private void UnlockMeleeWeaponByGem()
    {
        if (GameData.playerResources.gem < requireGemBuyWeapon)
        {
            Popup.Instance.ShowToastMessage("not enough gems");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeGem(requireGemBuyWeapon);
            UnlockMeleeWeaponSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_PURCHASE_SUCCESS);

            FirebaseAnalyticsHelper.LogEvent(
            "N_UnlockMeleeWeaponByGem",
            GameData.staticMeleeWeaponData.GetData(SelectingMeleeWeaponId).weaponName
            );
        }
    }

    private void UnlockMeleeWeaponByMedal()
    {

    }

    private void UpgradeMeleeWeapon()
    {
        if (GameData.playerResources.coin < requireCoinUpgrade)
        {
            Popup.Instance.ShowToastMessage("not enough coins");
            SoundManager.Instance.PlaySfxClick();
        }
        else
        {
            GameData.playerResources.ConsumeCoin(requireCoinUpgrade);
            UpgradeMeleeWeaponSuccess();
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_UPGRADE_SUCCESS);
        }
    }

    private void EquipMeleeWeapon()
    {
        ProfileManager.UserProfile.meleeWeaponId.Set(SelectingMeleeWeaponId);

        for (int i = 0; i < meleeWeaponData.Count; i++)
        {
            CellViewWeaponData data = meleeWeaponData[i];

            data.isEquipped = data.id == ProfileManager.UserProfile.meleeWeaponId;
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void UnlockMeleeWeaponSuccess()
    {
        ProfileManager.UserProfile.meleeWeaponId.Set(SelectingMeleeWeaponId);

        for (int i = 0; i < meleeWeaponData.Count; i++)
        {
            CellViewWeaponData data = meleeWeaponData[i];

            if (data.id == SelectingMeleeWeaponId)
            {
                SelectingMeleeWeaponId = data.id;
                data.isLock = false;
                data.level = 1;

                GameData.playerMeleeWeapons.ReceiveNewMeleeWeapon(SelectingMeleeWeaponId);
            }

            data.isEquipped = data.id == ProfileManager.UserProfile.meleeWeaponId;
            data.isSelected = data.id == SelectingMeleeWeaponId;
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void UpgradeMeleeWeaponSuccess()
    {
        for (int i = 0; i < meleeWeaponData.Count; i++)
        {
            CellViewWeaponData data = meleeWeaponData[i];

            if (data.id == SelectingMeleeWeaponId)
            {
                StaticMeleeWeaponData staticData = GameData.staticMeleeWeaponData.GetData(data.id);
                data.level++;
                data.level = Mathf.Clamp(data.level, 1, staticData.upgradeInfo.Length);
            }
        }

        if (GameData.playerMeleeWeapons.ContainsKey(SelectingMeleeWeaponId))
        {
            GameData.playerMeleeWeapons.IncreaseMeleeWeaponLevel(SelectingMeleeWeaponId);

            FirebaseAnalyticsHelper.LogEvent("N_UpgradeMeleeWeapon",
                GameData.staticMeleeWeaponData[SelectingMeleeWeaponId].weaponName,
                "ToLevel=" + GameData.playerMeleeWeapons[SelectingMeleeWeaponId].level);
        }
        else
        {
            DebugCustom.LogError("[UpgradeMeleeWeaponSuccess] Key not found=" + SelectingMeleeWeaponId);
        }

        scroller.RefreshActiveCellViews();
        UpdateWeaponInformation();
    }

    private void CreateMeleeWeaponData()
    {
        meleeWeaponData.Clear();

        foreach (StaticMeleeWeaponData staticData in GameData.staticMeleeWeaponData.Values)
        {
            CellViewWeaponData cellViewData = new CellViewWeaponData();
            cellViewData.id = staticData.id;
            cellViewData.weaponName = staticData.weaponName;
            cellViewData.statsPath = staticData.statsPath;
            cellViewData.weaponImage = GameResourcesUtils.GetMeleeWeaponImage(staticData.id);
            cellViewData.isLock = !GameData.playerMeleeWeapons.ContainsKey(staticData.id);
            cellViewData.level = cellViewData.isLock ? 0 : GameData.playerMeleeWeapons[staticData.id].level;
            cellViewData.isSelected = staticData.id == SelectingMeleeWeaponId;
            cellViewData.isNew = GameData.playerMeleeWeapons.ContainsKey(staticData.id) && GameData.playerMeleeWeapons[staticData.id].isNew;
            cellViewData.isEquipped = staticData.id == ProfileManager.UserProfile.meleeWeaponId;

            meleeWeaponData.Add(cellViewData);
        }
    }

    private void UpdateMeleeWeaponLayout()
    {
        nameStats_1.transform.parent.gameObject.SetActive(true);
        nameStats_2.transform.parent.gameObject.SetActive(true);
        nameStats_3.transform.parent.gameObject.SetActive(true);
        nameStats_4.transform.parent.gameObject.SetActive(true);

        crossStats_1.SetActive(true);
        crossStats_2.SetActive(false);
        crossStats_3.SetActive(false);
        crossStats_4.SetActive(false);

        nameStats_1.text = "DAMAGE";
        nameStats_2.text = "ATK RATE";
        nameStats_3.text = "CRIT RATE";
        nameStats_4.text = "CRIT DMG";

        btnBuyGrenade.gameObject.SetActive(false);
        remainingAmmo.transform.parent.gameObject.SetActive(false);
    }

    private void UpdateMeleeWeaponAttribute(CellViewWeaponData data)
    {
        StaticMeleeWeaponData staticData = GameData.staticMeleeWeaponData.GetData(data.id);

        int level = Mathf.Clamp(data.level, 1, staticData.upgradeInfo.Length);
        bool isMaxLevel = level >= staticData.upgradeInfo.Length;

        string path = string.Format(data.statsPath, level);
        SO_MeleeWeaponStats curLevelStats = Resources.Load<SO_MeleeWeaponStats>(path);

        path = string.Format(data.statsPath, staticData.upgradeInfo.Length);
        SO_MeleeWeaponStats maxLevelStats = Resources.Load<SO_MeleeWeaponStats>(path);

        curStats_1.text = string.Format("{0}", Mathf.RoundToInt(curLevelStats.Damage * 10));
        curStats_1.color = isMaxLevel ? colorStatsMax : colorStatsNormal;
        maxStats_1.text = string.Format("{0}", Mathf.RoundToInt(maxLevelStats.Damage * 10));
        WeaponStatsGrade gradeDamage = GameData.staticMeleeWeaponData.GetGradeDamage(curLevelStats.Damage);
        gradeStats_1.sprite = sprGrades[(int)gradeDamage];
        gradeStats_1.SetNativeSize();

        curStats_2.text = string.Format("{0}", Mathf.RoundToInt(curLevelStats.AttackTimePerSecond * 100));
        curStats_2.color = colorStatsNormal;
        maxStats_2.text = string.Empty;
        WeaponStatsGrade gradeFireRate = GameData.staticMeleeWeaponData.GetGradeAttackSpeed(curLevelStats.AttackTimePerSecond);
        gradeStats_2.sprite = sprGrades[(int)gradeFireRate];
        gradeStats_2.SetNativeSize();

        curStats_3.text = string.Format("{0}%", curLevelStats.CriticalRate);
        curStats_3.color = colorStatsNormal;
        maxStats_3.text = string.Empty;
        WeaponStatsGrade gradeCritRate = GameData.staticMeleeWeaponData.GetGradeCritRate(curLevelStats.CriticalRate);
        gradeStats_3.sprite = sprGrades[(int)gradeCritRate];
        gradeStats_3.SetNativeSize();

        curStats_4.text = string.Format("{0}%", curLevelStats.CriticalDamageBonus + 100);
        curStats_4.color = colorStatsNormal;
        maxStats_4.text = string.Empty;
        WeaponStatsGrade gradeCritDamage = GameData.staticMeleeWeaponData.GetGradeCritDamage(curLevelStats.CriticalDamageBonus);
        gradeStats_4.sprite = sprGrades[(int)gradeCritDamage];
        gradeStats_4.SetNativeSize();

        int power = Mathf.RoundToInt(GameData.staticMeleeWeaponData.GetBattlePower(data.id, level) * 100);
        textBattlePower.text = power.ToString();
    }

    private void UpdateMeleeWeaponPrice(CellViewWeaponData data)
    {
        StaticMeleeWeaponData staticData = GameData.staticMeleeWeaponData.GetData(data.id);

        if (GameData.playerMeleeWeapons.ContainsKey(data.id))
        {
            HideAllButtonsBuy();

            int level = Mathf.Clamp(data.level, 1, staticData.upgradeInfo.Length);
            bool isMaxLevel = level >= staticData.upgradeInfo.Length;

            if (!isMaxLevel)
            {
                requireCoinUpgrade = staticData.upgradeInfo[level];
                priceUpgrade.transform.parent.gameObject.SetActive(true);
                priceUpgrade.text = requireCoinUpgrade.ToString("n0");
                priceUpgrade.color = GameData.playerResources.coin >= requireCoinUpgrade ? Color.white : StaticValue.colorNotEnoughMoney;
            }
            else
            {
                priceUpgrade.transform.parent.gameObject.SetActive(false);
            }

            bool isEquipped = data.id == ProfileManager.UserProfile.meleeWeaponId;
            btnEquip.gameObject.SetActive(!isEquipped);
        }
        else
        {
            priceUpgrade.transform.parent.gameObject.SetActive(false);
            btnBuyGrenade.gameObject.SetActive(false);
            btnEquip.gameObject.SetActive(false);

            priceBuyByCoin.transform.parent.gameObject.SetActive(staticData.coinUnlock > 0);
            priceBuyByCoin.text = staticData.coinUnlock.ToString("n0");
            requireCoinBuyWeapon = staticData.coinUnlock;
            priceBuyByCoin.color = GameData.playerResources.coin >= requireCoinBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            priceBuyByGem.transform.parent.gameObject.SetActive(staticData.gemUnlock > 0);
            priceBuyByGem.text = staticData.gemUnlock.ToString("n0");
            requireGemBuyWeapon = staticData.gemUnlock;
            priceBuyByGem.color = GameData.playerResources.gem >= requireGemBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            priceBuyByMedal.transform.parent.gameObject.SetActive(staticData.medalUnlock > 0);
            priceBuyByMedal.text = staticData.medalUnlock.ToString("n0");
            requireMedalBuyWeapon = staticData.medalUnlock;
            priceBuyByMedal.color = GameData.playerResources.medal >= requireMedalBuyWeapon ? Color.white : StaticValue.colorNotEnoughMoney;

            btnGetFromDailyGift.gameObject.SetActive(staticData.otherWayObtain.Contains(WayToObtain.DailyQuest));
            btnBuyByBigSalePacks.gameObject.SetActive(staticData.otherWayObtain.Contains(WayToObtain.BigSalePacks));
            btnBuyByStarterPack.gameObject.SetActive(staticData.otherWayObtain.Contains(WayToObtain.StarterPack));
        }
    }

    private CellViewWeaponData GetMeleeWeaponData(int id)
    {
        for (int i = 0; i < meleeWeaponData.Count; i++)
        {
            CellViewWeaponData data = meleeWeaponData[i];

            if (data.id == id)
            {
                return data;
            }
        }

        return null;
    }

    #endregion

    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return currentWeaponData.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        //float numberCellViewDisplay = 3.7f;
        //float height = scrollerRectTransform.rect.height;
        //float cellViewSize = (height - ((numberCellViewDisplay - 1) * scroller.spacing)) / numberCellViewDisplay;
        //return cellViewSize;

        return 126f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        CellViewWeapon cellView = scroller.GetCellView(cellViewUpgradeWeapon) as CellViewWeapon;
        cellView.name = currentWeaponData[dataIndex].weaponName;
        cellView.SetData(currentWeaponData[dataIndex]);
        return cellView;
    }

    #endregion
}
