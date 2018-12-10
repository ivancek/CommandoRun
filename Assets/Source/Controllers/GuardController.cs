using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController
{
    private Soldier mySoldier;
    private Soldier playerSoldier;
    private PerceptionComponent soldierPerception;


    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        // Cache a reference to player's soldier
        playerSoldier = (Soldier)GameInstance.GameMode.PlayerPawn;

        // This controller is specific for the soldier. Cast.
        mySoldier = (Soldier)controlledPawn;
        mySoldier.gameObject.layer = 0;
        soldierPerception = mySoldier.gameObject.AddComponent<PerceptionComponent>();
        
        // Event subscriptions
        mySoldier.OnDeath += HandleSoldierDeath;
        soldierPerception.OnPlayerSensed += PlayerSensed;
    }

    private void PlayerSensed(bool isSensed)
    {
        if(isSensed)
        {
            if(playerSoldier.noiseEmitter.IsEmitingNoise)
            {
                KeepAttention();
            }
        }
        else
        {
            LoseAttention();
        }
    }

    
    private void HandleSoldierDeath()
    {
        mySoldier.OnDeath -= HandleSoldierDeath;
        soldierPerception.OnPlayerSensed -= PlayerSensed;
    }

    private void KeepAttention()
    {
        Vector3 targetPos = playerSoldier.transform.position;
        Debug.DrawLine(new Vector3(transform.position.x, 2.3f, transform.position.z), targetPos, Color.red);

        mySoldier.LookAtTweened(targetPos);
    }


    private void LoseAttention()
    {
        mySoldier.ResetRotation();
    }
}
