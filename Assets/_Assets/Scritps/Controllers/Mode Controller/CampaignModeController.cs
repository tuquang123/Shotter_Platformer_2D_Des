using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Spine.Unity;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class CampaignModeController : BaseModeController
{
    public TransportJet transportJetPrefab;

    private int highestComboKill;

    private int coinCollected;
    private int coinBonusComboKill;
    private int coinFirstComplete;
    private int coinTotal;

    private int expWin;

    private TransportJet jet;


    public Map Map { get; private set; }
    public bool IsAllowSpawnSideEnemy { get; set; }
    public BaseUnit Player { get { return GameController.Instance.Player; } }


    #region BASE MODE CONTROLLER IMPLEMENTION
    public override void InitMode()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsx2CoinEndGame, (sender, param) => OnViewAdsx2CoinEndGame((bool)param));
        EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, (sender, param) => OnGetComboKill((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.TransportJetDone, (sender, param) => OnTransportJetDone());
        EventDispatcher.Instance.RegisterListener(EventID.SelectBoosterDone, (sender, param) => OnSelectBoosterDone());
        EventDispatcher.Instance.RegisterListener(EventID.GameStart, (sender, param) => StartGame());
        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) => EndGame((bool)param));
        EventDispatcher.Instance.RegisterListener(EventID.FinishStage, (sender, param) => OnFinishStage());
        EventDispatcher.Instance.RegisterListener(EventID.GetItemDrop, (sender, param) => OnGetItemDrop((ItemDropData)param));
        EventDispatcher.Instance.RegisterListener(EventID.BonusCoinCollected, (sender, param) => OnBonusCoinCollected((float)param));

        //EventDispatcher.Instance.RegisterListener(EventID.BonusExpWin, (sender, param) => OnBonusExpWin((float)param));
        //EventDispatcher.Instance.RegisterListener(EventID.GetCoinCompleteQuest, (sender, param) => OnGetCoinCompleteQuest((int)param));
        //EventDispatcher.Instance.RegisterListener(EventID.GetGemCompleteQuest, (sender, param) => OnGetGemCompleteQuest((int)param));
        //EventDispatcher.Instance.RegisterListener(EventID.GetExpCompleteQuest, (sender, param) => OnGetExpCompleteQuest((int)param));

        IsAllowSpawnSideEnemy = false;
        CreateMap();
        CreateEnemyInZone(1);
        PlayBackgroundMusic();

        //if (GameData.staticStageRewardData.ContainsKey(GameData.currentStage.id))
        //{
        //    StaticStageRewardData rewardData = GameData.staticStageRewardData[GameData.currentStage.id];
        //    expWin = rewardData.expWin[(int)GameData.currentStage.difficulty];
        //}

        CameraFollow.Instance.SetCameraSize(4f);
        CameraFollow.Instance.SetInitialPoint(Map.cameraInitialPoint);
    }

    public override void StartGame()
    {
        Player.enabled = true;
        StartCoroutine(CoroutineRoundTime());
        UIController.Instance.ShowMissionStart();

        if (GameData.playerTutorials.IsCompletedStep(TutorialType.ActionInGame) == false)
        {
            if (string.Compare(GameData.currentStage.id, "1.1") == 0)
            {
                UIController.Instance.tutorialGamePlay.ShowTutorialActionIngame();
            }
        }
    }

    public override void PauseGame()
    {
        base.PauseGame();
    }

    public override void ResumeGame()
    {
        base.ResumeGame();
    }

    public override void ReplayGame()
    {
        throw new NotImplementedException();
    }

    public override void QuitGame()
    {
        throw new NotImplementedException();
    }

    public override void EndGame(bool isWin)
    {
        //int gemTotalReward = 0;
        //int expTotalReward = 0;

        List<RewardData> rewards = new List<RewardData>();

        PlayMusicWinLose(isWin);

        if (isWin)
        {
            SetProgressDailyQuest();

            coinCollected += Map.CoinCompleteStage;
            coinTotal += (coinCollected + coinBonusComboKill);

            if (MapUtils.IsStagePassed(GameData.currentStage.id, GameData.currentStage.difficulty) == false)
            {
                rewards = GameData.staticCampaignStageData.GetFirstTimeRewards(GameData.currentStage.id, GameData.currentStage.difficulty);

                for (int i = 0; i < rewards.Count; i++)
                {
                    if (rewards[i].type == RewardType.Coin)
                    {
                        rewards[i].value += coinTotal;
                    }
                }
            }
            else
            {
                RewardData rewardCoin = new RewardData();
                rewardCoin.type = RewardType.Coin;
                rewardCoin.value = coinTotal;
                rewards.Add(rewardCoin);
            }

            UIController.Instance.hudWin.Open(rewards);

            MapUtils.UnlockCampaignProgress(GameData.currentStage);
            DailyQuestTracker.Instance.Save();
            AchievementTracker.Instance.Save();

            // Rate
            if (ProfileManager.UserProfile.isNoLongerRate == false)
            {
                if (string.Compare(Map.stageNameId, "1.8") == 0)
                {
                    if (ProfileManager.UserProfile.isShowRateMap1 == false)
                    {
                        ProfileManager.UserProfile.isShowRateMap1.Set(true);
                        Popup.Instance.ShowRateUs();
                    }
                }
                else if (string.Compare(Map.stageNameId, "2.4") == 0)
                {
                    if (ProfileManager.UserProfile.isShowRateMap2 == false)
                    {
                        ProfileManager.UserProfile.isShowRateMap2.Set(true);
                        Popup.Instance.ShowRateUs();
                    }
                }
                else if (string.Compare(Map.stageNameId, "3.4") == 0)
                {
                    if (ProfileManager.UserProfile.isShowRateMap3 == false)
                    {
                        ProfileManager.UserProfile.isShowRateMap3.Set(true);
                        Popup.Instance.ShowRateUs();
                    }
                }
            }

            RewardUtils.Receive(rewards);
        }
        else
        {
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_VOICE_GAME_OVER);
            UIController.Instance.hudLose.Open();
        }

        if (!ProfileManager.UserProfile.isRemoveAds)
        {
            bool IsReady = Advertisements.Instance.IsInterstitialAvailable();
            if(IsReady)
            {
                Advertisements.Instance.ShowInterstitial(InterstitialClosed);
            }
            else 
                {
                
                int count = ProfileManager.UserProfile.countRewardInterstitialAds;

                if (count < StaticValue.VIEW_INTERSTITIAL_LIMIT_REWARD_TIMES)
                {
                    GameData.playerResources.ReceiveGem(StaticValue.VIEW_INTERSTITIAL_GEM_REWARD);
                    Popup.Instance.Show(
                    content: string.Format("Get <color=cyan>{0} gems</color> for the ads", StaticValue.VIEW_INTERSTITIAL_GEM_REWARD),
                    title: "rewards"
                    );

                    SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

                    count++;
                    ProfileManager.UserProfile.countRewardInterstitialAds.Set(count);
                }
            }
           
        }


        FirebaseAnalyticsHelper.LogEvent(
            "N_GameResult",
            isWin ? "Win" : "Lose",
            GameData.currentStage.id,
            GameData.currentStage.difficulty
            );

        FirebaseAnalyticsHelper.LogEvent(
            "N_UsePrimaryGun",
            GameData.staticGunData[ProfileManager.UserProfile.gunNormalId].gunName.Replace(" ", "_")
            );

        if (ProfileManager.UserProfile.gunSpecialId != -1)
        {
            FirebaseAnalyticsHelper.LogEvent(
            "N_UseSpecialGun",
            GameData.staticGunData[ProfileManager.UserProfile.gunSpecialId].gunName.Replace(" ", "_")
            );
        }
    }
    private void InterstitialClosed(string advertiser)
    {
        int count = ProfileManager.UserProfile.countRewardInterstitialAds;

        if (count < StaticValue.VIEW_INTERSTITIAL_LIMIT_REWARD_TIMES)
        {
            GameData.playerResources.ReceiveGem(StaticValue.VIEW_INTERSTITIAL_GEM_REWARD);
            Popup.Instance.Show(
            content: string.Format("Get <color=cyan>{0} gems</color> for the ads", StaticValue.VIEW_INTERSTITIAL_GEM_REWARD),
            title: "rewards"
            );

            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_GET_REWARD);

            count++;
            ProfileManager.UserProfile.countRewardInterstitialAds.Set(count);
        }
        Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
    }
    public override void OnPlayerDie()
    {
        throw new NotImplementedException();
    }

    public override void OnPlayerRevive()
    {
        throw new NotImplementedException();
    }
    #endregion


    #region LISTENERS

    private void OnSelectBoosterDone()
    {
        GameController.Instance.IsPaused = false;

        switch (Map.controllerType)
        {
            case ControllerType.Rambo:
                if (Map.isRamboStartOnJet)
                {
                    CreateTransportJet();
                }
                else
                {
                    GameController.Instance.CreatePlayer(Map.playerSpawnPoint.position);
                    GameController.Instance.IsGameStarted = true;
                    EventDispatcher.Instance.PostEvent(EventID.GameStart);
                }
                break;

            case ControllerType.Boat:
                CustomMapRacingBoat boatMode = Map.gameObject.GetComponent<CustomMapRacingBoat>();

                if (boatMode != null)
                    boatMode.Init(Map);

                GameController.Instance.IsGameStarted = true;
                EventDispatcher.Instance.PostEvent(EventID.GameStart);
                break;
        }
    }

    private void OnTransportJetDone()
    {
        Rambo rambo = (Rambo)Player;

        rambo.transform.parent = null;

        BoneFollower bone = rambo.GetComponent<BoneFollower>();
        if (bone)
            Destroy(bone);

        rambo.PlayAnimationParachute();
        rambo.transform.DOMove(Map.playerSpawnPoint.position, 1f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            rambo.enabled = true;
            rambo.Rigid.simulated = true;
            rambo.PlayAnimationIdle();
            rambo.effectDustGround.Play();
            rambo.ActiveHealthBar(true);
            rambo.transform.rotation = Quaternion.identity;
            GameController.Instance.IsGameStarted = true;
            EventDispatcher.Instance.PostEvent(EventID.GameStart);
        });
    }

    private void OnFinishStage()
    {
        GameController.Instance.IsPaused = true;
        GameController.Instance.DeactiveEnemies();
        IsAllowSpawnSideEnemy = false;
    }

    private void SetProgressDailyQuest()
    {
        //if (Map.quest.GetStar() == 3)
        //    EventDispatcher.Instance.PostEvent(EventID.FinishStageWith3Stars);

        if (GameData.selectingBoosters.Count > 0)
            EventDispatcher.Instance.PostEvent(EventID.UseBooster);

        if (GameData.selectingBoosters.Contains(BoosterType.Damage))
            EventDispatcher.Instance.PostEvent(EventID.UseBoosterDamage);

        if (GameData.selectingBoosters.Contains(BoosterType.Critical))
            EventDispatcher.Instance.PostEvent(EventID.UseBoosterCritical);

        if (GameData.selectingBoosters.Contains(BoosterType.Speed))
            EventDispatcher.Instance.PostEvent(EventID.UseBoosterSpeed);

        if (GameData.selectingBoosters.Contains(BoosterType.CoinMagnet))
            EventDispatcher.Instance.PostEvent(EventID.UseBoosterCoinMagnet);
    }

    private void OnViewAdsx2CoinEndGame(bool isWin)
    {
        //DebugCustom.Log(string.Format("View ads x2 coin, collected={0}, comboKillBonus={1}", coinCollected, coinBonusComboKill));
        //GameData.playerResources.ReceiveCoin(coinBonusComboKill + coinCollected);

        //coinTotal += (coinCollected + coinBonusComboKill);
        //coinBonusComboKill *= 2;
        //coinCollected *= 2;
    }

    private void OnGetComboKill(int count)
    {
        if (count == 10)
            coinBonusComboKill += 5;
        else if (count == 20)
            coinBonusComboKill += 25;
        else if (count == 25)
            coinBonusComboKill += 35;
        else if (count >= 26)
            coinBonusComboKill += 1;

        if (count > highestComboKill)
            highestComboKill = count;
    }

    private void OnGetItemDrop(ItemDropData data)
    {
        if (data.type == ItemDropType.Coin)
        {
            coinCollected += (int)data.value;
            UIController.Instance.UpdateCoinCollectedText(coinCollected);
        }
    }

    private void OnBonusExpWin(float percent)
    {
        expWin = Mathf.RoundToInt(expWin * (1 + (percent / 100f)));
    }

    private void OnBonusCoinCollected(float percent)
    {
        coinCollected = Mathf.RoundToInt(coinCollected * (1 + (percent / 100f)));
    }

    #endregion


    #region UNITS

    public void CreateEnemyInZone(int zoneId)
    {
        if (Map.mapData.enemyData == null)
            return;

        for (int i = 0, cnt = Map.mapData.enemyData.Count; i < cnt; i++)
        {
            EnemySpawnData data = Map.mapData.enemyData[i];

            if (data.packId == zoneId)
            {
                BaseUnit enemy = SpawnEnemy(data);
            }
        }
    }

    public void AutoSpawnEnemy()
    {
        if (Map.isAutoSpawnEnemy && IsAllowSpawnSideEnemy && Map.enemyAutoSpawnPrefabs.Length > 0)
        {
            StartCoroutine(CoroutineAutoSpawnEnemy(Map.enemyPerSpawn));
        }
    }

    private IEnumerator CoroutineAutoSpawnEnemy(int numberEnemy)
    {
        int enemySpawned = 0;

        while (enemySpawned < numberEnemy)
        {
            // Spawn from left
            if (CameraFollow.Instance.IsCanSpawnGroundEnemyFromLeft())
            {
                SpawnEnemyFromSide(CameraFollow.Instance.pointGroundSpawnLeft);
            }

            // Spawn from right
            if (CameraFollow.Instance.IsCanSpawnGroundEnemyFromRight())
            {
                SpawnEnemyFromSide(CameraFollow.Instance.pointGroundSpawnRight);
            }

            enemySpawned++;
            yield return StaticValue.waitOneSec;
        }
    }

    private void SpawnEnemyFromSide(Vector2 position)
    {
        if (GameData.mode == GameMode.Campaign)
        {
            int randomEnemyId = UnityEngine.Random.Range(0, Map.enemyAutoSpawnPrefabs.Length);
            int id = Map.enemyAutoSpawnPrefabs[randomEnemyId].id;

            int maxLevel = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
            int level = UnityEngine.Random.Range(1, maxLevel + 1);

            BaseEnemy enemy = SpawnEnemy(id, level, position);
            enemy.zoneId = -1;
            enemy.isRunPassArea = true;
            enemy.canJump = true;
            enemy.DelayTargetPlayer();
        }
    }

    public override BaseEnemy GetEnemyPrefab(int id)
    {
        for (int i = 0; i < Map.enemyPrefabs.Length; i++)
        {
            BaseEnemy enemyPrefab = Map.enemyPrefabs[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        DebugCustom.LogError("Enemy id not in list prefabs=" + id);
        return null;
    }

    private BaseEnemy SpawnEnemy(int id, int level, Vector2 position)
    {
        BaseEnemy enemyPrefab = GetEnemyPrefab(id);
        BaseEnemy enemy = enemyPrefab.GetFromPool();

        enemy.Active(id, level, position);

        GameController.Instance.AddUnit(enemy.gameObject, enemy);

        return enemy;
    }

    private BaseEnemy SpawnEnemy(EnemySpawnData spawnData)
    {
        BaseEnemy enemyPrefab = GetEnemyPrefab(spawnData.id);
        BaseEnemy enemy = enemyPrefab.GetFromPool();

        enemy.Active(spawnData);

        GameController.Instance.AddUnit(enemy.gameObject, enemy);

        return enemy;
    }

    #endregion


    public void PlayMusicWinLose(bool isWin)
    {
        StartCoroutine(CoroutineFadeMusic(isWin));
    }

    private void CreateTransportJet()
    {
        jet = Instantiate(transportJetPrefab, Map.jetStartPoint.position, Map.jetStartPoint.rotation);

        jet.Active(Map.jetDestination.position);
        GameController.Instance.CreatePlayer(jet.playerStandPoint.position, jet.transform);

        Rambo rambo = (Rambo)Player;
        BoneFollower bone = rambo.gameObject.AddComponent<BoneFollower>();
        bone.skeletonRenderer = jet.skeletonAnimation;
        bone.boneName = jet.boneStand;
        rambo.ActiveHealthBar(false);
        rambo.PlayAnimationIdleInJet();
        rambo.Rigid.simulated = false;
        rambo.enabled = false;
    }

    private void CreateMap()
    {
        Map mapPrefab = MapUtils.GetMapPrefab(GameData.currentStage.id);
        Map = Instantiate<Map>(mapPrefab);
        Map.Init();
    }

    private void PlayBackgroundMusic()
    {
        int mapId = int.Parse(Map.stageNameId.Split('.').First());
        string musicClipName = string.Format("music_map_{0}", mapId);
        SoundManager.Instance.PlayMusicFromBeginning(musicClipName);
    }

    private IEnumerator UpdateMapProgress()
    {
        while (true)
        {
            yield return StaticValue.waitHalfSec;

            if (Player)
            {
                float deltaX = Player.transform.position.x - Map.playerSpawnPoint.position.x;
                float mapLength = Map.mapEndPoint.position.x - Map.playerSpawnPoint.position.x;
                float progress = Mathf.Clamp01(deltaX / mapLength);
                UIController.Instance.UpdateMapProgress(progress);
            }
        }
    }

    private IEnumerator CoroutineRoundTime()
    {
        int minute = 0;
        int second = 0;

        while (true)
        {
            if (GameController.Instance.IsPaused == false)
            {
                minute = GameController.Instance.PlayTime / 60;
                second = GameController.Instance.PlayTime % 60;
                UIController.Instance.UpdateGameTime(minute, second);
                GameController.Instance.PlayTime++;
            }

            yield return StaticValue.waitOneSec;
        }
    }

    private IEnumerator CoroutineFadeMusic(bool isWin)
    {
        float currentVolume = SoundManager.Instance.audioMusic.volume;
        WaitForSeconds waitFadeMusic = new WaitForSeconds(0.2f);

        while (SoundManager.Instance.audioMusic.volume > 0)
        {
            SoundManager.Instance.audioMusic.volume -= 0.2f;


            yield return waitFadeMusic;
        }

        SoundManager.Instance.audioMusic.volume = currentVolume;
        SoundManager.Instance.audioMusic.Stop();

        string nextMusic = isWin ? StaticValue.SOUND_MUSIC_WIN : StaticValue.SOUND_MUSIC_LOSE;
        SoundManager.Instance.PlaySfx(nextMusic);
    }
}
