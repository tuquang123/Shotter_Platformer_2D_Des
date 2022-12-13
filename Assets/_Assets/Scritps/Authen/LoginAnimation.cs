using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class LoginAnimation : MonoBehaviour
{
    public Mask mask;
    public LoginLightEffect lightEffect;
    public GameObject touchToPlay;

    public void ActiveLightEffect()
    {
        mask.enabled = true;
        lightEffect.gameObject.SetActive(true);
        touchToPlay.SetActive(true);

        Camera.main.DOShakePosition(0.5f, 0.5f);

        SoundManager.Instance.PlayMusic(StaticValue.SOUND_MUSIC_MENU);
    }
}
