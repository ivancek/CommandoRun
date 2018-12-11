using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : StateAction
{
    public override void Act(int actionIndex, AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        soldier.ResetRotation();
    }
}