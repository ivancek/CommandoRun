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
        bool isPlayerInRange = Vector3.Distance(GameInstance.GameMode.PlayerPawn.transform.position, controller.GetControlledPawn().transform.position) <= controller.enemyStats.hearingRange;
        bool isPlayerAlive = GameInstance.GameMode.PlayerPawn.enabled;

        bool continueScanning = isPlayerAlive && isPlayerInRange;

        return continueScanning;
    }
}