using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperLaser : MonoBehaviour
{
    public LineRenderer laserRender;
    public LayerMask collisionMask;
    public float laserWidth = 0.3f;
    public float laserRange = 10f;

    private RaycastHit2D hit;

    void Start()
    {
        laserRender.startWidth = laserWidth;
        laserRender.endWidth = laserWidth;
    }

    void LateUpdate()
    {
        if (laserRender != null)
        {
            hit = Physics2D.Linecast(transform.position, transform.position + transform.right * laserRange, collisionMask);

            Vector3 hitPoint;

            if (hit)
            {
                laserRender.SetPosition(0, transform.position);
                hitPoint = hit.point;
                laserRender.SetPosition(1, hitPoint);
            }
            else
            {
                laserRender.SetPosition(0, transform.position);
                hitPoint = transform.position + transform.right * laserRange;
                laserRender.SetPosition(1, hitPoint);
            }
        }
    }
}
