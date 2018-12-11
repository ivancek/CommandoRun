using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : StateAction
{
    /// <summary>
    /// What distance should the pawn stop at when attacking
    /// </summary>
    public float attackStopDistance;

    public override void Act(AIController controller)
    {
        Attack(controller);
    }

    private void Attack(AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        IDamageReceiver target = controller.target as IDamageReceiver;

        if (soldier.IsInWeaponRange(controller.target.transform.position))
        {
            soldier.Attack(target);
        }
        else
        {
            soldier.SetDestination(controller.target.transform.position, attackStopDistance);
        }
    }
}
