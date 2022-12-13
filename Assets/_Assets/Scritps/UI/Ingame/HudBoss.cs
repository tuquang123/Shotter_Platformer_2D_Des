using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudBoss : MonoBehaviour
{
    public WarningBoss warning;
    public Image hpBoss;
    public Image iconBoss;
    public GameObject infoBossMegatron;
    public Sprite[] icons;

    public void Init()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ShowInfoBossMegatron, (sender, param) => { infoBossMegatron.SetActive(true); });
    }

    public void HideUI()
    {
        hpBoss.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateHP(float percent)
    {
        hpBoss.fillAmount = percent;
        hpBoss.transform.parent.gameObject.SetActive(true);
    }

    public void SetIconBoss(int bossId)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (int.Parse(icons[i].name) == bossId)
            {
                iconBoss.sprite = icons[i];
                iconBoss.SetNativeSize();
                return;
            }
        }
    }

    public void WarningBoss()
    {
        warning.Active();
    }

    public void StartFightingBoss()
    {
        infoBossMegatron.SetActive(false);
        EventDispatcher.Instance.PostEvent(EventID.ShowInfoBossDone);
    }
}
