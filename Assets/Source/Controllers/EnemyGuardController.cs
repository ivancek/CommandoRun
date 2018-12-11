using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuardController : GuardController
{
    [Header("Enemy Guard Controller")]
    /// <summary>
    /// Primary device asset data to use.
    /// </summary>
    public WeaponData primaryDevice;

    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        soldier.EquipPrimary(primaryDevice);
    }
}
