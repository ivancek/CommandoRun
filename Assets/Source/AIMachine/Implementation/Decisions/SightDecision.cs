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
        Vector3 directionToPlayer = GameInstance.GameMode.PlayerPawn.transform.position - controller.GetControlledPawn().transform.position;
        float dotProduct = Vector3.Dot(controller.GetControlledPawn().transform.forward, directionToPlayer.normalized);

        bool isPlayerInRange = Vector3.Distance(controller.target.transform.position, controller.GetControlledPawn().transform.position) <= controller.enemyStats.lookRange;
        bool isPlayerInViewCone = dotProduct > 0.5f;

        RaycastHit hit;
        Vector3 eyePosition = new Vector3(controller.transform.position.x, 2.4f, controller.transform.position.z);

        if(isPlayerInRange && isPlayerInViewCone)
        {
            if (Physics.SphereCast(eyePosition, 1.0f, directionToPlayer, out hit, controller.enemyStats.lookRange))
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
