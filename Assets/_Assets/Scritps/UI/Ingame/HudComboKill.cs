using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudComboKill : MonoBehaviour
{
    public Image imageCombo;
    public Text textComboKill;
    public Outline textComboKillOutline;
    public AudioClip[] sfxComboKills;
    public AudioClip comboKillMax;

    private float timeOutResetCombo = 2f;
    private IEnumerator coroutineHideCombo;


    public void Init()
    {
        EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, (sender, param) => UpdateComboKills((int)param));
    }

    public void UpdateComboKills(int killCount)
    {
        if (killCount > 0)
        {
            imageCombo.gameObject.SetActive(true);
            textComboKill.text = killCount.ToString();

            AudioClip clip = killCount > sfxComboKills.Length ? comboKillMax : sfxComboKills[killCount - 1];
            float soundVolumeDecibel = killCount % 2 == 1 ? -15f : 0f;
            SoundManager.Instance.PlaySfx(clip, soundVolumeDecibel);

            if (coroutineHideCombo != null)
            {
                StopCoroutine(coroutineHideCombo);
                coroutineHideCombo = null;
            }

            coroutineHideCombo = CoroutineHideCombo();
            StartCoroutine(coroutineHideCombo);
        }
    }

    private IEnumerator CoroutineHideCombo()
    {
        textComboKill.gameObject.SetActive(false);
        textComboKill.gameObject.SetActive(true);
        SetAlphaElements(1f);
        float timeCount = 0f;

        while (timeCount <= timeOutResetCombo)
        {
            timeCount += Time.deltaTime;
            float percent = Mathf.Clamp01((timeOutResetCombo - timeCount) / timeOutResetCombo);
            SetAlphaElements(percent);
            yield return null;
        }

        EventDispatcher.Instance.PostEvent(EventID.TimeOutComboKill);
        coroutineHideCombo = null;
        imageCombo.gameObject.SetActive(false);
    }

    private void SetAlphaElements(float a)
    {
        Color c = imageCombo.color;
        c.a = a;
        imageCombo.color = c;

        c = textComboKill.color;
        c.a = a;
        textComboKill.color = c;

        c = textComboKillOutline.effectColor;
        c.a = a;
        textComboKillOutline.effectColor = c;
    }
}
