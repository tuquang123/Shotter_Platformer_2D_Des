using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseUnit : MonoBehaviour
{
    [Header("STATS")]
    public SO_BaseUnitStats baseStats;
    public UnitStats stats = new UnitStats();

    [Header("BASE UNIT PROPERTIES")]
    public int id;
    public int level;
    public bool isDead;
    public bool isImmortal;
    public bool isStun;
    public bool isKnockBack;
    public bool isOnVehicle;
    public SpriteRenderer healthBar;
    public Transform bodyCenterPoint;
    public Transform aimPoint;
    public Transform showDamagePoint;
    public AudioClip[] soundDie;
    [SerializeField]
    public List<ItemDropData> itemDropList;

    protected float healthBarSizeX;
    protected Rigidbody2D rigid;
    protected AudioSource audioSource;

    public virtual float HpPercent { get { return Mathf.Clamp01(stats.HP / stats.MaxHp); } }
    public virtual bool IsMoving { get; set; }
    public Rigidbody2D Rigid { get { return rigid; } }
    public bool IsDisableAction { get { return isStun || isKnockBack; } }
    public virtual bool IsFacingRight { get { return false; } }
    public Transform BodyCenterPoint
    {
        get
        {
            if (bodyCenterPoint != null)
            {
                return bodyCenterPoint;
            }
            else
            {
                return transform;
            }
        }
    }


    #region PROTECTED METHODS

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (healthBar)
        {
            healthBarSizeX = healthBar.size.x;
        }
    }

    protected virtual void EffectTakeDamage() { }

    protected virtual void LoadScriptableObject() { }

    protected virtual void StopMoving()
    {
        if (rigid != null)
        {
            Vector3 tmpVector = rigid.velocity;
            tmpVector.x = 0;
            rigid.velocity = tmpVector;
            rigid.angularVelocity = 0;
        }
    }

    protected virtual void Idle() { }

    protected virtual void Move() { }

    protected virtual void UpdateDirection() { }

    protected virtual void Jump() { }

    protected virtual void Attack() { }

    protected virtual void Die()
    {
        isDead = true;

        PlaySoundDie();
    }

    protected virtual void ApplyDebuffs(AttackData attackData) { }

    protected virtual void PlaySound(AudioClip clip)
    {
        if (audioSource)
        {
            if (clip)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                DebugCustom.LogError("Clip NULL");
            }
        }
        else
        {
            DebugCustom.LogError("Audio source NULL");
        }
    }

    protected virtual void PlaySoundDie()
    {
        if (soundDie.Length > 0)
        {
            int index = Random.Range(0, soundDie.Length);

            if (audioSource)
            {
                audioSource.PlayOneShot(soundDie[index]);
            }
            else
            {
                SoundManager.Instance.PlaySfx(soundDie[index]);
            }
        }
    }

    protected virtual void ShowTextDamageTaken(AttackData attackData)
    {
        if (showDamagePoint != null)
        {
            Vector2 v = showDamagePoint.position;
            v.x += Random.Range(-0.5f, 0.5f);
            v.y += Random.Range(0, 0.5f);

            EffectController.Instance.SpawnTextDamageTMP(v, attackData, PoolingController.Instance.groupText);
        }
    }

    protected virtual void ShowTextDamageTaken(float damage)
    {
        if (showDamagePoint != null)
        {
            Vector2 v = showDamagePoint.position;
            v.x += Random.Range(-0.25f, 0.25f);
            v.y += Random.Range(0, 0.5f);

            int damageDisplay = Mathf.RoundToInt(damage * 10f);
            EffectController.Instance.SpawnTextTMP(v, Color.red, damageDisplay.ToString(), parent: PoolingController.Instance.groupText);
        }
    }

    protected IEnumerator DelayAction(UnityAction callback, WaitForSeconds delayTime)
    {
        yield return delayTime;

        if (callback != null)
        {
            callback();
        }
    }

    protected virtual void HandleAnimationStart(TrackEntry entry) { }

    protected virtual void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e) { }

    protected virtual void HandleAnimationCompleted(TrackEntry entry) { }

    protected virtual void AvoidSlideOnInclinedPlane() { }

    #endregion


    #region PUBLIC METHODS

    //public virtual void AddModifier(BaseModifier modifier) { }

    //public virtual void RemoveModifier(StatsType stats, float value) { }

    public virtual void AddModifier(ModifierData data) { }

    public virtual void RemoveModifier(ModifierData data) { }

    public virtual void ApplyModifier() { }

    public virtual void ReloadStats() { }

    public virtual void TakeDamage(AttackData attackData) { }

    public virtual void TakeDamage(float damage) { }

    public virtual void Renew()
    {
        isDead = false;

        isStun = false;
        isKnockBack = false;

        LoadScriptableObject();
        stats.Init(baseStats);

        //finalMaxHP = baseStats.HP + CalculateBaseStatsIncrease(StatsType.Hp);
        //SetStats(StatsType.Hp, finalMaxHP);
    }

    //public virtual void SetStats(StatsType type, float value)
    //{
    //    stats.SetStats(value, type);
    //}

    //public virtual float CalculateBaseStatsIncrease(StatsType type)
    //{
    //    return 0;
    //}

    public virtual void AddForce(Vector3 dir, float force, ForceMode2D forceMode = ForceMode2D.Impulse)
    {
        rigid.AddForce(dir * force, forceMode);
    }

    public virtual void Active(int id, int level, Vector2 position)
    {
        this.id = id;
        this.level = level;
        transform.position = position;

        Renew();
        enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    public virtual void UpdateHealthBar(bool isAutoHide) { }

    public virtual void ActiveHealthBar(bool isActive)
    {
        if (healthBar != null)
        {
            healthBar.transform.parent.gameObject.SetActive(isActive);
        }
    }

    public virtual void SetTarget(BaseUnit unit) { }

    public virtual AttackData GetCurentAttackData()
    {
        AttackData attackData = new AttackData(this, stats.Damage);

        return attackData;
    }

    public virtual bool IsOutOfScreen()
    {
        bool isOutOfScreenX = transform.position.x < CameraFollow.Instance.left.position.x - 0.5f || transform.position.x > CameraFollow.Instance.right.position.x + 0.5;
        bool isOutOfScreenY = transform.position.y < CameraFollow.Instance.bottom.position.y - 0.5f || transform.position.y > CameraFollow.Instance.top.position.y + 0.5f;
        bool isOutOfScreen = (isOutOfScreenX || isOutOfScreenY);

        return isOutOfScreen;
    }

    public virtual void GetStun(float duration) { }

    public virtual void FallBackward(float duration) { }

    #endregion

}
