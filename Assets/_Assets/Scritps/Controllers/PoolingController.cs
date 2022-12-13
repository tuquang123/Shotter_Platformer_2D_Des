using UnityEngine;
using System.Collections;

public class PoolingController : Singleton<PoolingController>
{
    public Transform groupBullet;
    public Transform groupGrenade;
    public Transform groupEffect;
    public Transform groupText;

    // Rambo bullet
    public ObjectPooling<BulletUzi> poolBulletUzi;
    public ObjectPooling<BulletMachineGunM4> poolBulletMachineGunM4;
    public ObjectPooling<BulletScarHGun> poolBulletScarHGun;
    public ObjectPooling<BulletAWP> poolBulletAWP;
    public ObjectPooling<BulletShotgun> poolBulletShotgun;
    public ObjectPooling<BulletP100> poolBulletP100;
    public ObjectPooling<BulletBullpup> poolBulletBullpup;
    public ObjectPooling<BulletSniperRifle> poolBulletSniperRifle;
    public ObjectPooling<BulletTeslaMini> poolBulletTeslaMini;

    public ObjectPooling<BulletSpreadGun> poolBulletSpreadGun;
    public ObjectPooling<BulletRocketChaser> poolBulletRocketChaser;
    public ObjectPooling<BulletFamasGun> poolBulletFamasGun;
    public ObjectPooling<BulletSplitGun> poolBulletSplitGun;
    public ObjectPooling<FireBall> poolBulletFireBall;
    public ObjectPooling<BulletKamePower> poolBulletKamePower;

    public ObjectPooling<BombSupportSkill> poolBombSupportSkill;
    public ObjectPooling<BombSupportSurvival> poolBombSupportSurvival;

    // Enemy bullet
    public ObjectPooling<BulletRifle> poolBulletRifle;
    public ObjectPooling<BulletPistol> poolBulletPistol;
    public ObjectPooling<BulletSniper> poolBulletSniper;
    public ObjectPooling<BulletTank> poolBulletTank;
    public ObjectPooling<BulletTankCannon> poolBulletTankCannon;
    public ObjectPooling<Bomb> poolBulletBomb;
    public ObjectPooling<HomingMissile> poolHomingMissile;
    public ObjectPooling<BulletPlasma> poolBulletPlasma;
    public ObjectPooling<Torpedo> poolTorpedo;
    public ObjectPooling<BulletSpider> poolBulletSpider;
    public ObjectPooling<BulletBazooka> poolBulletBazooka;

    // Boss Bullet
    public ObjectPooling<BulletBossMegatron> poolBulletBossMegatron;
    public ObjectPooling<RocketBossMegatank> poolRocketBossMegatank;
    public ObjectPooling<RocketBossSubmarine> poolRocketBossSubmarine;
    public ObjectPooling<BulletPoisonBossVenom> poolBulletPoisonBossVenom;
    public ObjectPooling<BulletBossProfessor> poolBulletBossProfessor;
    public ObjectPooling<StoneBossMonkey> poolStoneBossMonkey;
    public ObjectPooling<StoneBossMonkeyMinion> poolStoneBossMonkeyMinion;

    // Grenade
    public ObjectPooling<BaseGrenade> poolBaseGrenade;
    public ObjectPooling<BaseGrenadeEnemy> poolBaseGrenadeEnemy;

    // Enemy
    public ObjectPooling<EnemyRifle> poolEnemyRifle;
    public ObjectPooling<EnemyGeneral> poolEnemyGeneral;
    public ObjectPooling<EnemyGrenade> poolEnemyGrenade;
    public ObjectPooling<EnemyKnife> poolEnemyKnife;
    public ObjectPooling<EnemyHelicopter> poolEnemyHelicopter;
    public ObjectPooling<EnemyTank> poolEnemyTank;
    public ObjectPooling<EnemyTankCannon> poolEnemyTankCannon;
    public ObjectPooling<EnemyBomber> poolEnemyBomber;
    public ObjectPooling<EnemySniper> poolEnemySniper;
    public ObjectPooling<EnemyFire> poolEnemyFire;
    public ObjectPooling<EnemyMarine> poolEnemyMarine;
    public ObjectPooling<EnemyParachutist> poolEnemyParachutist;
    public ObjectPooling<EnemyBazooka> poolEnemyBazooka;
    public ObjectPooling<EnemyMonkey> poolEnemyMonkey;
    public ObjectPooling<EnemySpider> poolEnemySpider;
    public ObjectPooling<BossMonkeyMinion> poolBossMonkeyMinion;

    // UI
    public ObjectPooling<TextDamage> poolTextDamageTMP;
    public ObjectPooling<EffectTextBANG> poolTextBANG;
    public ObjectPooling<EffectTextCRIT> poolTextCRIT;
    public ObjectPooling<EffectTextWHAM> poolTextWHAM;

    // Item Drop
    public ObjectPooling<ItemDropHealth> poolItemDropHealth;
    public ObjectPooling<ItemDropCoin> poolItemDropCoin;
    public ObjectPooling<ItemDropGun> poolItemDropGun;

    // Objects
    public ObjectPooling<PoisonTrap> poolPoisonTrap;
    public ObjectPooling<Spike> poolSpike;


    //void Awake()
    //{
    //    InitPool();
    //}

    public void InitPool()
    {
        // Rambo bullet
        poolBulletUzi = new ObjectPooling<BulletUzi>();
        poolBulletMachineGunM4 = new ObjectPooling<BulletMachineGunM4>();
        poolBulletScarHGun = new ObjectPooling<BulletScarHGun>();
        poolBulletAWP = new ObjectPooling<BulletAWP>();
        poolBulletShotgun = new ObjectPooling<BulletShotgun>();
        poolBulletP100 = new ObjectPooling<BulletP100>();
        poolBulletBullpup = new ObjectPooling<BulletBullpup>();
        poolBulletSniperRifle = new ObjectPooling<BulletSniperRifle>();
        poolBulletTeslaMini = new ObjectPooling<BulletTeslaMini>();

        poolBulletSpreadGun = new ObjectPooling<BulletSpreadGun>();
        poolBulletRocketChaser = new ObjectPooling<BulletRocketChaser>();
        poolBulletFamasGun = new ObjectPooling<BulletFamasGun>();
        poolBulletSplitGun = new ObjectPooling<BulletSplitGun>();
        poolBulletFireBall = new ObjectPooling<FireBall>();
        poolBulletKamePower = new ObjectPooling<BulletKamePower>();

        poolBombSupportSkill = new ObjectPooling<BombSupportSkill>();
        poolBombSupportSurvival = new ObjectPooling<BombSupportSurvival>();

        // Enemy bullet
        poolBulletRifle = new ObjectPooling<BulletRifle>();
        poolBulletPistol = new ObjectPooling<BulletPistol>();
        poolBulletSniper = new ObjectPooling<BulletSniper>();
        poolBulletTank = new ObjectPooling<BulletTank>();
        poolBulletTankCannon = new ObjectPooling<BulletTankCannon>();
        poolBulletBomb = new ObjectPooling<Bomb>();
        poolHomingMissile = new ObjectPooling<HomingMissile>();
        poolBulletPlasma = new ObjectPooling<BulletPlasma>();
        poolTorpedo = new ObjectPooling<Torpedo>();
        poolBulletSpider = new ObjectPooling<BulletSpider>();
        poolBulletBazooka = new ObjectPooling<BulletBazooka>();

        // Boss bullet
        poolBulletBossMegatron = new ObjectPooling<BulletBossMegatron>();
        poolRocketBossMegatank = new ObjectPooling<RocketBossMegatank>();
        poolRocketBossSubmarine = new ObjectPooling<RocketBossSubmarine>();
        poolBulletPoisonBossVenom = new ObjectPooling<BulletPoisonBossVenom>();
        poolBulletBossProfessor = new ObjectPooling<BulletBossProfessor>();
        poolStoneBossMonkey = new ObjectPooling<StoneBossMonkey>();
        poolStoneBossMonkeyMinion = new ObjectPooling<StoneBossMonkeyMinion>();

        // Grenade
        poolBaseGrenade = new ObjectPooling<BaseGrenade>();
        poolBaseGrenadeEnemy = new ObjectPooling<BaseGrenadeEnemy>();

        // Enemy
        poolEnemyRifle = new ObjectPooling<EnemyRifle>();
        poolEnemyGeneral = new ObjectPooling<EnemyGeneral>();
        poolEnemyGrenade = new ObjectPooling<EnemyGrenade>();
        poolEnemyKnife = new ObjectPooling<EnemyKnife>();
        poolEnemyHelicopter = new ObjectPooling<EnemyHelicopter>();
        poolEnemyTank = new ObjectPooling<EnemyTank>();
        poolEnemyTankCannon = new ObjectPooling<EnemyTankCannon>();
        poolEnemyBomber = new ObjectPooling<EnemyBomber>();
        poolEnemySniper = new ObjectPooling<EnemySniper>();
        poolEnemyFire = new ObjectPooling<EnemyFire>();
        poolEnemyMarine = new ObjectPooling<EnemyMarine>();
        poolEnemyParachutist = new ObjectPooling<EnemyParachutist>();
        poolEnemyBazooka = new ObjectPooling<EnemyBazooka>();
        poolEnemyMonkey = new ObjectPooling<EnemyMonkey>();
        poolEnemySpider = new ObjectPooling<EnemySpider>();
        poolBossMonkeyMinion = new ObjectPooling<BossMonkeyMinion>();

        // UI
        poolTextDamageTMP = new ObjectPooling<TextDamage>();
        poolTextBANG = new ObjectPooling<EffectTextBANG>();
        poolTextCRIT = new ObjectPooling<EffectTextCRIT>();
        poolTextWHAM = new ObjectPooling<EffectTextWHAM>();

        // Item Drop
        poolItemDropHealth = new ObjectPooling<ItemDropHealth>();
        poolItemDropCoin = new ObjectPooling<ItemDropCoin>();
        poolItemDropGun = new ObjectPooling<ItemDropGun>();

        // Objects
        poolPoisonTrap = new ObjectPooling<PoisonTrap>();
        poolSpike = new ObjectPooling<Spike>();
    }
}
