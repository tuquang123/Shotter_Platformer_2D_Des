using UnityEngine;
using System.Collections;

public class EffectTextCRIT : BaseEffect
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolTextCRIT.Store(this);
    }
}
