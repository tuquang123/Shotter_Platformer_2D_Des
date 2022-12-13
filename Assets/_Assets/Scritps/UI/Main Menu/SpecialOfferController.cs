using UnityEngine;
using System.Collections;
using Spine.Unity;
using System;
using UnityEngine.UI;

public class SpecialOfferController : MonoBehaviour
{
    public GameObject popupSpecialOffer;
    public Text textCountDown;
    public SpecialOfferSpine packGun;
    public SpecialOfferSpine packMoney;
    //[SpineSkin]
    //public string skinPackEverybodyFavorite, skinPackDragonBreath, skinPackLetThereBeFire, skinPackSnippingForDummies,
    //    skinPackTaserLaser, skinPackShockingSale, skinPackEnthusiast;

    public GameObject btnBuyPackEverybodyFavorite;
    public GameObject btnBuyPackDragonBreath;
    public GameObject btnBuyPackLetThereBeFire;
    public GameObject btnBuyPackSnippingForDummies;
    public GameObject btnBuyPackTaserLaser;
    public GameObject btnBuyPackShockingSale;
    public GameObject btnBuyPackEnthusiast;

    public bool isShowPack = true;


    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.BuySpecialOffer, (sender, param) => Hide());
        CheckDay();
    }

    private void OnEnable()
    {
        StartCoroutine(CoroutineTimerEnd());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Show(bool isShow)
    {
        popupSpecialOffer.transform.localScale = isShow ? Vector3.one : Vector3.zero;
        popupSpecialOffer.SetActive(isShow);

        if (packGun.gameObject.activeSelf)
        {
            packGun.Show();
        }
        else
        {
            packMoney.Show();
        }

        if (isShow)
            SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_SHOW_DIALOG);
    }

    public void Hide()
    {
        Show(false);
        gameObject.SetActive(false);
    }

    private void CheckDay()
    {
        DateTime today = DateTime.Now;

        if (today.DayOfWeek == DayOfWeek.Sunday)
        {
            packGun.gameObject.SetActive(false);
            packMoney.gameObject.SetActive(true);

            if (ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast == false)
            {
                packMoney.day = today.DayOfWeek;
                btnBuyPackEnthusiast.SetActive(true);
                isShowPack = true;
                gameObject.SetActive(true);
            }
            else
            {
                isShowPack = false;
                gameObject.SetActive(false);
            }
        }
        else
        {
            isShowPack = false;
            gameObject.SetActive(false);

            //    packGun.gameObject.SetActive(true);
            //    packMoney.gameObject.SetActive(false);

            //    switch (today.DayOfWeek)
            //    {
            //        case DayOfWeek.Monday:
            //            {
            //                if (ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite == false)
            //                {
            //                    btnBuyPackEverybodyFavorite.SetActive(true);
            //                    isShowPack = true;
            //                    gameObject.SetActive(true);
            //                }
            //                else
            //                {
            //                    isShowPack = false;
            //                    gameObject.SetActive(false);
            //                }
            //            }
            //            break;

            //        case DayOfWeek.Tuesday:
            //            {
            //                if (ProfileManager.UserProfile.isPurchasedPackDragonBreath == false)
            //                {
            //                    btnBuyPackDragonBreath.SetActive(true);
            //                    isShowPack = true;
            //                    gameObject.SetActive(true);
            //                }
            //                else
            //                {
            //                    isShowPack = false;
            //                    gameObject.SetActive(false);
            //                }
            //            }
            //            break;

            //        case DayOfWeek.Wednesday:
            //            {
            //                if (ProfileManager.UserProfile.isPurchasedPackLetThereBeFire == false)
            //                {
            //                    btnBuyPackLetThereBeFire.SetActive(true);
            //                    isShowPack = true;
            //                    gameObject.SetActive(true);
            //                }
            //                else
            //                {
            //                    isShowPack = false;
            //                    gameObject.SetActive(false);
            //                }
            //            }
            //            break;

            //        case DayOfWeek.Thursday:
            //            {
            //                if (ProfileManager.UserProfile.isPurchasedPackSnippingForDummies == false)
            //                {
            //                    btnBuyPackSnippingForDummies.SetActive(true);
            //                    isShowPack = true;
            //                    gameObject.SetActive(true);
            //                }
            //                else
            //                {
            //                    isShowPack = false;
            //                    gameObject.SetActive(false);
            //                }
            //            }
            //            break;

            //        case DayOfWeek.Friday:
            //            {
            //                if (ProfileManager.UserProfile.isPurchasedPackTaserLaser == false)
            //                {
            //                    btnBuyPackTaserLaser.SetActive(true);
            //                    isShowPack = true;
            //                    gameObject.SetActive(true);
            //                }
            //                else
            //                {
            //                    isShowPack = false;
            //                    gameObject.SetActive(false);
            //                }
            //            }
            //            break;

            //        case DayOfWeek.Saturday:
            //            {
            //                if (ProfileManager.UserProfile.isPurchasedPackShockingSale == false)
            //                {
            //                    btnBuyPackShockingSale.SetActive(true);
            //                    isShowPack = true;
            //                    gameObject.SetActive(true);
            //                }
            //                else
            //                {
            //                    isShowPack = false;
            //                    gameObject.SetActive(false);
            //                }
            //            }
            //            break;
            //    }

            //    packGun.day = today.DayOfWeek;
        }
    }

    private IEnumerator CoroutineTimerEnd()
    {
        DateTime current = DateTime.Now;
        DateTime endDay = new DateTime(current.Year, current.Month, current.Day, 23, 59, 59);

        double timeLeft = TimeSpan.FromTicks(endDay.Ticks - current.Ticks).TotalSeconds;

        while (timeLeft > 0)
        {
            yield return StaticValue.waitOneSec;

            TimeSpan t = TimeSpan.FromSeconds(timeLeft);

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            MasterInfo.Instance.CountDownTimer(t, out hours, out minutes, out seconds);
            //Debug.Log(string.Format("{0}D {1}H:{2}M:{3}S", days, hours, minutes, seconds));
            textCountDown.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            timeLeft--;
        }

        textCountDown.text = string.Empty;
        Hide();
    }
}
