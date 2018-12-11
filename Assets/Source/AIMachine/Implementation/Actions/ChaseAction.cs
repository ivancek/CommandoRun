using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : StateAction
{
    /// <summary>
    /// What distance should the pawn stop at when moving
    /// </summary>
    public float moveStopDistance;

    public override void Act(AIController controller)
    {
        Chase(controller);
    }

    private void Chase(AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        soldier.SetDestination(controller.target.transform.position, moveStopDistance);
        soldier.navAgent.isStopped = false;
    }
}
