using UnityEngine;
using System.Collections;

public class EffectTextBANG : BaseEffect
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolTextBANG.Store(this);
    }
}
