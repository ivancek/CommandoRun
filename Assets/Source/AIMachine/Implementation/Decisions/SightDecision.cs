using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Sight")]
public class SightDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return See(controller);
    }

    private bool See(AIController controller)
    {
        bool playerSeen = false;

        bool isPlayerInRange = Vector3.Distance(controller.target.transform.position, controller.GetControlledPawn().transform.position) <= controller.enemyStats.lookRange;

        RaycastHit hit;
        Vector3 eyePosition = new Vector3(controller.transform.position.x, 2.4f, controller.transform.position.z);

        if(isPlayerInRange)
        {
            Vector3 direction = controller.target.transform.position - controller.transform.position;

            if (Physics.SphereCast(eyePosition, 1.0f, direction, out hit, controller.enemyStats.lookRange))
            {
                playerSeen = hit.collider.gameObject.layer == 11;

                Debug.DrawLine(eyePosition, hit.point, Color.red);
            }
        }
        else
        {
            if (Physics.SphereCast(eyePosition, 1.0f, controller.transform.forward, out hit, controller.enemyStats.lookRange))
            {
                playerSeen = hit.collider.gameObject.layer == 11;

                Debug.DrawLine(eyePosition, hit.point, Color.red);
            }
        }


        return playerSeen;
    }
}
