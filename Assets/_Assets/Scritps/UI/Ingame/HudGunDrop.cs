using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudGunDrop : MonoBehaviour
{
    public GameObject popup;
    public Text gunName;
    public Image gunImage;

    public void Init()
    {
        //EventDispatcher.Instance.RegisterListener(EventID.GetGunDrop, (sender, param) => Open((int)param));
    }

    public void Open(int gunId)
    {
        gunImage.sprite = GameResourcesUtils.GetGunImage(gunId);
        gunImage.SetNativeSize();

        if (GameData.staticGunData.ContainsKey(gunId))
        {
            gunName.text = GameData.staticGunData[gunId].gunName.ToUpper();
        }
        else
        {
            DebugCustom.LogError("Invalid gun id=" + gunId);
            gunName.text = string.Empty;
        }

        GameController.Instance.Player.enabled = false;
        GameController.Instance.SetActiveAllUnits(false);
        popup.SetActive(true);

        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_SHOW_DIALOG);
    }

    public void Close()
    {
        popup.SetActive(false);
        GameController.Instance.Player.enabled = true;
        GameController.Instance.SetActiveAllUnits(true);
    }
}
