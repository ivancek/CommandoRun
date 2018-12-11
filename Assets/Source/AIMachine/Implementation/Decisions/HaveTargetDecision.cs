using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HaveTarget")]
public class HaveTargetDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.target.enabled;
    }
}
