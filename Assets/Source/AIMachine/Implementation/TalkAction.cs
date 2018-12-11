using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Talk")]
public class TalkAction : StateAction
{
    public override void Act(int actionIndex, AIController controller)
    {
        if (controller.CheckIfCountDownElapsed(actionIndex, controller.enemyStats.talkRate))
        {
            Debug.LogFormat("Random sentence from {0}", controller.GetControlledPawn().name);
            controller.ResetActionTimer(actionIndex);
        }
    }
}