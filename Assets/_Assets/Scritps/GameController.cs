using DG.Tweening;
using Newtonsoft.Json;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public BaseModeController modeController;
    public ItemDropController itemDropController;
    public Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

    public Map CampaignMap { get { return ((CampaignModeController)modeController).Map; } }
    public MapSurvival SurvivalMap { get { return ((SurvivalModeController)modeController).Map; } }
    public BaseUnit Player { get; set; }
    public int PlayTime { get; set; }
    public bool IsGameStarted { get; set; }
    public bool IsPaused { get; set; }


    #region MODE CONTROLLER EDITOR WRAPPER

    public ModeControllerWrapper modeControllerWrapper;
    public CampaignModeController campaignModeController { get { return modeControllerWrapper.campaignModeController; } }
    public SurvivalModeController survivalModeController { get { return modeControllerWrapper.survivalModeController; } }

    #endregion


    #region UNITY METHODS

    private void Awake()
    {
        PoolingController.Instance.InitPool();

        switch (GameData.mode)
        {
            case GameMode.Campaign:
                modeController = campaignModeController;
                Destroy(survivalModeController.gameObject);
                break;

            case GameMode.Survival:
                modeController = survivalModeController;
                Destroy(campaignModeController.gameObject);
                break;
        }

        modeController.enabled = true;
        modeController.InitMode();
    }

    private void Start()
    {
        IsPaused = true;
        IsGameStarted = false;
        GameData.isUseRevive = false;
        DebugCustom.Log(string.Format("left={0}, right={1}, top={2}, bottom={3}", CameraFollow.Instance.left.position, CameraFollow.Instance.right.position, CameraFollow.Instance.top.position,
            CameraFollow.Instance.bottom.position));
    }

    private void OnApplicationPause(bool pause)
    {
        if (IsPaused == false)
        {
            if (IsGameStarted)
                UIController.Instance.hudPause.Open();

            if (CameraFollow.Instance.slowMotion.IsShowing)
                Time.timeScale = 0.2f;
        }
    }

    #endregion


    #region UNITS

    public void CreatePlayer(Vector2 position, Transform parent = null)
    {
        Rambo ramboPrefab = GameResourcesUtils.GetRamboPrefab(ProfileManager.UserProfile.ramboId);
        Rambo rambo = Instantiate(ramboPrefab);

        int id = ProfileManager.UserProfile.ramboId;
        int level = GameData.playerRambos.GetRamboLevel(id);

        rambo.Active(id, level, position);
        rambo.Rigid.simulated = true;
        GameController.Instance.Player = rambo;
        CameraFollow.Instance.SetTarget(Player.transform);

        GameController.Instance.AddUnit(Player.gameObject, Player);

        rambo.transform.parent = parent;
    }

    public void AddUnit(GameObject obj, BaseUnit unit)
    {
        if (activeUnits.ContainsKey(obj))
        {
            DebugCustom.LogError("[ADD UNIT] Key already exist: " + obj.name);
        }

        activeUnits[obj] = unit;
    }

    public void RemoveUnit(GameObject obj)
    {
        if (activeUnits.ContainsKey(obj))
        {
            activeUnits.Remove(obj);
        }
        else
        {
            DebugCustom.LogError("[REMOVE UNIT] Not found: " + obj.name);
        }
    }

    public void SetActiveAllUnits(bool isActive)
    {
        List<BaseUnit> tmp = new List<BaseUnit>(activeUnits.Values);

        foreach (BaseUnit unit in tmp)
        {
            if (unit is RubberBoat)
            {

            }
            else
            {
                unit.enabled = isActive;
            }
        }
    }

    public void DeactiveEnemies()
    {
        List<BaseUnit> tmp = new List<BaseUnit>(activeUnits.Values);

        for (int i = 0; i < tmp.Count; i++)
        {
            if (tmp[i].transform.root.CompareTag(StaticValue.TAG_ENEMY))
            {
                tmp[i].Deactive();
            }
        }
    }

    public BaseUnit GetUnit(GameObject obj)
    {
        if (activeUnits.ContainsKey(obj))
        {
            return activeUnits[obj];
        }
        else
        {
            DebugCustom.Log("[GET UNIT] Not found: " + obj.name);
            return null;
        }
    }

    public BaseEnemy GetEnemyPrefab(int id)
    {
        return modeController.GetEnemyPrefab(id);
    }

    #endregion
}
