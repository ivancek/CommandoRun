using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HumanController is the play class for the player. It implements specific actions a player can do while playing.
/// </summary>
public class HumanController : PlayerController
{
    [Header("Human Controller")]
    /// <summary>
    /// What distance should the pawn stop at when attacking
    /// </summary>
    public float attackStopDistance;
    
    /// <summary>
    /// What distance should the pawn stop at when moving
    /// </summary>
    public float moveStopDistance;

    /// <summary>
    /// What distance should the pawn stop at when interacting with objects
    /// </summary>
    public float useStopDistance;


    private Soldier soldier;
    
    
    
    /// <summary>
    /// Casts the controlled pawn to soldier. Need a reference for easier use later.
    /// </summary>
    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        soldier = (Soldier)controlledPawn;
        soldier.OnDeath += SoldierDied;
    }


    private void SoldierDied()
    {
        SetControlledPawn(null);
        soldier.OnDeath -= SoldierDied;
        soldier = null;
    }

    /// <summary>
    /// Override this to add your own input bindings.
    /// </summary>
    /// <param name="component">Input component to bind to.</param>
    public override void InitializeInputComponent(InputComponent component)
    {
        base.InitializeInputComponent(component);

        component.BindAction("LeftClick", EInputEvent.Repeat, HandleMouseButtonDown);
        component.BindAction("ToggleWalkRun", EInputEvent.Pressed, ToggleWalkRun);
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
    private void HandleMouseButtonDown()
    {
        if(!controlledPawn) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit outHit;

        // Set mask to ignore layer 11 & 12 (player & melee weapon) 
        int layerMask = 1 << 11;
        layerMask |= 1 << 12;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out outHit, 100, layerMask, QueryTriggerInteraction.Collide))
        {
            if (HandleAttack(outHit)) return;
            if (HandleInteraction(outHit)) return;
            if (HandleNavigation(outHit)) return;
        }
    }


    /// <summary>
    /// Checks for IDamageReceiver in RaycastHit result.
    /// If found, attacks when in range, otherwise sets destination.
    /// </summary>
    /// <param name="outHit"></param>
    /// <returns></returns>
    private bool HandleAttack(RaycastHit outHit)
    {
        IDamageReceiver target = outHit.transform.GetComponent<IDamageReceiver>();

        soldier.SetQueuedTarget(target);

        if (target == null)
        {
            return false;
        }

        if (soldier.IsInWeaponRange(outHit.transform.position))
        {
            soldier.Attack(target);
        }
        else
        {
            soldier.SetDestination(outHit.transform.position, attackStopDistance);
        }

        return true;
    }


    /// <summary>
    /// Checks for IInteractable in RaycastHit result. 
    /// If found, interacts when in range, otherwise sets destination.
    /// </summary>
    private bool HandleInteraction(RaycastHit outHit)
    {
        IInteractable interaction = outHit.transform.GetComponent<IInteractable>();
        soldier.SetQueuedInteraction(interaction);

        if (interaction == null)
        {
            return false;
        }

        if (soldier.IsInMeleeRange(outHit.transform.position))
        {
            soldier.InteractWith(interaction);
        }
        else
        {
            soldier.SetDestination(outHit.transform.position, useStopDistance);
        }

        return true;
    }


    /// <summary>
    /// Sets soldier destination if surface found.
    /// </summary>
    private bool HandleNavigation(RaycastHit outHit)
    {
        if (outHit.transform.GetComponent<NavigationSurface>())
        {
            soldier.SetDestination(outHit.point, moveStopDistance);
            return true;
        }

        return false;
    }
}
