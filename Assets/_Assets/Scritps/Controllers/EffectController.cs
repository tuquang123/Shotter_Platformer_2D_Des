using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance { get; private set; }

    public TextDamage textDamageTMP;
    public EffectTextCRIT textCRIT;

    public ParticleSystem fxExplosiveGas;
    public ParticleSystem fxExplosiveBomb;
    public ParticleSystem fxExplosiveC4;
    public ParticleSystem fxBulletImpact;
    public ParticleSystem fxBulletImpactExplodeSmall;
    public ParticleSystem fxBulletImpactExplodeMedium;
    public ParticleSystem fxBulletImpactExplodeLarge;
    public ParticleSystem fxBulletImpactSplitGun;
    public ParticleSystem fxBulletImpactTeslaMini;
    public ParticleSystem fxWoodBoxBroken;
    public ParticleSystem fxExplosiveMultiple;
    public ParticleSystem fxStoneRainExplosion;
    public ParticleSystem fxStoneBrokenSmall;
    public ParticleSystem fxStoneBrokenMedium;
    public ParticleSystem fxGroundSmoke;

    private ParticleSystem.EmitParams bulletHitParam;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bulletHitParam.velocity = Vector3.zero;
        bulletHitParam.startSize = 0.1f;
        bulletHitParam.startLifetime = 0.2f;
        bulletHitParam.startColor = Color.white;
    }

    public void SpawnParticleEffect(EffectObjectName effectName, Vector3 position)
    {
        switch (effectName)
        {
            case EffectObjectName.ExplosionBomb:
                fxExplosiveBomb.transform.position = position;
                fxExplosiveBomb.Play();
                break;
            case EffectObjectName.ExplosionGas:
                fxExplosiveGas.transform.position = position;
                fxExplosiveGas.Play();
                break;
            case EffectObjectName.BulletImpactNormal:
                fxBulletImpact.transform.position = position;
                fxBulletImpact.Play();
                break;
            case EffectObjectName.BulletImpactExplodeSmall:
                fxBulletImpactExplodeSmall.transform.position = position;
                fxBulletImpactExplodeSmall.Play();
                break;
            case EffectObjectName.BulletImpactExplodeMedium:
                fxBulletImpactExplodeMedium.transform.position = position;
                fxBulletImpactExplodeMedium.Play();
                break;
            case EffectObjectName.BulletImpactExplodeLarge:
                fxBulletImpactExplodeLarge.transform.position = position;
                fxBulletImpactExplodeLarge.Play();
                break;
            case EffectObjectName.BulletImpactSplitGun:
                fxBulletImpactSplitGun.transform.position = position;
                fxBulletImpactSplitGun.Play();
                break;
            case EffectObjectName.BulletImpactTeslaMini:
                fxBulletImpactTeslaMini.transform.position = position;
                fxBulletImpactTeslaMini.Play();
                break;
            case EffectObjectName.WoodBoxBroken:
                fxWoodBoxBroken.transform.position = position;
                fxWoodBoxBroken.Play(); ;
                break;
            case EffectObjectName.ExplosionC4:
                fxExplosiveC4.transform.position = position;
                fxExplosiveC4.Play(); ;
                break;
            case EffectObjectName.ExplosionMultiple:
                fxExplosiveMultiple.transform.position = position;
                fxExplosiveMultiple.Play();
                break;
            case EffectObjectName.StoneRainExplosion:
                fxStoneRainExplosion.transform.position = position;
                fxStoneRainExplosion.Play();
                break;
            case EffectObjectName.StoneBrokenSmall:
                fxStoneBrokenSmall.transform.position = position;
                fxStoneBrokenSmall.Play();
                break;
            case EffectObjectName.StoneBrokenMedium:
                fxStoneBrokenMedium.transform.position = position;
                fxStoneBrokenMedium.Play();
                break;
            case EffectObjectName.GroundSmoke:
                fxGroundSmoke.transform.position = position;
                fxGroundSmoke.Play();
                break;
        }
    }

    public void SpawnTextTMP(Vector2 position, Color color, string content, int fontSize = 3, Transform parent = null)
    {
        TextDamage text = PoolingController.Instance.poolTextDamageTMP.New();

        if (text == null)
        {
            text = Instantiate(textDamageTMP) as TextDamage;
        }

        text.Active(position, content, color, fontSize, parent);
    }

    public void SpawnTextDamageTMP(Vector2 position, AttackData attackData, Transform parent = null)
    {
        TextDamage text = PoolingController.Instance.poolTextDamageTMP.New();

        if (text == null)
        {
            text = Instantiate(textDamageTMP) as TextDamage;
        }

        text.Active(position, attackData, parent);
    }


    public void SpawnTextCRIT(Vector2 position, Transform parent = null)
    {
        EffectTextCRIT effect = PoolingController.Instance.poolTextCRIT.New();

        if (effect == null)
        {
            effect = Instantiate(textCRIT) as EffectTextCRIT;
        }

        effect.Active(position, parent);

        CameraFollow.Instance.AddShake(0.1f, 0.1f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_CRITICAL_HIT);
    }
}
