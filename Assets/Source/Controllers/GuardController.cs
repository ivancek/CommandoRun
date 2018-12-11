using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : AIController
{
    protected Soldier soldier;

    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        soldier = (Soldier)controlledPawn;
        soldier.gameObject.layer = 0;

        soldier.OnDeath += SoldierDied;
    }


    private void SoldierDied()
    {
        SetControlledPawn(null);
        soldier.OnDeath -= SoldierDied;
        soldier = null;
    }
}
