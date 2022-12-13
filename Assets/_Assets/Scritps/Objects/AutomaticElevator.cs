using UnityEngine;
using System.Collections;

public class AutomaticElevator : MonoBehaviour
{
    public GameObject elevator;
    public Transform botPoint;
    public Transform topPoint;
    public bool isMovingDown;
    public float speed = 2.5f;
    public float waitTime = 1.5f;

    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            if (isMovingDown)
            {
                if (elevator.transform.position.y > botPoint.position.y)
                {
                    elevator.transform.position = Vector2.MoveTowards(elevator.transform.position, botPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    isMoving = false;
                    isMovingDown = false;
                    elevator.transform.position = botPoint.position;
                    this.StartDelayAction(() =>
                    {
                        isMoving = true;
                    }, waitTime);
                }
            }
            else
            {
                if (elevator.transform.position.y < topPoint.position.y)
                {
                    elevator.transform.position = Vector2.MoveTowards(elevator.transform.position, topPoint.position, speed * Time.deltaTime);
                }
                else
                {
                    isMoving = false;
                    isMovingDown = true;
                    elevator.transform.position = topPoint.position;
                    this.StartDelayAction(() =>
                    {
                        isMoving = true;
                    }, waitTime);
                }
            }
        }
    }
}
