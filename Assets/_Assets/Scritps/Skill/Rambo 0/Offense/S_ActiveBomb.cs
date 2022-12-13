using UnityEngine;
using System.Collections;

public class S_ActiveBomb : BaseSkill
{
    public override void Excute()
    {
        base.Excute();

        EventDispatcher.Instance.PostEvent(EventID.ActiveBomb);
    }
}
