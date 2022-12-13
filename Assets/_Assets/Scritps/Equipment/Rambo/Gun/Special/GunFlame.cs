using UnityEngine;
using System.Collections;

public class GunFlame : BaseGun
{
    [HideInInspector]
    public BaseUnit shooter;
    public Flame flame;


    public override void LoadScriptableObject()
    {
        string path = string.Format(StaticValue.FORMAT_PATH_BASE_STATS_GUN_FLAME, level);
        baseStats = Resources.Load<SO_GunFlameStats>(path);
    }

    protected override void Awake()
    {
        shooter = transform.root.GetComponent<BaseUnit>();

        EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, (sender, param) => ActiveFlame((bool)param));
    }

    public override void Attack(AttackData attackData) { }

    private void ActiveFlame(bool isActive)
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
                    flame.Active();
                }
            }
            else
            {
                flame.Deactive();
            }
        }
    }
}
