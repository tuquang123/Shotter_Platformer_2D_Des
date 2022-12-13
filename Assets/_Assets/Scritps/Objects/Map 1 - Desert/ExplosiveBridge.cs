using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ExplosiveBridge : MonoBehaviour
{
    public float delayExplode = 0.5f;
    public Animator[] c4;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            DOTween.To(ColorSetter, 1f, 0f, 0.5f);
            this.StartDelayAction(Explode, delayExplode);

            for (int i = 0; i < c4.Length; i++)
            {
                c4[i].Play("C4");
            }
        }
    }

    private void Explode()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, transform.position);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
        gameObject.SetActive(false);
    }

    private void ColorSetter(float pNewValue)
    {
        Color c = sprite.color;
        c.g = pNewValue;
        c.b = pNewValue;
        sprite.color = c;
    }
}
