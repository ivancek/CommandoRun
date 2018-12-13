using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Investigate")]
public class InvestigateDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return Investigate(controller);
    }

    private bool Investigate(AIController controller)
    {
        return controller.CheckIfCountDownElapsed(controller.enemyStats.investigateDuration);
    }
}
