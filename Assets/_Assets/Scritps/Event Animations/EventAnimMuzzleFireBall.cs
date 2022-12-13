using UnityEngine;
using System.Collections;

public class EventAnimMuzzleFireBall : MonoBehaviour
{
    public BaseMuzzle mainMuzzle;

    public void Deactive()
    {
        mainMuzzle.Deactive();
    }
}
