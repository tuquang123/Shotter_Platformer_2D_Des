using UnityEngine;
using System.Collections;
using Spine.Unity;
using System;

public class SpecialOfferSpine : MonoBehaviour
{
    public DayOfWeek day;
    public SkeletonGraphic pack;
    [SpineSkin]
    public string skinPackEverybodyFavorite, skinPackDragonBreath, skinPackLetThereBeFire, skinPackSnippingForDummies,
        skinPackTaserLaser, skinPackShockingSale, skinPackEnthusiast;

    private bool isSetSkinDone;

    public void Show()
    {
        if (isSetSkinDone == false)
        {
            isSetSkinDone = true;
            SetSkin(day);
        }
    }

    public void SetSkin(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday:
                pack.Skeleton.SetSkin(skinPackEverybodyFavorite);
                break;

            case DayOfWeek.Tuesday:
                pack.Skeleton.SetSkin(skinPackDragonBreath);
                break;

            case DayOfWeek.Wednesday:
                pack.Skeleton.SetSkin(skinPackLetThereBeFire);
                break;

            case DayOfWeek.Thursday:
                pack.Skeleton.SetSkin(skinPackSnippingForDummies);
                break;

            case DayOfWeek.Friday:
                pack.Skeleton.SetSkin(skinPackTaserLaser);
                break;

            case DayOfWeek.Saturday:
                pack.Skeleton.SetSkin(skinPackShockingSale);
                break;

            case DayOfWeek.Sunday:
                pack.Skeleton.SetSkin(skinPackEnthusiast);
                break;
        }
    }
}
