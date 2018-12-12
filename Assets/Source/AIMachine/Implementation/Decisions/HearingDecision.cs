using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Hearing")]
public class HearingDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return Hear(controller);
    }

    private bool Hear(AIController controller)
    {
        bool isPlayerInRange = Vector3.Distance(GameInstance.GameMode.PlayerPawn.transform.position, controller.GetControlledPawn().transform.position) <= controller.enemyStats.hearingRange;
        bool isMakingNoise = ((Soldier)GameInstance.GameMode.PlayerPawn).noiseEmitter.IsEmitingNoise;

        bool playerHeard = isPlayerInRange && isMakingNoise;

        return playerHeard;
    }
}
