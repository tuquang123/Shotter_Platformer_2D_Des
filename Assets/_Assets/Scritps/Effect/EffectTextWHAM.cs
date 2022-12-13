using UnityEngine;
using System.Collections;

public class EffectTextWHAM : BaseEffect
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolTextWHAM.Store(this);
    }
}
