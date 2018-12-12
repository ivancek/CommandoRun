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
        bool isPlayerInRange = Vector3.Distance(GameInstance.GameMode.PlayerPawn.transform.position, controller.GetControlledPawn().transform.position) <= controller.enemyStats.lookRange;

        Vector3 directionToPlayer = GameInstance.GameMode.PlayerPawn.transform.position - controller.GetControlledPawn().transform.position;
        float dotProduct = Vector3.Dot(controller.GetControlledPawn().transform.forward, directionToPlayer.normalized);

        bool canSeePlayer = dotProduct > 0.5f;
        bool playerSeen = isPlayerInRange && canSeePlayer;

        if(playerSeen)
        {
            DrawLine(controller);
        }

        return playerSeen;
    }

    private void DrawLine(AIController controller)
    {
        Transform target = GameInstance.GameMode.PlayerPawn.transform;
        Vector3 start = new Vector3(controller.transform.position.x, 2.4f, controller.transform.position.z);
        Vector3 end = new Vector3(target.position.x, target.GetComponent<CapsuleCollider>().center.y, target.position.z);

        Debug.DrawLine(start, end, Color.red);
    }
}
