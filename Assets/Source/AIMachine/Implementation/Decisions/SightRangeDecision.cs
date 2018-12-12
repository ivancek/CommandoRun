using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/SightRange")]
public class SightRangeDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return Scan(controller);
    }

    private bool Scan(AIController controller)
    {
        bool isPlayerInRange = Vector3.Distance(GameInstance.GameMode.PlayerPawn.transform.position, controller.GetControlledPawn().transform.position) <= controller.enemyStats.lookRange;
        bool isPlayerAlive = GameInstance.GameMode.PlayerPawn.enabled;

        bool continueScanning = isPlayerAlive && isPlayerInRange;

        if (continueScanning)
        {
            DrawLine(controller);
        }

        return continueScanning;
    }

    private void DrawLine(AIController controller)
    {
        Transform target = GameInstance.GameMode.PlayerPawn.transform;
        Vector3 start = new Vector3(controller.transform.position.x, 2.4f, controller.transform.position.z);
        Vector3 end = new Vector3(target.position.x, target.GetComponent<CapsuleCollider>().center.y, target.position.z);

        Debug.DrawLine(start, end, Color.red);
    }
}