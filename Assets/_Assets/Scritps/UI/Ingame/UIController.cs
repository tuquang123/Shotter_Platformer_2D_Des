using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    public UIModeSurvival modeSurvivalUI;
    public GameObject modeCampaignUI;

    public HudComboKill hudComboKill;
    public HudPause hudPause;
    public HudQuest hudQuest;
    public HudBoss hudBoss;
    public HudWin hudWin;
    public HudLose hudLose;
    public HudSelectBooster hudSelectBooster;
    public HudSaveMe hudSaveMe;
    public HudGunDrop hudGunDrop;
    public HudSurvivalResult hudSurvivalResult;
    public HudSurvivalGuide hudSurvivalGuide;
    public LabelMissionStart missionStart;


    [Header("WEAPONS")]
    public ButtonActionIngame buttonSwitchGun;
    public ButtonActionIngame buttonThrowGrenade;
    public Text textSpecialGunAmmo;
    public GameObject infinityAmmoSymbol;
    public Text textNumberOfGrenade;
    public Text textCooldownGrenade;
    public Image imageCooldownGrenade;
    public GameObject btnFire;
    public Button btnAutoFire;
    public Sprite sprAutoFireOff;
    public Sprite sprAutoFireOn;


    [Header("BOOSTER")]
    public ButtonActionIngame buttonUseBoosterHP;
    public Text textRemainingBoosterHP;
    public GameObject[] activeBoosters;

    private int totalBoosterHP;

    [Header("TUTORIAL")]
    public TutorialGamePlayController tutorialGamePlay;

    [Header("SKILL")]
    public Image imgSkillBackground;
    public Button btnActiveSkill;
    public Text textCooldownSkill;


    [Space(20f)]
    public Text textLevelRambo;
    public Text textGameTime;
    public Text textCoinCollected;
    public Image hpPlayer;
    public Image arrowGoRight;
    public Image arrowGoLeft;
    public RectTransform iconRamboMapProgress;
    public GameObject panelIngameUI;
    public GameObject alarmRedScreen;
    public Animation takeDamageScreen;

    private void Start()
    {
        EventDispatcher.Instance.RegisterListener(EventID.SelectBoosterDone, (sender, param) => ActiveBoosters());
        EventDispatcher.Instance.RegisterListener(EventID.ToggleAutoFire, (sender, param) => OnToggleAutoFire());
        EventDispatcher.Instance.RegisterListener(EventID.GameStart, (sender, param) => OnGameStart());
        EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, (sender, param) => ActiveIngameUI(true));
        EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, (sender, param) => ActiveIngameUI(true));

        hudComboKill.Init();
        hudBoss.Init();
        hudGunDrop.Init();

        modeCampaignUI.SetActive(GameData.mode == GameMode.Campaign);
        modeSurvivalUI.gameObject.SetActive(GameData.mode == GameMode.Survival);

        if (GameData.mode == GameMode.Campaign)
        {
            hudSelectBooster.Open();
        }
        else if (GameData.mode == GameMode.Survival)
        {
            modeSurvivalUI.Init();
            hudSurvivalGuide.Open();
        }
    }

    public void Jump()
    {
        EventDispatcher.Instance.PostEvent(EventID.ClickButtonJump);
    }

    public void Shoot(bool isHold)
    {
        EventDispatcher.Instance.PostEvent(EventID.ClickButtonShoot, isHold);
    }

    public void ThrowGrenade()
    {
        EventDispatcher.Instance.PostEvent(EventID.ClickButtonThrowGrenade);
    }

    public void SetCooldownButtonGrenade(bool isDone)
    {
        if (isDone)
            buttonThrowGrenade.Enable();
        else
            buttonThrowGrenade.Disable();

        //buttonThrowGrenade.enabled = isDone;
        imageCooldownGrenade.gameObject.SetActive(!isDone);
        textCooldownGrenade.gameObject.SetActive(!isDone);
    }

    public void ActiveButtonGrenade(bool isActive)
    {
        if (isActive)
        {
            buttonThrowGrenade.Enable();
        }
        else
        {
            buttonThrowGrenade.Disable();
        }
    }

    public void ToggleSwitchGun()
    {
        EventDispatcher.Instance.PostEvent(EventID.ToggleSwitchGun);
    }

    public void ToggleAutoFire()
    {
        EventDispatcher.Instance.PostEvent(EventID.ToggleAutoFire);
    }

    private void OnToggleAutoFire()
    {
        if (btnAutoFire.image.sprite == sprAutoFireOff)
        {
            btnAutoFire.image.sprite = sprAutoFireOn;
        }
        else
        {
            btnAutoFire.image.sprite = sprAutoFireOff;
        }

        btnFire.SetActive(btnAutoFire.image.sprite == sprAutoFireOff);
    }

    public void UseBoosterHP()
    {
        totalBoosterHP--;
        textRemainingBoosterHP.text = totalBoosterHP > 0 ? string.Format("x{0:n0}", totalBoosterHP) : string.Empty;

        if (totalBoosterHP <= 0)
        {
            buttonUseBoosterHP.Disable();
        }

        EventDispatcher.Instance.PostEvent(EventID.UseBoosterHP);
    }

    public void ActiveIngameUI(bool isActive)
    {
        panelIngameUI.SetActive(isActive);
    }

    public void UpdateCoinCollectedText(int value)
    {
        textCoinCollected.text = value.ToString("n0");
    }

    public void UpdateGunTypeText(bool isUsingNormalGun, int specialAmmo)
    {
        textSpecialGunAmmo.gameObject.SetActive(!isUsingNormalGun);
        infinityAmmoSymbol.SetActive(isUsingNormalGun);

        if (isUsingNormalGun == false)
        {
            textSpecialGunAmmo.text = string.Format("x{0:n0}", specialAmmo);
        }
    }

    public void UpdateGrenadeText(int remainingGrenade)
    {
        textNumberOfGrenade.text = string.Format("x{0:n0}", remainingGrenade);
    }

    public void UpdateGameTime(int min, int second)
    {
        if (second < 0)
        {
            second = 0;
        }

        textGameTime.text = string.Format("{0:00}:{1:00}", min, second);
    }

    public void UpdatePlayerHpBar(float percent)
    {
        hpPlayer.fillAmount = percent;
    }

    public void UpdateMapProgress(float percent)
    {
        float targetX = Mathf.Clamp(percent * 184f, 0, 184f);

        Vector2 v = iconRamboMapProgress.anchoredPosition;
        v.x = Mathf.MoveTowards(v.x, targetX, 200f * Time.deltaTime);
        iconRamboMapProgress.anchoredPosition = v;
    }

    public void ShowArrowGo(bool isRight)
    {
        arrowGoRight.gameObject.SetActive(isRight);
        arrowGoLeft.gameObject.SetActive(!isRight);

        this.StartDelayAction(() =>
        {
            arrowGoRight.gameObject.SetActive(false);
            arrowGoLeft.gameObject.SetActive(false);
        }, 3f);
    }

    public void ShowMissionStart()
    {
        //missionStart.Show();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        GameController.Instance.SetActiveAllUnits(false);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySfxClick();
        GameController.Instance.SetActiveAllUnits(false);
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_GAME_PLAY);
    }

    private void OnGameStart()
    {
        ActiveIngameUI(true);
        InitRamboInfo();
    }

    public void ActiveBoosters()
    {
        if (GameData.mode == GameMode.Survival)
        {
            buttonUseBoosterHP.gameObject.SetActive(false);

            for (int i = 0; i < activeBoosters.Length; i++)
            {
                BoosterType type = (BoosterType)(int.Parse(activeBoosters[i].name));
                activeBoosters[i].SetActive(GameData.survivalUsingBooster == type);
            }
        }
        else
        {
            for (int i = 0; i < activeBoosters.Length; i++)
            {
                BoosterType type = (BoosterType)(int.Parse(activeBoosters[i].name));
                activeBoosters[i].SetActive(GameData.selectingBoosters.Contains(type));
            }

            buttonUseBoosterHP.gameObject.SetActive(true);

            if (GameData.selectingBoosters.Contains(BoosterType.Hp))
            {
                totalBoosterHP = 1;
                buttonUseBoosterHP.Enable();
                textRemainingBoosterHP.text = totalBoosterHP > 1 ? string.Format("x{0:n0}", totalBoosterHP) : string.Empty;
            }
            else
            {
                buttonUseBoosterHP.Disable();
            }
        }
    }

    private void InitRamboInfo()
    {
        int ramboId = ProfileManager.UserProfile.ramboId;
        //StaticRamboData ramboData = GameData.staticRamboData.GetData(ramboId);

        int ramboLevel = GameData.playerRambos.ContainsKey(ramboId) ? GameData.playerRambos[ramboId].level : 0;
        textLevelRambo.text = string.Format("Lv: {0}", ramboLevel);

        UpdatePlayerHpBar(1f);
    }

    #region SKILL
    public void SetSkillIcon(int id)
    {
        btnActiveSkill.image.sprite = GameResourcesUtils.GetSkillUnlockImage(id);
        imgSkillBackground.sprite = GameResourcesUtils.GetSkillUnlockImage(id);
    }

    public void ActiveSkill()
    {
        EventDispatcher.Instance.PostEvent(EventID.RamboActiveSkill);
    }

    public void EnableSkill(bool isActive)
    {
        btnActiveSkill.enabled = isActive;
        btnActiveSkill.image.raycastTarget = isActive;
        textCooldownSkill.gameObject.SetActive(!isActive);

        if (isActive)
        {
            btnActiveSkill.image.fillAmount = 1f;
        }
    }

    public void SetCooldownSkill(float percent)
    {
        btnActiveSkill.image.fillAmount = percent;
    }

    public void SetTextCooldownSkill(float remaining)
    {
        textCooldownSkill.text = remaining.ToString();
    }
    #endregion

    // Test
    public void InstantEndGame(bool isWin)
    {
        EventDispatcher.Instance.PostEvent(EventID.GameEnd, isWin);
    }


}
