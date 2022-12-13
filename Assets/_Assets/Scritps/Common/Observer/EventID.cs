public enum EventID
{
    None = 0,

    // UI
    ClickButtonJump,
    ClickButtonShoot,
    ClickButtonThrowGrenade,
    ToggleAutoFire,
    ToggleSwitchGun,
    SelectBooster,

    // Player
    TimeOutComboKill,
    GetItemDrop,
    GetGunDrop,
    OutOfAmmo,
    PlayerDie,
    ReviveByGem,
    ReviveByAds,
    UseGrenade,

    // Enemy
    BulletMegatronOutScreen,
    LaserPoisonHitGround,
    RocketMegatankHitPlayer,
    RocketMegatankMissPlayer,
    EnemyShootHitPlayer,
    UnitDie,

    // Game Play
    GameStart,
    GameEnd,
    FinishStage,
    TransportJetDone,
    WarningBossDone,
    SelectBoosterDone,
    UseBoosterHP,
    GetCoinCompleteQuest,
    GetGemCompleteQuest,
    GetExpCompleteQuest,
    OutGamePlay,

    // Vehicle
    BoatTriggerWater,
    BoatTriggerObstacle,
    BoatStop,

    // Map
    EnterHelicopterArena,
    EnterZone,
    ClearZone,
    MoveCameraToNewZone,

    // Quest
    GetComboKill,
    GetComboKillBySpecialGun,
    KillEnemyByGrenade,
    KillEnemyByKnife,
    KillEnemyBySpecialGun,
    KillTankByGrenade,
    KillBossMonkeyByGrenade,

    // Boss
    ShowInfoBossMegatron,
    ShowInfoBossDone,
    FinalBossStart,
    FinalBossDie,
    BossSubmarineDie,
    BossProfessorSatelliteDie,
    BossMonkeyDie,
    BossMonkeySpikeTrapStart,
    BossMonkeySpikeTrapEnd,
    BossMonkeySpikeHitPlayer,
    BossMonkeySpikeDeactive,

    // Upgrade
    SwichTabUpgradeWeapon,
    SelectWeaponCellView,
    PreviewDummyTakeDamage,

    // World Map
    ClickStageOnWorldMap,
    AnimateWorldMapDone,
    SelectDifficulty,
    ClaimCampaignBox,

    // Daily
    CheckTimeNewDayDone,
    ClaimDailyGift,

    // Resources
    ReceiveCoin,
    ReceiveGem,
    ReceiveStamina,
    ReceiveMedal,

    ConsumeCoin,
    ConsumeGem,
    ConsumeStamina,
    ConsumeMedal,

    // Profile
    ReceiveExp,
    LevelUp,

    // IAP
    BuyStarterPack,
    BuySpecialOffer,

    // Free rewards
    ViewAdsGetFreeCoin,
    ViewAdsx2CoinEndGame,
    ShareFacebookSuccess,

    // Daily Quest
    ClaimDailyQuestReward,
    MaintainDailyQuest,
    NewDay,

    CompleteAllDailyQuests,
    FinishStageWith3Stars,
    KillEnemyRifle,
    KillEnemyKnife,
    KillEnemyGeneral,
    KillEnemyGrenade,
    KillEnemyFire,
    KillEnemyTank,
    UseBooster,
    UseBoosterCritical,
    UseBoosterDamage,
    UseBoosterSpeed,
    UseBoosterCoinMagnet,
    BuyBooster,
    GetCriticalHit,
    BuyAmmo,
    BuyGrenade,
    CompleteSurvivalWave5,

    // Achievement
    ClaimAchievementReward,
    KillEnemyFlying,
    KillEnemySniperByGunAWP,
    GrenadeKillEnemyAtOnce,
    KillEnemySniper,
    CompleteStageWithoutReviving,

    // New version
    NewVersionAvailable,

    // Tournament
    StartFirstWave,
    CompleteWave,
    QuitSurvivalSession,
    CompleteSurvivalSession,
    LabelWaveAnimateComplete,
    BossSurvivalDie,
    GetFacebookAvatarDone,
    GetFacebookNameDone,
    UseSupportItemHP,
    UseSupportItemGrenade,
    UseSupportItemBomb,
    UseSupportItemBooster,
    ClaimTournamentRankReward,

    // Skills
    BonusCoinCollected,
    BonusExpWin,
    RamboActiveSkill,
    ActiveReflectShield,
    ActiveBomb,
    ActiveRage,
    UpgradeSkillSuccess,
    ResetUISkillTree,
    ClickNodeSkill,
    ClickStartTournament,

    // Tutorial
    CompleteStep,
    CompleteSubStep,

    SubStepSelectStage,

    SubStepSelectBooster,
    SubStepBuyBooster,

    SubStepSelectUzi,
    SubStepUpgradeUziTolevel2,
    SubStepUpgradeUziTolevel3,
    SubStepSwitchSpecialTab,
    SubStepSelectKame,

    SubStepUpgradeRamboToLevel2,
    SubStepEnterSkillTree,
    SubStepSelectSkillPhoenixDown,
    SubStepUnlockSkillPhoenixDown,
    SubStepGoUpgradeWeaponFromLose,
    SubStepGoUpgradeCharacterFromLose,

    SubStepClickButtonFreeGift,

    SubStepClickMission,
    SubStepClickAchievement,
}