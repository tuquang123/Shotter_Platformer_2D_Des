using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static void FaceTo(this Transform transform, Transform target, float speed)
    {
        Vector2 v = target.position - transform.position;

        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    public static void StartDelayAction(this MonoBehaviour mono, Action callback, float time)
    {
        mono.StartCoroutine(Delay(callback, time));
    }

    public static void StartActionEndOfFrame(this MonoBehaviour mono, Action callback)
    {
        mono.StartCoroutine(DelayEndOfFrame(callback));
    }

    private static IEnumerator Delay(Action callBack, float time)
    {
        yield return new WaitForSeconds(time);
        callBack.Invoke();
    }

    private static IEnumerator DelayEndOfFrame(Action callBack)
    {
        yield return new WaitForEndOfFrame();
        callBack.Invoke();
    }
}