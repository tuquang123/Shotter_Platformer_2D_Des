using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxObject : MonoBehaviour
{
    public float nearSpeed;
    public float middleSpeed;
    public float farSpeed;
    public Transform endPoint;
    public Transform startPoint;
    public Transform[] nearObjects;
    public Transform[] middleObjects;
    public Transform[] farObjects;

    private float lastCameraX;

    IEnumerator Start()
    {
        if (nearObjects.Length <= 0 && farObjects.Length <= 0)
        {
            enabled = false;
        }

        yield return StaticValue.waitHalfSec;

        lastCameraX = Camera.main.transform.position.x;
    }

    void Update()
    {
        if (GameController.Instance.Player == null)
            return;

        float sign = -Mathf.Sign(Camera.main.transform.position.x - lastCameraX);

        //if (lastCameraX != Camera.main.transform.position.x && GameController.Instance.Player.IsMoving)
        if (Mathf.Abs(lastCameraX - Camera.main.transform.position.x) > 0.02f && GameController.Instance.Player.IsMoving)
        {
            lastCameraX = Camera.main.transform.position.x;

            // Near objects
            float deltaParalax = sign * nearSpeed * Time.deltaTime;

            for (int i = 0; i < nearObjects.Length; i++)
            {
                Vector3 v = nearObjects[i].position;
                v.x += deltaParalax;

                // Clamp
                if (nearObjects[i].position.x < endPoint.position.x)
                {
                    v.x = startPoint.position.x;
                }
                else if (nearObjects[i].position.x > startPoint.position.x)
                {
                    v.x = endPoint.position.x;
                }

                nearObjects[i].position = v;
            }

            // Middle objects
            deltaParalax = sign * middleSpeed * Time.deltaTime;

            for (int i = 0; i < middleObjects.Length; i++)
            {
                Vector3 v = middleObjects[i].position;
                v.x += deltaParalax;

                // Clamp
                if (middleObjects[i].position.x < endPoint.position.x)
                {
                    v.x = startPoint.position.x;
                }
                else if (middleObjects[i].position.x > startPoint.position.x)
                {
                    v.x = endPoint.position.x;
                }

                middleObjects[i].position = v;
            }

            // Far objects
            deltaParalax = sign * farSpeed * Time.deltaTime;

            for (int i = 0; i < farObjects.Length; i++)
            {
                Vector3 v = farObjects[i].position;
                v.x += deltaParalax;

                // Clamp
                if (farObjects[i].position.x < endPoint.position.x)
                {
                    v.x = startPoint.position.x;
                }
                else if (farObjects[i].position.x > startPoint.position.x)
                {
                    v.x = endPoint.position.x;
                }

                farObjects[i].position = v;
            }
        }
    }
}
