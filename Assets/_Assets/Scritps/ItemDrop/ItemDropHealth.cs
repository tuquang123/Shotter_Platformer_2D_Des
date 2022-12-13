using UnityEngine;
using System.Collections;

public class ItemDropHealth : BaseItemDrop
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolItemDropHealth.Store(this);
    }
}
