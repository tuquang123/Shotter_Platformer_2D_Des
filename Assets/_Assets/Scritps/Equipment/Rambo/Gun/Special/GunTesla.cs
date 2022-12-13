using UnityEngine;
using System.Collections;

public class GunTesla : BaseGun
{
    public Transform teslaPoint;
    public Tesla teslaPrefab;
    [HideInInspector]
    public BaseUnit shooter;

    private Tesla tesla;


    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_TESLA, level);
        baseStats = Resources.Load<SO_GunTeslaStats>(path);
    }

    protected override void Awake()
    {
        BaseUnit tmp = transform.root.GetComponent<BaseUnit>();

        if (tmp is Vehicle)
        {
            shooter = ((Vehicle)tmp).Player;
        }
        else
        {
            shooter = tmp;
        }

        CreateTesla();

        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, (sender, param) => ActiveTesla((bool)param));
    }

    private void OnDisable()
    {
        if (this)
            tesla.Active(false);
    }

    private void OnEnable()
    {
        if (this && ((Rambo)shooter).isFiring)
        {
            ActiveTesla(true);
        }
    }

    public override void Attack(AttackData attackData)
    {

    }

    private void ActiveTesla(bool isActive)
    {
        if (this && gameObject.activeInHierarchy)
        {
            if (isActive)
            {
                if (ammo <= 0)
                {
                    ammo = 0;
                    EventDispatcher.Instance.PostEvent(EventID.OutOfAmmo);
                    return;
                }
                else
                {
                    tesla.Active(true);
                }
            }
            else
            {
                tesla.Active(false);
            }
        }
    }

    private void CreateTesla()
    {
        tesla = Instantiate(teslaPrefab, teslaPoint.position, teslaPoint.rotation, firePoint);
        tesla.gun = this;
        tesla.gameObject.SetActive(false);
    }
}
