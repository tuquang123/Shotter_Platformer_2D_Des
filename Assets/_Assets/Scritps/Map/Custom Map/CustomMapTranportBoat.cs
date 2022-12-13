using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMapTranportBoat : BaseCustomMap
{
    public RubberBoat boatPrefab;
    public Transform stopPoint;

    private RubberBoat boat;

    void Start()
    {
        boat = Instantiate(boatPrefab, transform.position, transform.rotation);
        boat.playerStopPoint = stopPoint;
        boat.autoMove = false;

        //GameController.Instance.Player = boat;
        GameController.Instance.AddUnit(boat.gameObject, boat);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag(StaticValue.TAG_PLAYER))
        {
            gameObject.SetActive(false);
            boat.GetIn((Rambo)GameController.Instance.Player);
            boat.StartMove();
        }
    }
}
