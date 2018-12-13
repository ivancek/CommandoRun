using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Instructions Talk")]
public class InstructionTalkAction : StateAction
{
    public override void Act(AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        soldier.speechComponent.InstructionsSpeak(controller.enemyStats.instructionsRate);
    }
}