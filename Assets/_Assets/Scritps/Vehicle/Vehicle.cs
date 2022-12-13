using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : BaseUnit
{
    [Header("VEHICLE")]
    public float fuel;
    public bool infinityFuel = true;

    public virtual BaseUnit Player { get; }

    public abstract void Idle();
    public abstract void GetIn(Rambo rambo);
    public abstract void GetOut();
    //public abstract void ForceBack(float force);
}
