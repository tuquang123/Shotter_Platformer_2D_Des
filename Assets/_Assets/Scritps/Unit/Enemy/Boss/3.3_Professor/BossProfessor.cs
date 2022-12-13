using UnityEngine;
using System.Collections;
using Spine.Unity;
using DG.Tweening;
using Spine;
using System.Collections.Generic;

public class BossProfessor : BaseEnemy
{
    [Header("BOSS PROFESSOR PROPERTIES")]
    public BaseBullet bulletSatellitePrefab;
    [SpineAnimation]
    public string pulse, extendSub, narrowSub, idleToImmortal, immortal, laugh;
    public Transform groupSatellite;
    public AudioClip soundShoot, soundPulse, soundAppear, soundLaugh;
    public GameObject effectImmortal;
    public BossProfessorEnergyPulse energyPulse;
    public BossProfessorSatellite[] satellites;
    public BaseEnemy[] minionPrefabs;

    [SerializeField]
    private bool flagShoot;
    [SerializeField]
    private bool flagPulse;
    [SerializeField]
    private bool flagSpawn;
    private bool flagMovingEntrance = true;
    private bool flagRotate;
    private bool isRotateRight;
    private bool isAtBase;
    private float timerShoot;
    private int numberSatelliteActive;
    private float mostLeftPointX;
    private float mostRightPointX;
    private Vector2 pointSpawnLeft;
    private Vector2 pointSpawnRight;
    private WaitForSeconds delayImmortalSpawn = new WaitForSeconds(3f);
    private Dictionary<GameObject, BaseUnit> minions = new Dictionary<GameObject, BaseUnit>();


    protected override void Start()
    {
        base.Start();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, OnUnitDie);
        EventDispatcher.Instance.RegisterListener(EventID.BossProfessorSatelliteDie, (sender, param) => OnSatelliteDie());

        numberSatelliteActive = satellites.Length;
        mostLeftPointX = basePosition.x - 5.4f;
        mostRightPointX = basePosition.x + 5.4f;
    }

    protected override void Update()
    {
        if (isDead == false)
        {
            Entrance();
            RotateSatellite();

            if (isReadyAttack)
                Attack();
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_BOSS_PROFESSOR, level);
        baseStats = Resources.Load<SO_BossProfessorStats>(path);
    }

    protected override void Attack()
    {
        if (flagShoot)
        {
            if (timerShoot < ((SO_BossProfessorStats)baseStats).ShootDuration)
            {
                timerShoot += Time.deltaTime;

                float currentTime = Time.time;
                if (currentTime - lastTimeAttack > stats.AttackRate)
                {
                    lastTimeAttack = currentTime;
                    Shoot();
                }
            }
            else
            {
                flagShoot = false;
                DeactiveRotate();
                skeletonAnimation.AnimationState.SetAnimation(2, narrowSub, false);

                if (isAtBase && numberSatelliteActive > 0)
                {
                    Vector2 v = transform.position;
                    float randomX = Random.Range(3f, 5.4f);
                    v.x = Random.Range(0, 2) == 0 ? v.x + randomX : v.x - randomX;
                    v.y += Random.Range(0.5f, 1f);

                    float s = Vector2.Distance(transform.position, v);

                    transform.DOMove(v, s / baseStats.MoveSpeed).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        isAtBase = false;
                        ActiveShoot(false);

                    });
                }
                else
                {
                    Vector2 v = transform.position;
                    v.x = Mathf.Clamp(target.transform.position.x, mostLeftPointX, mostRightPointX);

                    float s = Vector2.Distance(transform.position, v);

                    transform.DOMove(v, s / baseStats.MoveSpeed).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        ActivePulse();
                    });
                }
            }
        }
        else if (flagPulse)
        {
            flagPulse = false;
            skeletonAnimation.AnimationState.SetAnimation(1, pulse, false);
        }
        else if (flagSpawn)
        {
            flagSpawn = false;
            StartCoroutine(CoroutineSpawnMinions());
        }
    }

    protected override void StartDie()
    {
        base.StartDie();

        DeactiveAllSatellites();
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        base.HandleAnimationCompleted(entry);

        if (isDead)
            return;

        if (string.Compare(entry.animation.name, idleToImmortal) == 0)
        {
            skeletonAnimation.AnimationState.SetAnimation(3, immortal, false);
        }

        if (string.Compare(entry.animation.name, pulse) == 0)
        {
            energyPulse.Active(true);
            ActiveSoundPulse(true);

            Vector2 tmp = transform.position;
            Vector2 v = transform.position;

            v.x = transform.position.x <= basePosition.x ?
                Mathf.Clamp(v.x + 6f, mostLeftPointX, mostRightPointX) : Mathf.Clamp(v.x - 6f, mostLeftPointX, mostRightPointX);

            float s = Vector2.Distance(transform.position, v);

            transform.DOMove(v, s / baseStats.MoveSpeed).SetDelay(1f).SetEase(Ease.Linear).OnComplete(() =>
              {
                  s = Vector2.Distance(transform.position, tmp);

                  transform.DOMove(tmp, s / baseStats.MoveSpeed).SetDelay(1f).SetEase(Ease.Linear).OnComplete(() =>
                  {
                      energyPulse.Active(false);
                      ActiveSoundPulse(false);
                      PlayAnimationIdle();

                      s = Vector2.Distance(transform.position, basePosition);

                      transform.DOMove(basePosition, s / baseStats.MoveSpeed).SetDelay(1f).SetEase(Ease.Linear).OnComplete(() =>
                      {
                          isAtBase = true;

                          if (numberSatelliteActive > 0)
                          {
                              ActiveShoot(true);
                          }
                          else
                          {
                              ActiveSpawn();
                          }
                      });
                  });
              });
        }
    }

    public override void Renew()
    {
        isDead = false;

        LoadScriptableObject();
        stats.Init(baseStats);

        isFinalBoss = true;
        //ActiveEnergyBall(false);
        bodyCollider.enabled = false;
        DeactiveRotate();
        isEffectMeleeWeapon = false;
        isReadyAttack = false;
        target = null;
        transform.parent = null;
        UpdateHealthBar();
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        UIController.Instance.hudBoss.SetIconBoss(id);
        UIController.Instance.hudBoss.UpdateHP(HpPercent);
    }

    public void SetPointSpawnMinion(Vector2 leftPoint, Vector2 rightPoint)
    {
        pointSpawnLeft = leftPoint;
        pointSpawnRight = rightPoint;
    }

    private void RotateSatellite()
    {
        if (flagRotate)
        {
            float rotateSpeed = ((SO_BossProfessorStats)baseStats).RotateSpeed;

            if (isRotateRight)
            {
                groupSatellite.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
            else
            {
                groupSatellite.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            }
        }
    }

    private void Entrance()
    {
        if (flagMovingEntrance)
        {
            flagMovingEntrance = false;

            transform.DOMove(basePosition, 3f).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Init();
                isAtBase = true;
                ActiveImmortal(false);
                ActiveShoot(true);

            }).OnStart(() =>
            {
                PlaySound(soundAppear);
                CameraFollow.Instance.AddShake(0.5f, 3f);
                PlayAnimationIdle();
            });
        }
    }

    private void Init()
    {
        for (int i = 0; i < satellites.Length; i++)
        {
            satellites[i].Init();
        }
    }

    private void Shoot()
    {
        PlaySound(soundShoot);

        for (int i = 0; i < satellites.Length; i++)
        {
            satellites[i].Shoot();
        }
    }

    private void ActiveImmortal(bool isActive)
    {
        if (!isActive)
        {
            skeletonAnimation.AnimationState.SetEmptyAnimation(3, 0f);
        }
        else
        {
            skeletonAnimation.AnimationState.SetAnimation(3, idleToImmortal, false);
        }

        effectImmortal.SetActive(isActive);
        isImmortal = isActive;
        bodyCollider.enabled = !isActive;
    }

    private void ActiveRotate(bool isRotateRight)
    {
        flagRotate = true;
        this.isRotateRight = isRotateRight;
    }

    private void DeactiveRotate()
    {
        flagRotate = false;
    }

    private void DeactiveAllSatellites()
    {
        for (int i = 0; i < satellites.Length; i++)
        {
            if (satellites[i].isDead == false)
                satellites[i].Deactive();
        }
    }

    private void ActiveSoundPulse(bool isActive)
    {
        if (isActive)
        {
            if (audioSource.clip == null)
            {
                audioSource.loop = true;
                audioSource.clip = soundPulse;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    private void ActiveShoot(bool isRotateRight)
    {
        isReadyAttack = false;
        timerShoot = 0;
        PlayAnimationIdle();
        skeletonAnimation.AnimationState.SetAnimation(2, extendSub, false);
        ActiveRotate(isRotateRight);

        StartCoroutine(DelayAction(() =>
        {
            //ActiveEnergyBall(true);
            isReadyAttack = true;
            flagShoot = true;
            flagPulse = false;
            flagSpawn = false;
        },

        StaticValue.waitOneSec));
    }

    private void ActivePulse()
    {
        isReadyAttack = false;
        timerShoot = 0;
        PlayAnimationIdle();

        StartCoroutine(DelayAction(() =>
        {
            //ActiveEnergyBall(true);
            isReadyAttack = true;
            flagShoot = false;
            flagPulse = true;
            flagSpawn = false;
        },

        StaticValue.waitOneSec));
    }

    private void ActiveSpawn()
    {
        isReadyAttack = false;
        timerShoot = 0;
        PlayAnimationIdle();
        PlaySound(soundLaugh);
        skeletonAnimation.AnimationState.SetAnimation(1, laugh, true);

        StartCoroutine(DelayAction(() =>
        {
            ActiveImmortal(true);
            isReadyAttack = true;
            flagShoot = false;
            flagPulse = false;
            flagSpawn = true;
        },

        delayImmortalSpawn));
    }

    private void OnSatelliteDie()
    {
        numberSatelliteActive--;

        if (numberSatelliteActive <= 0)
        {
            ActiveImmortal(false);
        }
        else if (numberSatelliteActive <= 4)
        {
            ActiveImmortal(true);
        }
    }


    #region MINIONS

    private IEnumerator CoroutineSpawnMinions()
    {
        int enemySpawned = 0;

        while (enemySpawned < 2)
        {
            //// Spawn from left
            //if (CameraFollow.Instance.IsCanSpawnGroundEnemyFromLeft())
            //{
            //    SpawnMinionsFromSide(CameraFollow.Instance.pointGroundSpawnLeft);
            //}
            SpawnMinionsFromSide(pointSpawnLeft);

            //// Spawn from right
            //if (CameraFollow.Instance.IsCanSpawnGroundEnemyFromRight())
            //{
            //    SpawnMinionsFromSide(CameraFollow.Instance.pointGroundSpawnRight);
            //}
            SpawnMinionsFromSide(pointSpawnRight);

            enemySpawned++;
            yield return StaticValue.waitOneSec;
        }
    }

    private void SpawnMinionsFromSide(Vector2 position)
    {
        int randomEnemyId = Random.Range(0, minionPrefabs.Length);
        int id = minionPrefabs[randomEnemyId].id;

        int minLevel = ((SO_BossProfessorStats)baseStats).EnemyMinLevel;
        int maxLevel = ((SO_BossProfessorStats)baseStats).EnemyMaxLevel;
        int level = Random.Range(minLevel, maxLevel + 1);

        //if (GameData.mode == GameMode.Campaign)
        //{
        //    if (GameData.currentStage.difficulty == Difficulty.Hard)
        //    {
        //        level += StaticValue.LEVEL_INCREASE_MODE_HARD;
        //    }
        //    else if (GameData.currentStage.difficulty == Difficulty.Crazy)
        //    {
        //        level += StaticValue.LEVEL_INCREASE_MODE_CRAZY;
        //    }

        //    level = Mathf.Clamp(level, 1, StaticValue.MAX_LEVEL_ENEMY);
        //}

        BaseEnemy enemyPrefab = GameController.Instance.GetEnemyPrefab(id);
        BaseEnemy enemy = enemyPrefab.GetFromPool();
        enemy.Active(id, level, position);
        enemy.zoneId = -1;
        enemy.isRunPassArea = true;
        enemy.DelayTargetPlayer();
        GameController.Instance.AddUnit(enemy.gameObject, enemy);
        minions.Add(enemy.gameObject, enemy);
    }

    private void OnUnitDie(Component senser, object param)
    {
        UnitDieData data = (UnitDieData)param;
        BaseEnemy enemy = data.unit.GetComponent<BaseEnemy>();

        if (minions.ContainsKey(enemy.gameObject))
        {
            minions.Remove(enemy.gameObject);
            if (minions.Count <= 0)
            {
                if (numberSatelliteActive > 0)
                {
                    ActiveShoot(true);
                }
                else
                {
                    ActiveImmortal(false);

                    Vector2 v = transform.position;
                    v.x = Mathf.Clamp(target.transform.position.x, mostLeftPointX, mostRightPointX);

                    float s = Vector2.Distance(transform.position, v);

                    transform.DOMove(v, s / baseStats.MoveSpeed).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(() =>
                      {
                          ActivePulse();
                      });
                }
            }
        }
    }
    #endregion
}
