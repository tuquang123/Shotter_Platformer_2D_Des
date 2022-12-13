using UnityEngine;
using System.Collections;
using TMPro;

public class TextDamage : BaseEffect
{
    public TextMesh textDamage;

    public int  sizeNormalDamage = 3;
    public Color32 colorNormalDamage;

    public int sizeCritDamage = 7;
    public Color32 colorCriticalDamage;

    public int sizeGrenadeDamage = 7;
    public Color32 colorGrenadeDamage;


    private bool isInGame = true;


    public override void Deactive()
    {
        base.Deactive();

        textDamage.text = string.Empty;

        if (isInGame)
        {
            PoolingController.Instance.poolTextDamageTMP.Store(this);
            transform.SetParent(PoolingController.Instance.groupEffect, true);
            //transform.parent = PoolingController.Instance.groupText;
        }
        else
        {
            PoolingPreviewController.Instance.textDamage.Store(this);
            transform.SetParent(PoolingPreviewController.Instance.group, true);
            //transform.parent = PoolingPreviewController.Instance.group;
        }
    }

    public void Active(Vector2 position, AttackData attackData, Transform parent = null, bool isInGame = true)
    {
        this.isInGame = isInGame;
        transform.SetParent(parent, true);
        //transform.parent = parent;
        transform.position = position;

        if (attackData.weapon == WeaponType.Grenade)
        {
           textDamage.color = colorGrenadeDamage;
            textDamage.fontSize = sizeGrenadeDamage;
        }
        else
        {
            if (attackData.isCritical)
            {
                textDamage.color = colorCriticalDamage;
                textDamage.fontSize = sizeCritDamage;
            }
            else
            {
                textDamage.color = colorNormalDamage;
               textDamage.fontSize = sizeNormalDamage;
            }
        }

        float ratio = Random.Range(0.85f, 1.15f);
        int damageDisplay = Mathf.RoundToInt(attackData.damage * 10f * ratio);
        textDamage.text = damageDisplay.ToString();

        gameObject.SetActive(true);
    }

    public void Active(Vector2 position, string content, Color32 color, int fontSize = 3, Transform parent = null, bool isInGame = true)
    {
        this.isInGame = isInGame;

        transform.SetParent(parent, true);
        //transform.parent = parent;
        transform.position = position;

       textDamage.fontSize = fontSize;
        textDamage.text = content;
        textDamage.color = color;

        gameObject.SetActive(true);
    }
}
