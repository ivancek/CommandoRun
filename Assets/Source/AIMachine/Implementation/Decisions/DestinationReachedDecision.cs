using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/DestinationReached")]
public class DestinationReachedDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return DestinationReached(controller);
    }

    private bool DestinationReached(AIController controller)
    {
        return ((Soldier)controller.GetControlledPawn()).DestinationReached;
    }

    private void DrawLine(AIController controller)
    {
        Transform target = GameInstance.GameMode.PlayerPawn.transform;
        Vector3 start = new Vector3(controller.transform.position.x, 2.4f, controller.transform.position.z);
        Vector3 end = new Vector3(target.position.x, target.GetComponent<CapsuleCollider>().center.y, target.position.z);

        Debug.DrawLine(start, end, Color.red);
    }
}