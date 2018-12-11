using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all Controllers
/// </summary>
public class Controller : Actor
{
    protected Pawn controlledPawn;

    /// <summary>
    /// Called when this controller has taken control of the pawn.
    /// </summary>
    public virtual void NotifyPawnControlled(Pawn controlledPawn) { }

    /// <summary>
    /// Called when this controller has taken control of the pawn.
    /// </summary>
    public virtual void NotifyPawnUncontrolled(Pawn controlledPawn) { }


    /// <summary>
    /// Initializes the Controller
    /// </summary>
    public override void Init()
    {
        base.Init();
    }


    /// <summary>
    /// Returns the Pawn currently controlled by this PlayerController.
    /// </summary>
    public Pawn GetControlledPawn()
    {
        return controlledPawn;
    }


    /// <summary>
    /// In order to send player commands, PlayerController must control the pawn.
    /// </summary>
    public void SetControlledPawn(Pawn pawnToControl)
    {
        controlledPawn = pawnToControl;

        if(pawnToControl != null)
        {
            controlledPawn.SetController(this);
            NotifyPawnControlled(controlledPawn);
        }
    }
}
