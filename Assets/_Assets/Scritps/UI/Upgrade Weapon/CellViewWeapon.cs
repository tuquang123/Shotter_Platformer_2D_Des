using UnityEngine;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using UnityEngine.UI;
using Spine.Unity;

public class CellViewWeapon : EnhancedScrollerCellView
{
    public Text weaponName;
    public Image bg;
    public Sprite bgSelected;
    public Sprite bgUnselected;
    public Sprite bgLock;
    public Image imageWeapon;
    public GameObject labelEquipped;
    public GameObject labelSelected;
    public GameObject labelNew;
    public SkeletonGraphic effectUpgrade;
    public Color32 colorWeaponNameSelected;
    public Color32 colorWeaponNameUnselected;
    public Color32 colorWeaponNameLocked;
    public GameObject[] levelStars;

    private CellViewWeaponData _data;

    public void SetData(CellViewWeaponData data)
    {
        _data = data;

        UpdateInformation();
    }

    private void UpdateInformation()
    {
        imageWeapon.sprite = _data.weaponImage;
        imageWeapon.SetNativeSize();
        //imageWeapon.rectTransform.localScale = Vector3.one * _data.scaleRatioInShop;
        weaponName.text = _data.weaponName;
        labelEquipped.SetActive(_data.isEquipped);
        labelSelected.SetActive(_data.isSelected);
        labelNew.SetActive(_data.isNew);

        if (_data.isSelected)
        {
            bg.sprite = bgSelected;
            weaponName.color = colorWeaponNameSelected;
        }
        else
        {
            bg.sprite = _data.isLock ? bgLock : bgUnselected;
            weaponName.color = _data.isLock ? colorWeaponNameLocked : colorWeaponNameUnselected;
        }


        for (int i = 0; i < levelStars.Length; i++)
        {
            levelStars[i].SetActive(i < _data.level);
        }
    }

    public override void RefreshCellView()
    {
        base.RefreshCellView();

        UpdateInformation();

        if (_data.isUpgrading)
        {
            _data.isUpgrading = false;
            effectUpgrade.gameObject.SetActive(true);
            effectUpgrade.AnimationState.SetAnimation(0, "animation", false);
        }
    }

    public void SelectWeaponCellView()
    {
        labelNew.SetActive(false);

        EventDispatcher.Instance.PostEvent(EventID.SelectWeaponCellView, _data);

        if (GameData.isShowingTutorial)
        {
            if (_data.id == StaticValue.GUN_ID_UZI)
            {
                EventDispatcher.Instance.PostEvent(EventID.SubStepSelectUzi);
            }
            else if (_data.id == StaticValue.GUN_ID_KAME_POWER)
            {
                EventDispatcher.Instance.PostEvent(EventID.SubStepSelectKame);
            }
        }
    }
}
