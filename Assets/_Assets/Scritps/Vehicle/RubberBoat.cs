using CnControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RubberBoat : Vehicle
{
    [Header("RUBBER BOAT")]
    public Animation anim;
    public ParticleSystem moveSplash;
    public ParticleSystem dropSplash;
    public ParticleSystem jumpSplash;
    public string moveAnimName;
    public string jumpAnimName;
    public string idleAnimName;
    public string knockBackAnimName;
    public bool autoMove;
    public bool canAttack;
    public Transform spawnHeroPosition;
    public Transform playerStopPoint;
    public AudioClip soundMove, soundJump;

    private float moveForce = 280f;
    private bool breakMove;
    private bool isJumping;
    private bool isKnocking;
    private float timerKnocking = 1f;
    private float obstacleCrashDamage = 30f;
    private float bossCrashDamage = 50f;
    private Rambo player;

    private bool isEmergencyStop;

    public override BaseUnit Player { get { return player; } }
    public override float HpPercent { get { return player.HpPercent; } }
    public override bool IsFacingRight { get { return player.IsFacingRight; } }


    #region UNITY METHODS

    void Start()
    {
        if (GameData.currentStage.difficulty == Difficulty.Hard)
        {
            //moveForce *= 1.15f;
            obstacleCrashDamage = 50f;
            bossCrashDamage = 80f;
        }
        else if (GameData.currentStage.difficulty == Difficulty.Crazy)
        {
            //moveForce *= 1.2f;
            obstacleCrashDamage = 70f;
            bossCrashDamage = 100f;
        }

        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonJump, (sender, param) => Jump());
        EventDispatcher.Instance.RegisterListener(EventID.PlayerDie, EmergencyStop);
        EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, (sender, param) => Renew());
        EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, (sender, param) => Renew());

        if (autoMove)
        {
            moveSplash.Play();
            anim.Play(moveAnimName);
        }
        else
        {
            Idle();
        }

        isOnVehicle = true;
    }

    void Update()
    {
        if (breakMove)
        {
            transform.position = Vector3.Lerp(transform.position, playerStopPoint.position, Time.deltaTime);

            if (Vector3.Distance(transform.position, playerStopPoint.position) <= 0.1f)
            {
                transform.position = playerStopPoint.position;
                rigid.bodyType = RigidbodyType2D.Static;

                Idle();
                GetOut();
                DebugCustom.Log("Disable BOAT");
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
#endif
    }

    void FixedUpdate()
    {
        if (!breakMove)
        {
            Move();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(StaticValue.TAG_GETOUT_VEHICLE))
        {
            if (!breakMove)
            {
                breakMove = true;
                rigid.velocity = Vector2.zero;
                rigid.angularVelocity = 0f;
                rigid.isKinematic = true;

                EventDispatcher.Instance.PostEvent(EventID.BoatStop);
                //EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
            }
        }
        else if (other.CompareTag(StaticValue.TAG_ENEMY))
        {
            AddForce(transform.right, 2.5f);
            TakeDamage(bossCrashDamage);
        }
        else if (other.CompareTag(StaticValue.TAG_DESTRUCTIBLE_OBSTACLE))
        {

            TakeDamage(obstacleCrashDamage);
            other.gameObject.SetActive(false);

            if (!isEmergencyStop)
            {
                CameraFollow.Instance.AddShake(0.1f, 1f);
                EventDispatcher.Instance.PostEvent(EventID.BoatTriggerObstacle);
            }
        }
    }

    #endregion


    #region PUBLIC METHODS

    public void StartMove()
    {
        autoMove = true;
        moveSplash.Play();
        anim.Play(moveAnimName);
    }

    public void BoatTriggerWater()
    {
        dropSplash.Play();
        moveSplash.Play();
        isJumping = false;
        EventDispatcher.Instance.PostEvent(EventID.BoatTriggerWater, transform.position);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_TRIGGER_WATER, -15f);
    }

    private void ActiveSoundMove(bool isActive)
    {
        if (isActive)
        {
            if (audioSource.clip == null)
            {
                audioSource.loop = true;
                audioSource.clip = soundMove;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    #endregion


    #region VEHICLE IMPLEMENTION

    public override void TakeDamage(AttackData attackData)
    {
        if (!isImmortal && player)
            player.TakeDamage(attackData);
    }

    public override void TakeDamage(float damage)
    {
        if (!isImmortal && player)
            player.TakeDamage(damage);
    }

    protected override void Move()
    {
        if (isKnocking)
        {
            timerKnocking = Mathf.MoveTowards(timerKnocking, 0f, Time.deltaTime);
            rigid.velocity = Vector3.Lerp(rigid.velocity, Vector2.right * moveForce * Time.deltaTime, Time.deltaTime);
            isKnocking = timerKnocking != 0f;
        }
        else
        {
            if (autoMove)
            {
                if (isEmergencyStop)
                {
                    rigid.velocity = Vector3.Lerp(rigid.velocity, Vector2.zero, 5f * Time.deltaTime);
                    player.lastDiePosition = spawnHeroPosition.position;
                }
                else
                {
                    // TODO: Move slow 2.6
                    rigid.velocity = Vector2.right * 180f * Time.deltaTime;
                }
            }
            else
            {
                if (isEmergencyStop)
                {
                    player.lastDiePosition = spawnHeroPosition.position;
                }
                else
                {
                    float input = CnInputManager.GetAxis(StaticValue.JOYSTICK_HORIZONTAL);

                    if (input < -0.2f || input > 0.2f)
                    {
                        rigid.velocity = Vector2.right * input * moveForce * Time.deltaTime;
                    }
                }
            }
        }
    }

    protected override void Jump()
    {
        if (breakMove)
            return;

        moveSplash.Stop();
        moveSplash.Clear();

        dropSplash.Stop();
        dropSplash.Clear();

        if (!isJumping)
        {
            jumpSplash.Play();
        }

        anim.Play(jumpAnimName, PlayMode.StopAll);
        anim.PlayQueued(moveAnimName);

        isJumping = true;

        SoundManager.Instance.PlaySfx(soundJump, -15f);
    }

    public override void Idle()
    {
        anim.CrossFade(idleAnimName, 0.06f);

        moveSplash.Stop();
        dropSplash.Stop();
        jumpSplash.Stop();
    }

    public override void GetIn(Rambo rambo)
    {
        tag = StaticValue.TAG_PLAYER;
        rambo.SetLookDir(true);
        rambo.Rigid.isKinematic = true;
        rambo.Rigid.simulated = false;
        rambo.transform.parent = spawnHeroPosition;
        rambo.transform.localPosition = Vector3.zero;
        rambo.transform.localEulerAngles = Vector3.zero;
        rambo.enableMoving = false;
        rambo.enableJumping = false;
        rambo.enableFlipX = false;
        rambo.isOnVehicle = true;

        player = rambo;

        ActiveSoundMove(true);

        enabled = true;
    }

    public override void GetOut()
    {
        enabled = false;

        tag = StaticValue.TAG_NONE;

        if (player)
        {
            player.Rigid.isKinematic = false;
            player.Rigid.simulated = true;
            player.transform.parent = null;
            player.enableMoving = true;
            player.enableJumping = true;
            player.enableFlipX = true;
            player.isOnVehicle = false;
            player.enabled = true;
            player.transform.eulerAngles = Vector3.zero;

            player = null;
        }

        ActiveSoundMove(false);
    }

    public override void AddForce(Vector3 dir, float force, ForceMode2D forceMode = ForceMode2D.Impulse)
    {
        if (!isEmergencyStop)
        {
            base.AddForce(dir, force, forceMode);

            isKnocking = true;
            timerKnocking = 1f;
            anim.Play(knockBackAnimName);
            anim.PlayQueued(moveAnimName);

            dropSplash.Play();
            moveSplash.Play();
        }
    }

    public override void Renew()
    {
        StartCoroutine(DelayRenew());
    }

    #endregion


    private void EmergencyStop(Component arg1, object arg2)
    {
        isImmortal = true;
        isEmergencyStop = true;
    }

    private IEnumerator DelayRenew()
    {
        yield return StaticValue.waitOneSec;

        isEmergencyStop = false;

        yield return StaticValue.waitTwoSec;

        isImmortal = false;
    }
}
