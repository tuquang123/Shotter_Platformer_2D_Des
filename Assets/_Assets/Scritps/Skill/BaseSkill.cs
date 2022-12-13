using UnityEngine;
using System.Collections;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillType type;
    public int id;
    public int level;
    public float value;

    public virtual void Excute() { }
}
