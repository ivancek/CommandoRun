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
    public float MIN_ROT_SPEED = 100.0f;
    public float MAX_ROT_SPEED = 400.0f;


    /// <summary>
    /// Controller responsible for this pawn.
    /// </summary>
    protected Controller myController;

    protected Quaternion targetRot;
    protected Quaternion defaultRot;
    protected float desiredRotationSpeed;


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


    /// <summary>
    /// REturns the current controller for this pawn. Can be null.
    /// </summary>
    public Controller GetController()
    {
        return myController;
    }


    /// <summary>
    /// Sets target rotation
    /// </summary>
    public void SetTargetRotation(Quaternion rotation)
    {
        targetRot = rotation;
        desiredRotationSpeed = GetDesiredRotationSpeed(targetRot);
    }


    /// <summary>
    /// Sets target rotation
    /// </summary>
    public void SetTargetRotation(Vector3 direction)
    {
        targetRot = direction == Vector3.zero ? transform.rotation : Quaternion.LookRotation(direction);
        desiredRotationSpeed = GetDesiredRotationSpeed(targetRot);
    }


    /// <summary>
    /// Gets the target rotation based from destination.
    /// </summary>
    public Quaternion GetTargetRotation(Vector3 position)
    {
        Vector3 direction = new Vector3(position.x, transform.position.y, position.z) - transform.position;
        return Quaternion.LookRotation(direction);
    }


    /// <summary>
    /// Calculates the desired rotation speed (angle percent) based of min and max rotation speed.
    /// </summary>
    public float GetDesiredRotationSpeed(Quaternion targetRotation)
    {
        // get angle
        float angle = Quaternion.Angle(targetRotation, transform.rotation);
        // max angle we can have is 180. We get the percentage of that;
        float percent = angle / 180;
        // we start with min rot speed and add the percent of max.
        float desiredSpeed = MIN_ROT_SPEED + MAX_ROT_SPEED * percent;

        return desiredSpeed;
    }
}
