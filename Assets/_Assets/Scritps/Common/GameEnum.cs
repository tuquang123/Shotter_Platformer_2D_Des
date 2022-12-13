public enum GameMode
{
    Campaign,
    Survival
}

public enum MapType
{
    Map_1_Desert = 1,
    Map_2_Lab,
    Map_3_Jungle
}

public enum Difficulty
{
    Normal,
    Hard,
    Crazy
}

public enum StatsType
{
    Damage,
    Hp,
    MaxHp,
    MoveSpeed,
    AttackTimePerSecond,
    CriticalRate,
    CriticalDamageBonus
}

public enum EquipmentOptionAdjust
{
    Percent,
    Point
}

public enum PlayerState
{
    Idle,
    Move,
    Crouch,
    Jump,
    Die
}

public enum EnemyState
{
    None,
    Idle,
    Patrol,
    Attack,
    Die
}

public enum ModifierType
{
    AddPoint,
    AddPercentBase
}

public enum WeaponType
{
    None,
    NormalGun,
    SpecialGun,
    MeleeWeapon,
    Grenade,
    DropGun
}

public enum GunType
{
    Pistol,
    Rifle,
}

public enum MeleeWeaponType
{
    Knife,
    Pan,
    Guitar
}

public enum CameraLockDirection
{
    Left,
    Right,
    Up,
    Down
}

public enum MapTriggerPointType
{
    EnterZone,
    LockZone,
    LoadNextEnemyPack,
}

public enum EffectObjectName
{
    None,
    BulletImpactNormal,
    BulletImpactExplodeSmall,
    BulletImpactExplodeMedium,
    ExplosionBomb,
    ExplosionGas,
    WoodBoxBroken,
    BulletImpactExplodeLarge,
    ExplosionC4,
    ExplosionMultiple,
    StoneRainExplosion,
    StoneBrokenSmall,
    StoneBrokenMedium,
    GroundSmoke,
    BulletImpactSplitGun,
    BulletImpactTeslaMini,
}

public enum WaypointAction
{
    Move,
    Camp,
    Jump,
    Crouch,
    CrouchMoveForward,
    JumpForward
}

public enum ItemDropType
{
    Health,
    Ammo,
    Coin,
    GunSpread,
    GunRocketChaser,
    GunFamas,
    GunLaser,
    GunSplit,
    GunFireBall,
    GunTesla,
    GunKamePower,
    GunFlame
}

public enum ControllerType
{
    Rambo,
    Boat
}

public enum PopupType
{
    YesNo,
    Ok
}

public enum BoosterType
{
    None = -1,
    Hp,
    Grenade,
    Damage,
    CoinMagnet,
    Speed,
    Critical
}

public enum MainMenuNavigation
{
    None = 0,
    OpenWorldMap,
    ShowUpgradeWeapon,
    ShowUpgradeSoldier,
    ShowUpgradeSkill,
}


public enum WorldMapNavigation
{
    None = 0,
    NextStageFromGame,
}


public enum RewardType
{
    Coin = 0,
    Gem = 1,
    Exp = 2,
    Stamina = 3,
    Medal = 4,
    TournamentTicket = 5,

    BoosterHp = 50,
    BoosterDamage = 51,
    BoosterCoinMagnet = 52,
    BoosterSpeed = 53,
    BoosterCritical = 54,

    GunM4 = 100,
    GunSpread = 101,
    GunScarH = 102,
    GunBullpup = 103,
    GunKamePower = 104,
    GunSniperRifle = 105,
    GunTeslaMini = 106,
    GunLaser = 107,
    GunFlame = 108,

    GrenadeF1 = 200,
    GrenadeTet = 201,

    MeleeWeaponPan = 250,
    MeleeWeaponGuitar = 251,
}

public enum RankBenefitType
{
    MaxHpPackIngame,
    BonusCoin,
    BonusExp,
}

public enum WayToObtain
{
    Coin,
    Gem,
    Medal,
    BigSalePacks,
    DailyQuest,
    StarterPack,
}

public enum AppStatus
{
    Normal = 0,
    ShouldUpdate,
    ForceUpdate,
}

public enum SurvivalEnemy
{
    Rifle = 0,
    General = 1,
    Grenade = 2,
    Knife = 3,
    Sniper = 4,
    Fire = 5,
    Parachutist = 7,
    Monkey = 8,
    Bazooka = 11,
    Helicopter = 100,
    Bomber = 101,
    Tank = 102,
    TankCannon = 103,
    BossMegatron = 1000,
    BossMegatank = 1001,
    BossVenom = 1002,
    BossProfessor = 1004,
    BossMonkey = 1005
}

public enum BossType
{
    None = 0,
    Megatron = 1000,
    Megatank = 1001,
    Venom = 1002,
    Submarine = 1003,
    Professor = 1004,
    Monkey = 1005
}

public enum DebuffType
{
    None = 0,
    Stun,
    Freeze,
    Knockback,
    Burn,
    Electric,
    TakeMoreDamageWhenHighHP,
    Reflect,
}

public enum SkillType
{
    Passive,
    Active
}

public enum SkillCatergory
{
    Offense,
    Defense,
    Utility
}

public enum ToastLength
{
    Normal,
    Long
}

public enum TournamentRank
{
    Ducky = 0,
    Bronze,
    Silver,
    Gold,
    Platinum,
    Diamond,
    Legend
}

public enum ShowResult
{
    Failed = 0,
    Skipped = 1,
    Finished = 2,
}

public enum WeaponStatsGrade
{
    Grade_C,
    Grade_B,
    Grade_A,
    Grade_S,
    Grade_SS,
}

public enum TutorialType
{
    WorldMap,
    Booster,
    ActionInGame,
    Weapon,
    Character,
    MenuFeatures,
    RecommendUpgradeCharacter,
    RecommendUpgradeWeapon,
    FreeGift,
    Mission,
}
