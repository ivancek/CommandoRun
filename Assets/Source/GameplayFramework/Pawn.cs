using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for controllable actors.
/// </summary>
public class Pawn : Actor
{
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
