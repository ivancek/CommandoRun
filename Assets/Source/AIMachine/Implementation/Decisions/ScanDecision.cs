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
        int layerMask = 1 << 11;
        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, controller.enemyStats.searchRange, layerMask, QueryTriggerInteraction.Collide);
        controller.target = colliders.Length > 0 ? colliders[0].gameObject.GetComponent<Actor>() : null;
    
        return colliders.Length > 0;
    }
}