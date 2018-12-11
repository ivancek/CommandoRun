using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController
{
    private Soldier mySoldier;

    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        mySoldier = (Soldier)controlledPawn;
        mySoldier.gameObject.layer = 0;

        mySoldier.OnDeath += SoldierDied;
    }


    private void SoldierDied()
    {
        SetControlledPawn(null);
        mySoldier.OnDeath -= SoldierDied;
        mySoldier = null;
    }
}
