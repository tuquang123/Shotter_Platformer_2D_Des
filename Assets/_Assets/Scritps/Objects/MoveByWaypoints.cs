using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveByWaypoints : MonoBehaviour
{
    [SerializeField]
    private bool startOnAwake = true;
    [SerializeField]
    private bool loop = true;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform[] waypoints;

    private int nextPointIndex = 1;

    void Start()
    {
        if (waypoints.Length <= 1)
        {
            enabled = false;
            return;
        }

        transform.position = waypoints[0].position;

        if (startOnAwake)
        {
            StartMove();
        }
    }

    void OnStepComplete()
    {
        nextPointIndex++;
        nextPointIndex %= waypoints.Length;

        if (nextPointIndex == 0 && loop == false)
        {
            StopMove();
        }
        else
        {
            StartMove();
        }
    }

    public void StartMove()
    {
        transform.DOMove(waypoints[nextPointIndex].position, speed).SetEase(Ease.Linear).SetSpeedBased(true).OnComplete(OnStepComplete);
    }

    public void StopMove()
    {
        transform.DOKill();
    }
}
