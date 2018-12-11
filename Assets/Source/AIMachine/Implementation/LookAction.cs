using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Look")]
public class LookAction : StateAction
{
    public override void Act(AIController controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();
        soldier.LookAtTweened(controller.target.transform.position);

        DrawLine(controller);
    }

    private void DrawLine(AIController controller)
    {
        Vector3 start = new Vector3(controller.transform.position.x, 2.4f, controller.transform.position.z);
        Vector3 end = new Vector3(controller.target.transform.position.x, controller.target.GetComponent<CapsuleCollider>().center.y, controller.target.transform.position.z);

        Debug.DrawLine(start, end, Color.red);
    }
}