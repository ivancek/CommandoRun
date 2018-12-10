using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for controllable actors.
/// </summary>
public class Pawn : Actor
{
    [Header("Pawn")]
    public AIController AIControllerPrefab;


    /// <summary>
    /// Controller responsible for this pawn.
    /// </summary>
    protected Controller myController;


    /// <summary>
    /// Initializes the Pawn.
    /// </summary>
    public override void Init()
    {
        base.Init();

        // Pawns placed in the world by hand or spawned from specific prefab should have aiControllerPefab defined.
        // Therefore, instantiate the controller as child of this pawn and give control to the controller.
        // Doing this will trigger NotifyPawnControlled() which you can override in your own aiController implementations.
        if (AIControllerPrefab)
        {
            Instantiate(AIControllerPrefab, transform, false).SetControlledPawn(this);
        }
    }


    /// <summary>
    /// Sets the controller
    /// </summary>
    public void SetController(Controller controller)
    {
        myController = controller;
    }


    public Controller GetController()
    {
        return myController;
    }
}
