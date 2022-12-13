using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class DummyPreview : MonoBehaviour
{
    public BaseEffect textDamageTMP;
    public Color32 colorText;

    private SpriteRenderer sprite;
    private List<string> showTexts = new List<string> { "Ratata", "Oh nooo!", "Oops!!" };
    private bool isFlashing;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        EventDispatcher.Instance.RegisterListener(EventID.PreviewDummyTakeDamage, (sender, param) => TakeDamge());
        //EventDispatcher.Instance.RegisterListener(EventID.PreviewRocketChaserReady, (sender, param) => FocusOnThis((BulletPreviewRocketChaser)param));
    }

    public void TakeDamge()
    {
        EffectTakeDamage();

        Vector2 v = transform.position;
        v.x += Random.Range(-0.5f, 0.5f);
        v.y += Random.Range(0, 0.5f);

        TextDamage text = PoolingPreviewController.Instance.textDamage.New();

        if (text == null)
        {
            text = Instantiate(textDamageTMP) as TextDamage;
        }

        string str = showTexts[Random.Range(0, showTexts.Count)];
        text.Active(v, str, colorText, parent: PoolingPreviewController.Instance.group, isInGame: false);
    }

    protected void EffectTakeDamage()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            DOTween.To(ColorSetter, 1f, 0f, 0.1f).OnComplete(ChangeColorToDefault);
        }
    }

    private void ColorSetter(float pNewValue)
    {
        Color c = sprite.color;
        c.g = pNewValue;
        c.b = pNewValue;

        sprite.color = c;
    }

    private void ChangeColorToDefault()
    {
        sprite.color = Color.white;
        isFlashing = false;
    }

    private void FocusOnThis(BulletPreviewRocketChaser rocket)
    {
        rocket.Focus(transform);
    }
}
