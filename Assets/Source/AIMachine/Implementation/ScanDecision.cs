using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
public class ScanDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return Scan(controller);
    }

    private bool Scan(AIController controller)
    {
        controller.navMeshAgent.isStopped = true;

        int layerMask = 1 << 11;
        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.enemyStats.searchRange, layerMask, QueryTriggerInteraction.Collide);

        if(colliders.Length > 0)
        {
            if(!controller.target)
            {
                controller.target = colliders[0].gameObject.GetComponent<Pawn>();
            }
        }
        else
        {
            controller.target = null;
        }

        return colliders.Length > 0;
    }
}