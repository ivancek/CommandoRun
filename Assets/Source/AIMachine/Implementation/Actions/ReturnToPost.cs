using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/ReturnToPost")]
public class ReturnToPost : StateAction
{
    /// <summary>
    /// What distance should the pawn stop at when moving
    /// </summary>
    public float moveStopDistance;

    public override void Act(AIController controller)
    {
        MoveToLocation(controller);
    }

    private void MoveToLocation(AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        soldier.SetDestination(controller.spawnPoint, moveStopDistance);
        soldier.navAgent.isStopped = false;
    }
}
