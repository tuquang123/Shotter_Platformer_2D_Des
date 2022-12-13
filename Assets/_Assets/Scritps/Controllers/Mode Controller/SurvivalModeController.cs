using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Facebook.Unity;
using UnityEngine.Advertisements;

public class SurvivalModeController : BaseModeController
{
    public MapSurvival[] mapPrefabs;

    // Phân cấp enemy theo mức độ nguy hiểm
    public BaseEnemy[] enemyType_1;
    public BaseEnemy[] enemyType_2;
    public BaseEnemy[] enemyType_3;
    public BaseEnemy[] enemyType_4;
    public BaseEnemy[] enemyType_5;

    [Header("SKILL")]
    public BombSupportSurvival bombPrefab;
    private WaitForSeconds bombInterval = new WaitForSeconds(0.1f);

    // Statistics
    private int soldierKill;
    private int vehicleKill;
    private int bossKill;
    private int highestComboKill;

    private int soldierScore;
    private int vehicleScore;
    private int bossScore;
    private int timeScore;
    private int totalScore;

    private int curWaveIndex = -1;
    private int numberWave;
    private int timerPerWave;
    private int enemyKilledInWave;
    private int totalEnemiesInWave;
    private bool isPreparingNextWave;
    private IEnumerator coroutineStartWave;
    private IEnumerator coroutineCountDownNextWave;
    private SO_SurvivalWave curWaveData;
    private SurvivalWaveData currentWaveData;
    private Dictionary<int, int> unitScoreData; // key=enemy id, value=score
    private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();
    public MapSurvival Map { get; private set; }
    public BaseUnit Player { get { return GameController.Instance.Player; } }


    #region BASE MODE CONTROLLER IMPLEMENTION

    public override void InitMode()
    {
        EventDispatcher.Instance.RegisterListener(EventID.StartFirstWave, (sender, param) => OnStartPlay());
        EventDispatcher.Instance.RegisterListener(EventID.GameStart, (sender, param) => StartGame());
        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) => EndGame((bool)param));
        EventDispatcher.Instance.RegisterListener(EventID.LabelWaveAnimateComplete, (sender, param) => CountDownNextWave());
        //EventDispatcher.Instance.RegisterListener(EventID.BossSurvivalDie, (sender, param) => OnBossDie());
        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);
        EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, (sender, param) => OnGetComboKill((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemBomb, (sender, param) => OnUseSupportItemBomb());
        EventDispatcher.Instance.RegisterListener(EventID.QuitSurvivalSession, (sender, param) => OnQuitSurvivalSession());

        LoadScoreData();
        CreateMap();
        PlayBackgroundMusic();
    }

    public override void StartGame()
    {
        Player.enabled = true;
        CountDownNextWave();
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
        GameController.Instance.IsPaused = true;
        GameController.Instance.DeactiveEnemies();
        UIController.Instance.ActiveIngameUI(false);
        UIController.Instance.alarmRedScreen.SetActive(false);
        DailyQuestTracker.Instance.Save();
        AchievementTracker.Instance.Save();

        SurvivalResultData data = new SurvivalResultData(numberWave, soldierKill, soldierScore, vehicleKill, vehicleScore,
            bossKill, bossScore, timeScore, highestComboKill, totalScore);

        if (totalScore > 0)
        {
            GameData.playerTournamentData.score += totalScore;
            TournamentData tournamentData = new TournamentData(AccessToken.CurrentAccessToken.UserId, GameData.playerTournamentData.score, GetMostUsedGunId(), false);
            FireBaseDatabase.Instance.SaveTournamentData(tournamentData);
        }

        UIController.Instance.hudSurvivalResult.Open(data);

        if (!ProfileManager.UserProfile.isRemoveAds)
        {
            if (Advertisements.Instance.IsRewardVideoAvailable())
            {
                Advertisements.Instance.ShowRewardedVideo(CompleteMethod3);
            }
            
            //AdMobController.Instance.ShowRewardedVideoAd(null);
        }

        EventDispatcher.Instance.PostEvent(EventID.CompleteSurvivalSession);

        FirebaseAnalyticsHelper.LogEvent("N_CompleteSurvival", "Wave=" + numberWave, "Score=" + totalScore);
    }
   
    private void CompleteMethod3(bool completed, string advertiser)
    {
        Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
        if (completed == true)
        {
            //give the reward
        }
        else
        {
            //no reward
        }
    }

    public override void OnPlayerDie()
    {
        throw new NotImplementedException();
    }

    public override BaseEnemy GetEnemyPrefab(int id)
    {
        for (int i = 0; i < enemyType_1.Length; i++)
        {
            BaseEnemy enemyPrefab = enemyType_1[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        for (int i = 0; i < enemyType_2.Length; i++)
        {
            BaseEnemy enemyPrefab = enemyType_2[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        for (int i = 0; i < enemyType_3.Length; i++)
        {
            BaseEnemy enemyPrefab = enemyType_3[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        for (int i = 0; i < enemyType_4.Length; i++)
        {
            BaseEnemy enemyPrefab = enemyType_4[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        for (int i = 0; i < enemyType_5.Length; i++)
        {
            BaseEnemy enemyPrefab = enemyType_5[i];

            if (enemyPrefab.id == id)
            {
                return enemyPrefab;
            }
        }

        DebugCustom.LogError("Enemy id not in list prefabs=" + id);
        return null;
    }

    public override void OnPlayerRevive()
    {
        throw new NotImplementedException();
    }

    #endregion


    #region LISTENERS

    private void OnStartPlay()
    {
        GameController.Instance.IsPaused = false;
        GameController.Instance.CreatePlayer(Map.playerSpawnPoint.position);
        GameController.Instance.IsGameStarted = true;
        EventDispatcher.Instance.PostEvent(EventID.GameStart);
    }

    //private void OnBossDie()
    //{
    //    SceneFading.Instance.FadePingPongBlackAlpha(
    //            fadingTime: 2f,
    //            toBlackCallback: () =>
    //            {
    //                UIController.Instance.hudBoss.HideUI();
    //                CameraFollow.Instance.SetCameraSize(7.5f);
    //                Map.SetDefaultMapMargin();
    //                Map.HideBossPoints();
    //            },
    //            finishCallback: () =>
    //            {
    //                CompleteWave();
    //            }
    //    );
    //}

    private void OnQuitSurvivalSession()
    {
        if (totalScore > 0)
        {
            GameData.playerTournamentData.score += totalScore;
            TournamentData tournamentData = new TournamentData(AccessToken.CurrentAccessToken.UserId, GameData.playerTournamentData.score, ProfileManager.UserProfile.gunNormalId, false);
            FireBaseDatabase.Instance.SaveTournamentData(tournamentData);
        }

        if (!ProfileManager.UserProfile.isRemoveAds)
        {
            if (Advertisements.Instance.IsInterstitialAvailable())
            {
                Advertisements.Instance.ShowInterstitial(InterstitialClosed);

            }
            
            UIController.Instance.BackToMainMenu();
        }
        else
        {
            UIController.Instance.BackToMainMenu();
        }
    }
    private void InterstitialClosed(string advertiser)
    {
        UIController.Instance.BackToMainMenu();
    }

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            //if (enemy.isFinalBoss)
            //{
            //    int score = GetUnitScore(enemy.id);
            //    totalScore += score;
            //    bossScore += score;
            //    bossKill++;
            //}
            //else if (curWaveData.IsBossWave == false)
            //{
            //    int score = GetUnitScore(enemy.id) * enemy.level;
            //    totalScore += score;

            //    if (enemy.id < 100)
            //    {
            //        // Soldiers
            //        soldierKill++;
            //        soldierScore += score;
            //    }
            //    else if (enemy.id < 1000)
            //    {
            //        // Vehicles
            //        vehicleKill++;
            //        vehicleScore += score;
            //    }

            //    UIController.Instance.modeSurvivalUI.SetScoreText(totalScore);

            //    // Remaining units
            //    RemoveUnit(enemy);
            //    DebugCustom.Log("Remaining unit=" + activeUnits.Count);
            //    if (activeUnits.Count <= 0 && enemyKilledInWave >= totalEnemiesInWave)
            //    {
            //        CompleteWave();
            //    }
            //}

            int score = GetUnitScore(enemy.id, enemy.level);
            totalScore += score;

            if (enemy.id < 100)
            {
                // Soldiers
                soldierKill++;
                soldierScore += score;
            }
            else if (enemy.id < 1000)
            {
                // Vehicles
                vehicleKill++;
                vehicleScore += score;
            }

            UIController.Instance.modeSurvivalUI.SetScoreText(totalScore);

            // Remaining units
            RemoveUnit(enemy);
            //DebugCustom.Log("Remaining unit=" + activeUnits.Count);
            if (activeUnits.Count <= 0 && enemyKilledInWave >= totalEnemiesInWave)
            {
                CompleteWave();
            }
        }
    }

    private void OnGetComboKill(int count)
    {
        if (count > highestComboKill)
        {
            highestComboKill = count;
        }
    }

    #endregion


    #region SUPPORT ITEMS

    private void OnUseSupportItemBomb()
    {
        StartCoroutine(CoroutineBomb());
    }

    private void ReleaseBomb(Vector2 pos)
    {
        BombSupportSurvival bomb = PoolingController.Instance.poolBombSupportSurvival.New();

        if (bomb == null)
        {
            bomb = Instantiate<BombSupportSurvival>(bombPrefab);
        }

        bomb.Active(pos, UnityEngine.Random.Range(50f, 80f));
    }

    private IEnumerator CoroutineBomb()
    {
        Vector2 startPosition = CameraFollow.Instance.top.position;
        startPosition.y += 1.5f;
        startPosition.x = CameraFollow.Instance.left.position.x;

        Vector2 endPosition = startPosition;
        endPosition.x = CameraFollow.Instance.right.position.x;

        while (startPosition.x < endPosition.x)
        {
            ReleaseBomb(startPosition);
            Vector2 v = startPosition;
            v.x += 1f;
            startPosition = v;
            yield return bombInterval;
        }
    }

    #endregion

    public void AddUnit(BaseUnit unit)
    {
        if (activeUnits.ContainsKey(unit.gameObject) == false)
        {
            activeUnits.Add(unit.gameObject, unit);
        }
    }

    public void RemoveUnit(BaseUnit unit)
    {
        if (activeUnits.ContainsKey(unit.gameObject))
        {
            activeUnits.Remove(unit.gameObject);
            enemyKilledInWave++;
        }
    }

    private void CreateMap()
    {
        if (mapPrefabs.Length > 0)
        {
            MapSurvival mapPrefab = mapPrefabs[UnityEngine.Random.Range(0, mapPrefabs.Length)];
            Map = Instantiate<MapSurvival>(mapPrefab);
            Map.Init();

            CameraFollow.Instance.SetCameraSize(7.5f);
            CameraFollow.Instance.SetInitialPoint(Map.cameraInitialPoint);
        }
        else
        {
            DebugCustom.LogError("Map prefabs empty");
        }
    }

    private void PlayBackgroundMusic()
    {
        //int totalMap = Enum.GetNames(typeof(MapType)).Length;
        //int randomId = UnityEngine.Random.Range(0, totalMap);
        //string musicClipName = string.Format("music_map_{0}", randomId + 1);
        SoundManager.Instance.PlayMusicFromBeginning(StaticValue.SOUND_MUSIC_SURVIVAL);
    }

    private void CountDownNextWave()
    {
        if (coroutineCountDownNextWave != null)
        {
            StopCoroutine(coroutineCountDownNextWave);
        }

        coroutineCountDownNextWave = CoroutineCountDownNextWave();
        StartCoroutine(coroutineCountDownNextWave);
    }

    private void StartNextWave()
    {
        //curWaveIndex++;
        //numberWave++;
        //curWaveIndex = Mathf.Clamp(curWaveIndex, 0, Map.wavesInfo.Length - 1);
        //curWaveData = Map.wavesInfo[curWaveIndex];

        //timerPerWave = 0;
        //isPreparingNextWave = false;

        //if (curWaveData.IsBossWave)
        //{
        //    DebugCustom.Log(string.Format("Start new wave {0} - BOSS", numberWave));
        //    TriggerPointBoss bossPoint = GetBossPoint((int)curWaveData.BossType);
        //    bossPoint.levelInNormal = UnityEngine.Random.Range(curWaveData.MinLevelUnit, curWaveData.MaxLevelUnit + 1);

        //    SceneFading.Instance.FadePingPongBlackAlpha(
        //        fadingTime: 2f,
        //        toBlackCallback: () =>
        //        {
        //            CameraFollow.Instance.SetCameraSize(4f);
        //            GameController.Instance.Player.transform.position = bossPoint.playerStandPosition.position;
        //            bossPoint.gameObject.SetActive(true);
        //        }
        //        /*finishCallback: () => { bossPoint.gameObject.SetActive(true); }*/);
        //}
        //else
        //{
        //    DebugCustom.Log(string.Format("Start new wave {0} - NORMAL", numberWave));
        //    CalculateWaveUnits(curWaveData);

        //    if (coroutineStartWave != null)
        //    {
        //        StopCoroutine(coroutineStartWave);
        //    }

        //    coroutineStartWave = CoroutineStartWave(curWaveData);
        //    StartCoroutine(coroutineStartWave);
        //}

        numberWave++;
        timerPerWave = 0;
        highestComboKill = 0;
        isPreparingNextWave = false;

        currentWaveData = new SurvivalWaveData();
        currentWaveData.waveId = numberWave;

        int minUnitQuantity = numberWave + 9;
        int maxUnitQuantity = Mathf.RoundToInt(minUnitQuantity * 1.2f);
        currentWaveData.numberUnits = UnityEngine.Random.Range(minUnitQuantity, maxUnitQuantity + 1);

        int minLevel = Mathf.Clamp(numberWave / 2, 1, 20);
        currentWaveData.minLevelUnit = minLevel;
        currentWaveData.maxLevelUnit = Mathf.Clamp(minLevel + 1, 1, 20);

        currentWaveData.units = new List<SurvivalEnemy>();

        int type_1 = 0, type_2 = 0, type_3 = 0, type_4 = 0, type_5 = 0;

        for (int i = 0; i < currentWaveData.numberUnits; i++)
        {
            int random = UnityEngine.Random.Range(1, 1001);
            SurvivalEnemy type;

            if (random <= 500)
            {
                int rd = UnityEngine.Random.Range(0, enemyType_1.Length);
                type = (SurvivalEnemy)enemyType_1[rd].id;
                type_1++;
            }
            else if (random <= 750)
            {
                int rd = UnityEngine.Random.Range(0, enemyType_2.Length);
                type = (SurvivalEnemy)enemyType_2[rd].id;
                type_2++;
            }
            else if (random <= 850)
            {
                int rd = UnityEngine.Random.Range(0, enemyType_3.Length);
                type = (SurvivalEnemy)enemyType_3[rd].id;
                type_3++;
            }
            else if (random <= 950)
            {
                int rd = UnityEngine.Random.Range(0, enemyType_4.Length);
                type = (SurvivalEnemy)enemyType_4[rd].id;
                type_4++;
            }
            else
            {
                int rd = UnityEngine.Random.Range(0, enemyType_5.Length);
                type = (SurvivalEnemy)enemyType_5[rd].id;
                type_5++;
            }

            currentWaveData.units.Add(type);

            //int randomIndex = UnityEngine.Random.Range(0, Map.enemyPrefabs.Length);
            //SurvivalEnemy type = (SurvivalEnemy)Map.enemyPrefabs[randomIndex].id;
            //currentWaveData.units.Add(type);
        }

        DebugCustom.Log(string.Format("type1={0},type2={1},type3={2},type4={3},type5={4}", type_1, type_2, type_3, type_4, type_5));
        DebugCustom.Log(string.Format("Start new wave {0}", numberWave));
        CalculateWaveUnits(currentWaveData);

        if (coroutineStartWave != null)
        {
            StopCoroutine(coroutineStartWave);
        }

        coroutineStartWave = CoroutineStartWave(currentWaveData);
        StartCoroutine(coroutineStartWave);
    }

    private void CompleteWave()
    {
        int scoreComboKill = highestComboKill;
        int scoreTime = (Mathf.Clamp(60 - timerPerWave, 0, 60)) * 3;
        DebugCustom.Log(string.Format("Complete wave {0}, scoreComboKill={1}, scoreTime={2}", numberWave, scoreComboKill, scoreTime));
        totalScore += (scoreComboKill + scoreTime);
        timeScore += scoreTime;
        UIController.Instance.modeSurvivalUI.SetScoreText(totalScore);
        UIController.Instance.modeSurvivalUI.ShowComplete();
        isPreparingNextWave = true;

        EventDispatcher.Instance.PostEvent(EventID.CompleteWave, curWaveData);

        if (numberWave == 5)
        {
            EventDispatcher.Instance.PostEvent(EventID.CompleteSurvivalWave5);
        }
    }

    private void UpdateWaveTime()
    {
        int minute = timerPerWave / 60;
        int second = timerPerWave % 60;
        UIController.Instance.UpdateGameTime(minute, second);
    }

    //private void CalculateWaveUnits(SO_SurvivalWave waveData)
    //{
    //    activeUnits.Clear();
    //    enemyKilledInWave = 0;
    //    totalEnemiesInWave = 0;

    //    for (int i = 0; i < waveData.Time.Count; i++)
    //    {
    //        TimeData timeData = waveData.Time[i];

    //        for (int j = 0; j < timeData.units.Count; j++)
    //        {
    //            SurvivalUnitData enemy = timeData.units[j];

    //            if (enemy.type != SurvivalEnemy.Bomber)
    //            {
    //                totalEnemiesInWave += enemy.quantity;
    //            }
    //        }
    //    }

    //    DebugCustom.Log(string.Format("Wave {0} has {1} enemies", numberWave, totalEnemiesInWave));
    //}

    private void CalculateWaveUnits(SurvivalWaveData waveData)
    {
        activeUnits.Clear();
        enemyKilledInWave = 0;
        totalEnemiesInWave = 0;

        for (int i = 0; i < waveData.units.Count; i++)
        {
            SurvivalEnemy type = waveData.units[i];

            if (type != SurvivalEnemy.Bomber)
            {
                totalEnemiesInWave++;
            }
        }

        DebugCustom.Log(string.Format("Wave {0} has {1} enemies", numberWave, totalEnemiesInWave));
    }

    //private IEnumerator CoroutineStartWave(SO_SurvivalWave waveData)
    //{
    //    int totalWaves = waveData.Time.Count;
    //    int curTimeIndex = 0;

    //    int totalItems = waveData.TimeDropItem.Count;
    //    int curItemIndex = 0;

    //    while (GameController.Instance.IsPaused == false && isPreparingNextWave == false)
    //    {
    //        // Spawn items
    //        if (curItemIndex < totalItems)
    //        {
    //            if (timerPerWave == waveData.TimeDropItem[curItemIndex].time)
    //            {
    //                List<ItemDropData> items = new List<ItemDropData> { waveData.TimeDropItem[curItemIndex].item };
    //                int randomIndex = UnityEngine.Random.Range(0, Map.itemSpawnPoints.Length);
    //                Vector2 pos = Map.itemSpawnPoints[randomIndex].position;
    //                GameController.Instance.itemDropController.Spawn(items, pos);
    //                curItemIndex++;
    //            }
    //        }

    //        // Spawn Units
    //        if (curTimeIndex < totalWaves)
    //        {
    //            if (timerPerWave == waveData.Time[curTimeIndex].time)
    //            {
    //                List<SurvivalUnitData> packUnits = waveData.Time[curTimeIndex].units;

    //                for (int i = 0; i < packUnits.Count; i++)
    //                {
    //                    SurvivalUnitData pack = packUnits[i];

    //                    for (int j = 0; j < pack.quantity; j++)
    //                    {
    //                        List<int> locations = Map.GetLocationCanSpawnUnit(pack.type);

    //                        if (locations.Count > 0)
    //                        {
    //                            int randomIndex = UnityEngine.Random.Range(0, locations.Count);
    //                            Map.AddUnitToSpawnLocation(pack.type, locations[randomIndex], waveData.MinLevelUnit, waveData.MaxLevelUnit);
    //                        }
    //                    }
    //                }

    //                Map.Spawn();
    //                curTimeIndex++;
    //            }
    //        }

    //        yield return StaticValue.waitOneSec;
    //        timerPerWave++;
    //        UpdateWaveTime();
    //    }
    //}

    private IEnumerator CoroutineStartWave(SurvivalWaveData waveData)
    {
        int totalWaves = 3;
        int curTimeIndex = 0;
        int totalUnits = waveData.numberUnits;
        int quantityWave_1 = Mathf.RoundToInt(totalUnits / 3f);
        int quantityWave_2 = Mathf.RoundToInt(totalUnits / 3f);
        int quantityWave_3 = totalUnits - (quantityWave_1 + quantityWave_2);
        DebugCustom.Log(string.Format("Total units={0}, split 3 waves={1},{2},{3}", totalUnits, quantityWave_1, quantityWave_2, quantityWave_3));

        while (GameController.Instance.IsPaused == false && isPreparingNextWave == false)
        {
            // Spawn Units
            if (curTimeIndex < totalWaves)
            {
                if (timerPerWave == 1 || timerPerWave == 10 || timerPerWave == 20)
                {
                    int startLoopIndex = 0;
                    int endLoopIndex = 0;

                    if (timerPerWave == 1)
                    {
                        startLoopIndex = 0;
                        endLoopIndex = quantityWave_1;
                    }
                    else if (timerPerWave == 10)
                    {
                        startLoopIndex = quantityWave_1;
                        endLoopIndex = quantityWave_1 + quantityWave_2;
                    }
                    else if (timerPerWave == 20)
                    {
                        startLoopIndex = quantityWave_1 + quantityWave_2;
                        endLoopIndex = totalUnits;
                    }

                    DebugCustom.Log(string.Format("Time={0}, Spawn {1} units", timerPerWave, endLoopIndex - startLoopIndex));

                    for (int i = startLoopIndex; i < endLoopIndex; i++)
                    {
                        SurvivalEnemy type = currentWaveData.units[i];
                        List<int> locations = Map.GetLocationCanSpawnUnit(type);

                        if (locations.Count > 0)
                        {
                            int randomIndex = UnityEngine.Random.Range(0, locations.Count);
                            Map.AddUnitToSpawnLocation(type, locations[randomIndex], currentWaveData.minLevelUnit, currentWaveData.maxLevelUnit);
                        }
                    }

                    Map.Spawn();
                    curTimeIndex++;
                }
            }

            yield return StaticValue.waitOneSec;
            timerPerWave++;
            UpdateWaveTime();
        }
    }

    private IEnumerator CoroutineCountDownNextWave()
    {
        int remainingTime = 3;

        while (remainingTime > 0)
        {
            string msg = string.Format("NEXT WAVE IN: {0}", remainingTime);
            UIController.Instance.modeSurvivalUI.ShowNotice(msg);
            remainingTime--;
            yield return StaticValue.waitOneSec;
        }

        StartNextWave();
        UIController.Instance.modeSurvivalUI.SetTextWave(numberWave);
        UIController.Instance.modeSurvivalUI.HideNotice();
        coroutineCountDownNextWave = null;
    }

    //private TriggerPointBoss GetBossPoint(int id)
    //{
    //    for (int i = 0; i < Map.pointBosses.Length; i++)
    //    {
    //        TriggerPointBoss pointBoss = Map.pointBosses[i];

    //        if (pointBoss.bossPrefab.id == id)
    //        {
    //            return pointBoss;
    //        }
    //    }

    //    return null;
    //}

    private int GetMostUsedGunId()
    {
        // Update gun profile
        int gunId = ProfileManager.UserProfile.gunNormalId;
        Dictionary<int, int> gunProfile;

        if (string.IsNullOrEmpty(ProfileManager.UserProfile.tournamentGunProfile))
        {
            gunProfile = new Dictionary<int, int>();
            gunProfile.Add(gunId, 1);
        }
        else
        {
            gunProfile = JsonConvert.DeserializeObject<Dictionary<int, int>>(ProfileManager.UserProfile.tournamentGunProfile);

            if (gunProfile.ContainsKey(gunId))
            {
                int useTimes = gunProfile[gunId];
                useTimes++;
                gunProfile[gunId] = useTimes;
            }
            else
            {
                gunProfile.Add(gunId, 1);
            }
        }

        string s = JsonConvert.SerializeObject(gunProfile);
        ProfileManager.UserProfile.tournamentGunProfile.Set(s);
        ProfileManager.SaveAll();

        // Get most used gun
        int mostUseTimes = 0;
        int mostUseGunId = ProfileManager.UserProfile.gunNormalId;

        foreach (KeyValuePair<int, int> guns in gunProfile)
        {
            if (guns.Value > mostUseTimes)
            {
                mostUseTimes = guns.Value;
                mostUseGunId = guns.Key;
            }
        }

        DebugCustom.Log("Most use gun id=" + mostUseGunId);
        return mostUseGunId;
    }

    private int GetUnitScore(int id, int level)
    {
        if (unitScoreData.ContainsKey(id))
        {
            int scorePerUnit = unitScoreData[id];
            int score = Mathf.RoundToInt(scorePerUnit * ((float)level / 2f));
            return score;
        }
        else
        {
            DebugCustom.LogError("[GetUnitScore] Key not found=" + id);
            return 0;
        }
    }

    private void LoadScoreData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(StaticValue.PATH_JSON_SURVIVAL_UNIT_SCORE_DATA);
        unitScoreData = JsonConvert.DeserializeObject<Dictionary<int, int>>(textAsset.text);
    }


    public void NextWave()
    {
        totalEnemiesInWave = 0;

        foreach (BaseUnit unit in GameController.Instance.activeUnits.Values)
        {
            if (unit.CompareTag(StaticValue.TAG_ENEMY))
            {
                unit.TakeDamage(1000000);
            }
        }
    }
}
