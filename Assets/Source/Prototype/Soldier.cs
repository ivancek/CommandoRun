using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


/// <summary>
/// Base class for all soldiers in the game.
/// </summary>
public class Soldier : Pawn
{
    public float MELEE_RANGE = 1.2f;
    public float RUN_SPEED = 4.8f;
    public float WALK_SPEED = 1.5f;
    public float MIN_ROT_SPEED = 100.0f;
    public float MAX_ROT_SPEED = 400.0f;

    // Set in Inspector
    public NavMeshAgent navAgent;
    public Animator myAnimator;


    // Private Properties for easier code reading.
    private int AnimatorSpeed { get { return isRunning ? 2 : 1; } }
    private float NavAgentSpeed { get { return isRunning ? RUN_SPEED : WALK_SPEED; } }
    private bool DestinationReached { get { return Vector3.Distance(transform.position, navAgent.destination) <= navAgent.stoppingDistance; } }
    
    // Private fields
    private bool isRunning;
    private bool isMoving;
    private Quaternion targetRot;
    private float desiredRotationSpeed;

    /// <summary>
    /// MonoBehaviour Update.
    /// </summary>
    private void Update()
    {
        RotateTowardsTargetRotation(targetRot);

        if (isMoving)
        {
            if (DestinationReached)
            {
                myAnimator.SetInteger("speed", 0);
                isMoving = false;
            }
        }
    }



    /// <summary>
    /// Rotates towards target rotation
    /// </summary>
    private void RotateTowardsTargetRotation(Quaternion newRot)
    {
        var step = desiredRotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, step);
    }


    /// <summary>
    /// MonoBehaviour Awake
    /// </summary>
    public override void Init()
    {
        base.Init();

        navAgent.speed = WALK_SPEED;
    }


    public bool IsInMeleeRange(Vector3 location)
    {
        return Vector3.Distance(transform.position, location) < MELEE_RANGE;
    }


    /// <summary>
    /// Sets the destination of navAgent.
    /// </summary>
    public void SetDestination(Vector3 destination)
    {
        // We need to mark the soldier as moving. (needed in update).
        isMoving = true;

        targetRot = GetTargetRotation(destination);
        desiredRotationSpeed = GetDesiredRotationSpeed(targetRot);

        // Notify nav agent and animator of new changes.
        myAnimator.SetInteger("speed", AnimatorSpeed);
        navAgent.SetDestination(destination);
    }


    /// <summary>
    /// Calculates the desired rotation speed (angle percent) based of min and max rotation speed.
    /// </summary>
    private float GetDesiredRotationSpeed(Quaternion targetRotation)
    {
        // get angle
        float angle = Quaternion.Angle(targetRotation, transform.rotation);
        // max angle we can have is 180. We get the percentage of that;
        float percent = angle / 180;
        // we start with min rot speed and add the percent of max.
        float desiredSpeed = MIN_ROT_SPEED + MAX_ROT_SPEED * percent;

        return desiredSpeed;
    }


    /// <summary>
    /// Gets the target rotation based from destination.
    /// </summary>
    private Quaternion GetTargetRotation(Vector3 destination)
    {
        Vector3 direction = new Vector3(destination.x, transform.position.y, destination.z) - transform.position;
        return Quaternion.LookRotation(direction);
    }


    /// <summary>
    /// Toggles between running and walking.
    /// </summary>
    public void ToggleRunWalk()
    {
        // Toggle the running state.
        isRunning = !isRunning;

        // Set the new speed to the nav agent.
        navAgent.speed = NavAgentSpeed;

        // When toggling speed while moving, we need to update the animator speed parameter as well.
        // Otherwise, we would change moving speed, but animation would stay the same.
        if(isMoving)
        {
            myAnimator.SetInteger("speed", AnimatorSpeed);
        }
    }
}
