using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeTab : MonoBehaviour
{
    public WeaponTab tab;
    public Image bg;
    public Image label;
    public GameObject notification;
    public Sprite bgSelect;
    public Sprite bgUnselect;
    public Sprite labelSelect;
    public Sprite labelUnselect;

    public void Highlight(bool isActive)
    {
        bg.sprite = isActive ? bgSelect : bgUnselect;
        bg.SetNativeSize();
        label.sprite = isActive ? labelSelect : labelUnselect;
        label.SetNativeSize();
    }

    public void UpdateNotification()
    {
        switch (tab)
        {
            case WeaponTab.Rifle:
                {
                    bool hasNewRifle = false;

                    foreach (KeyValuePair<int, PlayerGunData> gun in GameData.playerGuns)
                    {
                        if (gun.Value.isNew && GameData.staticGunData[gun.Key].isSpecialGun == false)
                        {
                            hasNewRifle = true;
                            break;
                        }
                    }

                    notification.SetActive(hasNewRifle);
                }

                break;

            case WeaponTab.Special:
                {
                    bool hasNewSpecial = false;

                    foreach (KeyValuePair<int, PlayerGunData> gun in GameData.playerGuns)
                    {
                        if (gun.Value.isNew && GameData.staticGunData[gun.Key].isSpecialGun)
                        {
                            hasNewSpecial = true;
                            break;
                        }
                    }

                    notification.SetActive(hasNewSpecial);
                }

                break;

            case WeaponTab.Grenade:
                {
                    bool hasNewGrenade = false;

                    foreach (KeyValuePair<int, PlayerGrenadeData> grenade in GameData.playerGrenades)
                    {
                        if (grenade.Value.isNew)
                        {
                            hasNewGrenade = true;
                            break;
                        }
                    }

                    notification.SetActive(hasNewGrenade);
                }

                break;

            case WeaponTab.MeleeWeapon:
                {
                    bool hasNewMeleeWeapon = false;

                    foreach (KeyValuePair<int, PlayerMeleeWeaponData> meleeWeapon in GameData.playerMeleeWeapons)
                    {
                        if (meleeWeapon.Value.isNew)
                        {
                            hasNewMeleeWeapon = true;
                            break;
                        }
                    }

                    notification.SetActive(hasNewMeleeWeapon);
                }

                break;
        }
    }

    public void OnClick()
    {
        EventDispatcher.Instance.PostEvent(EventID.SwichTabUpgradeWeapon, tab);
    }
}
