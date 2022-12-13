using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValue
{
    // Joystick
    public const string JOYSTICK_HORIZONTAL = "Horizontal";
    public const string JOYSTICK_VERTICAL = "Vertical";
    public const float VERTICAL_VALUE_CHANGE = 0.25f;
    public const float HORIZONTAL_VALUE_CHANGE = 0.25f;

    // Layer
    public static readonly int LAYER_DEFAULT = 0;
    public static readonly int LAYER_OBSTACLE = LayerMask.NameToLayer("Obstacle");
    public static readonly int LAYER_GROUND = LayerMask.NameToLayer("Ground");
    public static readonly int LAYER_BODY_ENEMY = LayerMask.NameToLayer("Body Enemy");
    public static readonly int LAYER_FOOT_ENEMY = LayerMask.NameToLayer("Foot Enemy");
    public static readonly int LAYER_VEHICLE_ENEMY = LayerMask.NameToLayer("Vehicle Enemy");
    public static readonly int LAYER_BULLET_RAMBO = LayerMask.NameToLayer("Bullet Rambo");
    public static readonly int LAYER_BULLET_ENEMY = LayerMask.NameToLayer("Bullet Enemy");

    // Tag
    public const string TAG_NONE = "Untagged";
    public const string TAG_MAP = "Map";
    public const string TAG_PLAYER = "Player";
    public const string TAG_ENEMY = "Enemy";
    public const string TAG_DESTRUCTIBLE_OBSTACLE = "Destructible Obstacle";
    public const string TAG_BULLET_RAMBO = "Bullet Rambo";
    public const string TAG_BULLET_ENEMY = "Bullet Enemy";
    public const string TAG_FINISH = "Finish";
    public const string TAG_ENEMY_BODY_PART = "Enemy Body Part";
    public const string TAG_GETOUT_VEHICLE = "Get out Vehicle";

    // Scene
    public const string SCENE_LOADING = "Loading";
    public const string SCENE_GAME_PLAY = "GamePlay";
    public const string SCENE_LOGIN = "Login";
    public const string SCENE_MENU = "Menu";

    // Map
    public const string NAME_MAP_1_DESERT = "STORM DESERT";
    public const string NAME_MAP_2_LAB = "MUTANT LAB";
    public const string NAME_MAP_3_JUNGLE = "SILENT JUNGLE";

    // Rambo Ids
    public const int RAMBO_ID_JOHN = 0;

    // Boss Ids
    public const int ID_BOSS_MEGATRON = 1000;
    public const int ID_BOSS_MEGATANK = 1001;
    public const int ID_BOSS_VENOM = 1002;
    public const int ID_BOSS_SUBMARINE = 1003;
    public const int ID_BOSS_PROFESSOR = 1004;
    public const int ID_BOSS_MONKEY = 1005;

    // Weapon IDs
    public const int GUN_ID_UZI = 0;
    public const int GUN_ID_M4 = 1;
    public const int GUN_ID_SCAR_H = 2;
    public const int GUN_ID_AWP = 3;
    public const int GUN_ID_SHOTGUN = 4;
    public const int GUN_ID_P100 = 5;
    public const int GUN_ID_BULLPUP = 6;
    public const int GUN_ID_SNIPER_RIFLE = 7;
    public const int GUN_ID_TESLA_MINI = 8;
    public const int GUN_ID_SPREAD = 100;
    public const int GUN_ID_ROCKET_CHASER = 101;
    public const int GUN_ID_FAMAS = 102;
    public const int GUN_ID_LASER = 103;
    public const int GUN_ID_SPLIT = 104;
    public const int GUN_ID_FIRE_BALL = 105;
    public const int GUN_ID_TESLA = 106;
    public const int GUN_ID_KAME_POWER = 107;
    public const int GUN_ID_FLAME = 108;

    public const int GRENADE_ID_F1 = 500;
    public const int GRENADE_ID_TET_HOLIDAY = 501;

    public const int MELEE_WEAPON_ID_KNIFE = 600;
    public const int MELEE_WEAPON_ID_PAN = 601;
    public const int MELEE_WEAPON_ID_GUITAR = 602;

    // Game play
    public const int LEVEL_INCREASE_MODE_HARD = 2;
    public const int LEVEL_INCREASE_MODE_CRAZY = 7;
    public const int MAX_LEVEL_ENEMY = 20;

    public const int SHARE_FACEBOOK_GEM_REWARD = 5;
    public const int SHARE_FACEBOOK_LIMIT_TIMES = 5;

    public const int COST_REVIVE_BY_GEM = 30;
    public const int COST_GEM_PER_POINT_RESET = 30;

    public const int VIEW_INTERSTITIAL_GEM_REWARD = 5;
    public const int VIEW_INTERSTITIAL_LIMIT_REWARD_TIMES = 10;

    // Tournament
    public const int TOURNAMENT_FREE_ENTRANCE = 2;
    public const int TOURNAMENT_MAX_ENTRANCE = 5;
    public const int COST_ENTRANCE_TOURNAMENT_3RD = 75;
    public const int COST_ENTRANCE_TOURNAMENT_4TH = 150;
    public const int COST_ENTRANCE_TOURNAMENT_5TH = 300;
    public const int COST_SUPPORT_ITEM_HP = 40;
    public const int COST_SUPPORT_ITEM_BOMB = 20;
    public const int COST_SUPPORT_ITEM_BOOSTER = 10;
    public const int COST_SUPPORT_ITEM_GRENADE = 5000;
    public const int SUPPORT_ITEM_GRENADE_INCREASE = 10;

    // Resources Path
    public const string PATH_RAMBO_PREFAB = "Rambo/";
    public const string PATH_MAP_PREFAB = "Map/";

    public const string PATH_GUN_PREFAB = "Gun/";
    public const string PATH_GRENADE_PREFAB = "Grenade/";
    public const string PATH_MELEE_WEAPON_PREFAB = "Melee Weapon/";

    public const string PATH_GUN_PREVIEW_PREFAB = "Gun Preview/";
    public const string PATH_GRENADE_PREVIEW_PREFAB = "Grenade Preview/";
    public const string PATH_MELEE_WEAPON_PREVIEW_PREFAB = "Melee Weapon Preview/";

    public const string PATH_JSON_STAGE_QUEST_DESCRIPTION = "JSON/Mix/stage_quest_description";
    public const string PATH_JSON_RANK_NAME = "JSON/Mix/rank_name";
    public const string PATH_JSON_TIPS = "JSON/Mix/tips";
    public const string PATH_JSON_EXCHANGE_WEAPON_TO_GEM = "JSON/Mix/weapon_exchange_to_gem";

    public const string PATH_JSON_MAP_ENEMY_DATA = "JSON/Map Enemy Data/";
    public const string PATH_JSON_CAMPAIGN_STAGE_LEVEL_DATA = "JSON/Mix/campaign_stage_level_data";

    public const string PATH_JSON_STATIC_TOURNAMENT_RANK_DATA = "JSON/Tournament Data/tournament_rank_data";
    public const string PATH_JSON_SURVIVAL_UNIT_SCORE_DATA = "JSON/Tournament Data/tournament_survival_unit_score_data";
    public const string PATH_JSON_TOURNAMENT_TOP_RANK_REWARD = "JSON/Tournament Data/tournament_top_rank_reward_data";

    public const string PATH_JSON_STATIC_RECOMMEND_GUN_DATA = "JSON/Static Data/static_recommend_gun_data";
    public const string PATH_JSON_STATIC_GUN_DATA = "JSON/Static Data/static_gun_data";
    public const string PATH_JSON_STATIC_GRENADE_DATA = "JSON/Static Data/static_grenade_data";
    public const string PATH_JSON_STATIC_MELEE_WEAPON_DATA = "JSON/Static Data/static_melee_weapon_data";
    public const string PATH_JSON_STATIC_RAMBO_DATA = "JSON/Static Data/static_rambo_data";
    public const string PATH_JSON_STATIC_RAMBO_SKILL_DATA = "JSON/Static Data/static_rambo_skill_data";
    public const string PATH_JSON_STATIC_CAMPAIGN_STAGE_DATA = "JSON/Static Data/static_campaign_stage_data";
    public const string PATH_JSON_STATIC_CAMPAIGN_BOX_REWARD_DATA = "JSON/Static Data/static_campaign_box_reward_data";
    public const string PATH_JSON_STATIC_BOOSTER_DATA = "JSON/Static Data/static_booster_data";
    public const string PATH_JSON_STATIC_DAILY_QUEST_DATA = "JSON/Static Data/static_daily_quest_data";
    public const string PATH_JSON_STATIC_ACHIEVEMENT_DATA = "JSON/Static Data/static_achievement_data";
    public const string PATH_JSON_STATIC_RANK_DATA = "JSON/Static Data/static_rank_data";
    public const string PATH_JSON_STATIC_FREE_GIFT_DATA = "JSON/Static Data/static_free_gift_data";

    public const string PATH_JSON_NEW_PLAYER_PROFILE = "JSON/New Player Data/new_player_profile";
    public const string PATH_JSON_NEW_PLAYER_CAMPAIGN_PROGRESS_DATA = "JSON/New Player Data/new_player_campaign_progress_data"; // Old
    public const string PATH_JSON_NEW_PLAYER_CAMPAIGN_STAGE_PROGRESS_DATA = "JSON/New Player Data/new_player_campaign_stage_progress_data"; // New
    public const string PATH_JSON_NEW_PLAYER_CAMPAIGN_REWARD_PROGRESS_DATA = "JSON/New Player Data/new_player_campaign_reward_progress_data";
    public const string PATH_JSON_NEW_PLAYER_GRENADE_DATA = "JSON/New Player Data/new_player_grenades_data";
    public const string PATH_JSON_NEW_PLAYER_GUN_DATA = "JSON/New Player Data/new_player_gun_data";
    public const string PATH_JSON_NEW_PLAYER_RAMBO_DATA = "JSON/New Player Data/new_player_rambo_data";
    public const string PATH_JSON_NEW_PLAYER_RAMBO_SKILL_DATA = "JSON/New Player Data/new_player_rambo_skill_data";
    public const string PATH_JSON_NEW_PLAYER_RESOURCES_DATA = "JSON/New Player Data/new_player_resources_data";
    public const string PATH_JSON_NEW_PLAYER_MELEE_WEAPON_DATA = "JSON/New Player Data/new_player_melee_weapon_data";
    public const string PATH_JSON_NEW_PLAYER_BOOSTER_DATA = "JSON/New Player Data/new_player_booster_data";
    public const string PATH_JSON_NEW_PLAYER_SELECTING_BOOSTER = "JSON/New Player Data/new_player_selecting_booster";
    public const string PATH_JSON_NEW_PLAYER_DAILY_QUEST_DATA = "JSON/New Player Data/new_player_daily_quest_data";
    public const string PATH_JSON_NEW_PLAYER_ACHIEVEMENT_DATA = "JSON/New Player Data/new_player_achievement_data";
    public const string PATH_JSON_NEW_PLAYER_TUTORIAL_DATA = "JSON/New Player Data/new_player_tutorial_data";

    public const string PATH_IMAGE_GUN = "Sprites/Gun/";
    public const string PATH_IMAGE_GRENADE = "Sprites/Grenade/";
    public const string PATH_IMAGE_MELEE_WEAPON = "Sprites/Melee Weapon/";
    public const string PATH_IMAGE_ENEMY_ICON = "Sprites/Enemy Icon/";
    public const string PATH_IMAGE_SKILL_LOCK = "Sprites/Skill Lock/";
    public const string PATH_IMAGE_SKILL_UNLOCK = "Sprites/Skill Unlock/";
    public const string PATH_IMAGE_RANK_ICON = "Sprites/Rank Icon/";
    public const string PATH_IMAGE_TOURNAMENT_RANK_ICON = "Sprites/Tournament Rank Icon/";

    // Scriptable Object Path
    public const string FORMAT_PATH_BASE_STATS_RAMBO = "Scriptable Object/Rambo/{0}/rambo_{0}_lv{1}";

    public const string FORMAT_PATH_BASE_STATS_ENEMY_RIFLE = "Scriptable Object/Enemy/Enemy Rifle/enemy_rifle_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_GENERAL = "Scriptable Object/Enemy/Enemy General/enemy_general_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_GRENADE = "Scriptable Object/Enemy/Enemy Grenade/enemy_grenade_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_KNIFE = "Scriptable Object/Enemy/Enemy Knife/enemy_knife_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_HELICOPTER = "Scriptable Object/Enemy/Enemy Helicopter/enemy_helicopter_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_BOMBER = "Scriptable Object/Enemy/Enemy Bomber/enemy_bomber_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_TANK = "Scriptable Object/Enemy/Enemy Tank/enemy_tank_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_TANK_CANNON = "Scriptable Object/Enemy/Enemy Tank Cannon/enemy_tank_cannon_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_SNIPER = "Scriptable Object/Enemy/Enemy Sniper/enemy_sniper_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_FIRE = "Scriptable Object/Enemy/Enemy Fire/enemy_fire_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_MARINE = "Scriptable Object/Enemy/Enemy Marine/enemy_marine_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_PARACHUTIST = "Scriptable Object/Enemy/Enemy Parachutist/enemy_parachutist_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_MONKEY = "Scriptable Object/Enemy/Enemy Monkey/enemy_monkey_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_SPIDER = "Scriptable Object/Enemy/Enemy Spider/enemy_spider_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_ENEMY_BAZOOKA = "Scriptable Object/Enemy/Enemy Bazooka/enemy_bazooka_lv{0}";

    public const string FORMAT_PATH_BASE_STATS_BOSS_MEGATRON = "Scriptable Object/Boss/Boss Megatron/boss_megatron_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_BOSS_MEGATANK = "Scriptable Object/Boss/Boss Megatank/boss_megatank_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_BOSS_VENOM = "Scriptable Object/Boss/Boss Venom/boss_venom_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_BOSS_SUBMARINE = "Scriptable Object/Boss/Boss Submarine/boss_submarine_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_BOSS_PROFESSOR = "Scriptable Object/Boss/Boss Professor/boss_professor_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_BOSS_MONKEY = "Scriptable Object/Boss/Boss Monkey/boss_monkey_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_BOSS_MONKEY_MINION = "Scriptable Object/Boss/Boss Monkey Minion/boss_monkey_minion_lv{0}";

    // Normal guns
    public const string FORMAT_PATH_BASE_STATS_GUN_UZI = "Scriptable Object/Gun/Uzi/gun_uzi_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_M4 = "Scriptable Object/Gun/M4CQB/gun_M4_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_SCAR_H = "Scriptable Object/Gun/ScarH/gun_scar_H_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_AWP = "Scriptable Object/Gun/AWP/gun_awp_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_SHOTGUN = "Scriptable Object/Gun/Shotgun/shotgun_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_P100 = "Scriptable Object/Gun/P100/gun_p100_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_BULLPUP = "Scriptable Object/Gun/Bullpup/gun_bullpup_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_SNIPER_RIFLE = "Scriptable Object/Gun/Sniper Rifle/gun_sniper_rifle_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_TESLA_MINI = "Scriptable Object/Gun/Tesla Mini/gun_tesla_mini_lv{0}";

    // Special guns
    public const string FORMAT_PATH_BASE_STATS_GUN_SPREAD = "Scriptable Object/Gun/Spread/gun_spread_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_ROCKET_CHASER = "Scriptable Object/Gun/Rocket Chaser/gun_rocket_chaser_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_FAMAS = "Scriptable Object/Gun/Famas/gun_famas_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_LASER = "Scriptable Object/Gun/Laser/gun_laser_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_SPLIT = "Scriptable Object/Gun/Split/gun_split_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_FIRE_BALL = "Scriptable Object/Gun/Fire Ball/gun_fire_ball_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_TESLA = "Scriptable Object/Gun/Tesla/gun_tesla_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_KAME_POWER = "Scriptable Object/Gun/Kame Power/gun_kame_power_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUN_FLAME = "Scriptable Object/Gun/Flame/gun_flame_lv{0}";

    public const string FORMAT_PATH_BASE_STATS_GRENADE = "Scriptable Object/Grenade/Grenade Base/grenade_base_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GRENADE_TET = "Scriptable Object/Grenade/Grenade Tet/grenade_Tet_lv{0}";

    public const string FORMAT_PATH_BASE_STATS_KNIFE = "Scriptable Object/Melee Weapon/Knife/knife_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_PAN = "Scriptable Object/Melee Weapon/Pan/pan_lv{0}";
    public const string FORMAT_PATH_BASE_STATS_GUITAR = "Scriptable Object/Melee Weapon/Guitar/guitar_lv{0}";


    // Objects Path
    public const string PATH_TRIGGER_POINT_BOMBER_PREFAB = "Objects/trigger_point_bomber";
    public const string PATH_TRIGGER_POINT_HELICOPTER_PREFAB = "Objects/trigger_point_helicopter";

    // Quest key mapping
    public const string KEY_DESCRIPTION_QUEST_FINISH_STAGE = "finish_stage";
    public const string KEY_DESCRIPTION_QUEST_FINISH_STAGE_IN_TIME = "finish_stage_in_time";
    public const string KEY_DESCRIPTION_QUEST_FINISH_STAGE_WITHOUT_REVIVE = "finish_stage_without_revive";
    public const string KEY_DESCRIPTION_QUEST_COMBO_KILL = "combo_kill";
    public const string KEY_DESCRIPTION_QUEST_COMBO_KILL_BY_SPECIAL_GUN = "combo_kill_by_special_gun";
    public const string KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_GRENADE = "kill_enemy_by_grenade";
    public const string KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_KNIFE = "kill_enemy_by_knife";
    public const string KEY_DESCRIPTION_QUEST_KILL_ENEMY_BY_SPECIAL_GUN = "kill_enemy_by_special_gun";
    public const string KEY_DESCRIPTION_QUEST_KILL_FINAL_BOSS = "kill_final_boss";
    public const string KEY_DESCRIPTION_QUEST_KILL_FINAL_BOSS_IN_TIME = "kill_final_boss_in_time";
    public const string KEY_DESCRIPTION_QUEST_KILL_TANK_BY_GRENADE = "kill_tank_by_grenade";
    public const string KEY_DESCRIPTION_QUEST_REMAINING_HP = "remaining_hp";
    public const string KEY_DESCRIPTION_QUEST_BOAT_NOT_HIT_OBSTACLE = "boat_not_hit_obstacle";
    public const string KEY_DESCRIPTION_QUEST_USE_BOOSTER_DAMAGE = "use_booster_damage";
    public const string KEY_DESCRIPTION_QUEST_USE_GRENADES = "use_grenades";
    public const string KEY_DESCRIPTION_KILL_BOSS_MONKEY_BY_GRENADE = "kill_boss_monkey_by_grenade";

    // Sound
    public const string SOUND_MUSIC_SURVIVAL = "music_survival";
    public const string SOUND_MUSIC_MAP_1 = "music_map_1";
    public const string SOUND_MUSIC_MAP_2 = "music_map_2";
    public const string SOUND_MUSIC_MAP_3 = "music_map_3";
    public const string SOUND_MUSIC_BOSS = "music_boss";
    public const string SOUND_MUSIC_MENU = "music_menu";
    public const string SOUND_MUSIC_WIN = "music_win";
    public const string SOUND_MUSIC_LOSE = "music_lose";
    public const string SOUND_SFX_CARTOUCHE = "sfx_cartouche";
    public const string SOUND_SFX_EXPLOSIVE = "sfx_explosive";
    public const string SOUND_SFX_THROW_GRENADE = "sfx_throw_grenade";
    public const string SOUND_SFX_MILITARY_ALARM = "sfx_military_alarm";
    public const string SOUND_SFX_WARNING = "sfx_warning";
    public const string SOUND_SFX_DOOR_OPEN = "sfx_door_open";
    public const string SOUND_SFX_DOOR_CLOSE = "sfx_door_close";
    public const string SOUND_SFX_TRIGGER_WATER = "sfx_trigger_water";
    public const string SOUND_SFX_PLANE_MOVE = "sfx_plane_move";
    public const string SOUND_SFX_VOICE_VICTORY = "sfx_voice_victory";
    public const string SOUND_SFX_VOICE_GAME_OVER = "sfx_voice_game_over";
    public const string SOUND_SFX_TEXT_TYPING = "sfx_text_typing";
    public const string SOUND_SFX_GET_REWARD = "sfx_get_reward";
    public const string SOUND_SFX_PURCHASE_SUCCESS = "sfx_purchase_success";
    public const string SOUND_SFX_SHOW_DIALOG = "sfx_show_dialog";
    public const string SOUND_SFX_UPGRADE_SUCCESS = "sfx_upgrade_success";
    public const string SOUND_SFX_SHOW_DAILY_GIFT = "sfx_show_daily_gift";
    public const string SOUND_SFX_EQUIP_WEAPON = "sfx_equip_weapon";
    public const string SOUND_SFX_START_MISSION = "sfx_start_mission";
    public const string SOUND_SFX_ONE_STAR = "sfx_get_1_star";
    public const string SOUND_SFX_TWO_STAR = "sfx_get_2_star";
    public const string SOUND_SFX_THREE_STAR = "sfx_get_3_star";
    public const string SOUND_SFX_CRITICAL_HIT = "sfx_critical_hit";


    public static readonly WaitForSeconds waitHalfSec = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds waitOneSec = new WaitForSeconds(1f);
    public static readonly WaitForSeconds waitTwoSec = new WaitForSeconds(2f);

    public static readonly Color colorNotEnoughMoney = new Color(0.96f, 0.18f, 0.38f, 1f);
    public static readonly Color32 color32NotEnoughMoney = new Color32(245, 45, 98, 255);

    public static readonly DateTime defaultDate = new DateTime(1991, 11, 9);

#if UNITY_ANDROID
    public static string storeUrl = "https://play.google.com/store/apps/details?id=com.sevenapp.metalblackops";
#elif UNITY_IOS
    public static string storeUrl = "https://itunes.apple.com/us/app/metal-command/id1356050859";
#else
    public static string storeUrl = "https://play.google.com/store/apps/details?id=com.sevenapp.metalblackops";
#endif
}
