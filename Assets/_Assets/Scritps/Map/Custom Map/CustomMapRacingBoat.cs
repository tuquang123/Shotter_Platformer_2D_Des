using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMapRacingBoat : BaseCustomMap
{
    public RubberBoat boatPrefab;
    public bool isBoatAutoMoveFirst = true;

    public override void Init(Map map)
    {
        RubberBoat boat = Instantiate(boatPrefab, map.playerSpawnPoint.position, map.playerSpawnPoint.rotation);
        boat.autoMove = isBoatAutoMoveFirst;

        GameController.Instance.Player = boat;
        GameController.Instance.AddUnit(boat.gameObject, boat);

        Rambo ramboPrefab = GameResourcesUtils.GetRamboPrefab(ProfileManager.UserProfile.ramboId);
        Rambo rambo = Instantiate(ramboPrefab);
        int id = ProfileManager.UserProfile.ramboId;
        int level = GameData.playerRambos.GetRamboLevel(id);
        rambo.Active(id, level, Vector2.zero);

        boat.GetIn(rambo);

        CameraFollow.Instance.SetTarget(boat.transform);
    }
}
