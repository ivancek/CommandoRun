using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HumanController is the play class for the player. It implements specific actions a player can do while playing.
/// </summary>
public class HumanController : PlayerController
{
    private Soldier soldier;

    /// <summary>
    /// Casts the controlled pawn to soldier. Need a reference for easier use later.
    /// </summary>
    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        soldier = (Soldier)GetControlledPawn();
    }

    /// <summary>
    /// Override this to add your own input bindings.
    /// </summary>
    /// <param name="component">Input component to bind to.</param>
    public override void InitializeInputComponent(InputComponent component)
    {
        base.InitializeInputComponent(component);

        component.BindAction("LeftClick", HandleLeftClick);
        component.BindAction("ToggleWalkRun", ToggleWalkRun);
    }


    /// <summary>
    /// Toggle soldier's move speed.
    /// </summary>
    private void ToggleWalkRun()
    {
        soldier.ToggleRunWalk();
    }


    /// <summary>
    /// Player controller fire method
    /// </summary>
    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit outHit;

        if(Physics.Raycast(ray, out outHit))
        {
            IInteractable interaction = outHit.transform.GetComponent<IInteractable>();
            soldier.SetQueuedInteraction(interaction);

            if (interaction != null)
            {
                if(soldier.IsInMeleeRange(outHit.transform.position))
                {
                    soldier.InteractWith(interaction);
                }
                else
                {
                    soldier.SetDestination(outHit.transform.position);
                }
            }

            if(outHit.transform.GetComponent<NavigationSurface>())
            {
                soldier.SetDestination(outHit.point);
            }

            
        }
    }
}
