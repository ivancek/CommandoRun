using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController
{
    private Pawn playerPawn;
    private Soldier mySoldier;
    private PerceptionComponent soldierPerception;

    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        // This controller is specific for the soldier. Cast.
        mySoldier = (Soldier)controlledPawn;
        mySoldier.gameObject.layer = 0;
        soldierPerception = mySoldier.gameObject.AddComponent<PerceptionComponent>();

        // Event subscriptions
        mySoldier.OnDeath += HandleSoldierDeath;
        soldierPerception.OnPlayerSensed += SetPlayerPawn;
        soldierPerception.OnPlayerLost += ClearPlayerPawn;
    }


    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (playerPawn)
        {
            mySoldier.LookAtTweened(playerPawn.transform.position);
        }
    }


    private void HandleSoldierDeath()
    {
        soldierPerception.OnPlayerSensed -= SetPlayerPawn;
        soldierPerception.OnPlayerLost -= ClearPlayerPawn;
    }


    private void ClearPlayerPawn()
    {
        SetPlayerPawn(null);
        mySoldier.ResetRotation();
    }


    private void SetPlayerPawn(Pawn obj)
    {
        playerPawn = obj;
    }
        
}
