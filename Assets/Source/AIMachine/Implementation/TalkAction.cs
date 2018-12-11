using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Talk")]
public class TalkAction : StateAction
{
    public override void Act(AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        soldier.speechComponent.RandomSpeak(controller.enemyStats.talkRate);
    }
}