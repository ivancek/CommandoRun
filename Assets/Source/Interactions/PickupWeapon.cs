using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : Pickup
{
    public override void Interact(Controller controller)
    {
        Soldier soldier = (Soldier)controller.GetControlledPawn();

        soldier.EquipPrimary(itemData.prefab);
        Destroy(gameObject);
    }
}
