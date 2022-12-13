using UnityEngine;
using System.Collections;

public class S_Rage : BaseSkill
{
    public override void Excute()
    {
        base.Excute();

        EventDispatcher.Instance.PostEvent(EventID.ActiveRage);
    }
}
