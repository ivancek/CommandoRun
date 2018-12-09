using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


/// <summary>
/// Base class for all soldiers in the game.
/// </summary>
public class Soldier : Pawn, IDamageReceiver
{
    [Header("Setup")]
    public float MELEE_RANGE = 1.2f;
    public float RUN_SPEED = 4.8f;
    public float WALK_SPEED = 1.5f;
    public float MIN_ROT_SPEED = 100.0f;
    public float MAX_ROT_SPEED = 400.0f;
    public float MAX_HEALTH = 100.0f;

    // Set in Inspector
    public NavMeshAgent navAgent;
    public CapsuleCollider capsCollider;
    public Animator myAnimator;
    public Transform weaponContainer;


    // Private Properties for easier code reading.
    private int AnimatorSpeed { get { return isRunning ? 2 : 1; } }
    private float NavAgentSpeed { get { return isRunning ? RUN_SPEED : WALK_SPEED; } }
    private bool DestinationReached { get { return Vector3.Distance(transform.position, navAgent.destination) <= navAgent.stoppingDistance; } }

    // Private fields
    private IInteractable queuedInteraction;
    private IDamageReceiver queuedTarget;

    private EquipableDevice primaryDevice;
    private Quaternion targetRot;

    private bool isRunning;
    private bool isMoving;
    private bool forceRotation;
    private float desiredRotationSpeed;

    /// <summary>
    /// Actor Tick.
    /// </summary>
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        // We force the rotation to override navAgent's rotation speed. See SetDestination function.
        if(forceRotation)
        {
            RotateTowardsTargetRotation(targetRot);
        }

        if (isMoving)
        {
            if (DestinationReached)
            {
                myAnimator.SetInteger("speed", 0);
                Attack(queuedTarget);
                InteractWith(queuedInteraction);
                isMoving = false;
            }
        }
    }


    /// <summary>
    /// Quickly rotates towards target rotation.
    /// Sets forceRotation to false, once angle is lower than 5.
    /// Helper function to keep code cleaner.
    /// </summary>
    private void RotateTowardsTargetRotation(Quaternion newRot)
    {
        if(Quaternion.Angle(transform.rotation, newRot) > 5)
        {
            var step = desiredRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, step);
            return;
        }

        forceRotation = false;
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
    /// Just fucking Die!
    /// </summary>
    private void Die()
    {
        int randomInt = UnityEngine.Random.Range(1, 6);
        navAgent.enabled = false;
        capsCollider.enabled = false;
        myAnimator.Play(string.Format("Death{0}", randomInt));
    }


    /// <summary>
    /// MonoBehaviour Awake
    /// </summary>
    public override void Init()
    {
        base.Init();

        navAgent.speed = WALK_SPEED;
    }


    /// <summary>
    /// Sets queued interaction. When Soldier is out of range, we use this to remember
    /// what to do when he reaches the destination. Think walk to and use.
    /// </summary>
    /// <param name="interaction"></param>
    public void SetQueuedInteraction(IInteractable interaction)
    {
        queuedInteraction = interaction;
    }


    /// <summary>
    /// Sets queued interaction. When Soldier is out of range, we use this to remember
    /// what to do when he reaches the destination. Think walk to and use.
    /// </summary>
    /// <param name="interaction"></param>
    public void SetQueuedTarget(IDamageReceiver target)
    {
        queuedTarget = target;
    }


    /// <summary>
    /// Interacts with interaction and clears it.
    /// </summary>
    public void InteractWith(IInteractable interaction)
    {
        if(interaction != null)
        {
            interaction.Interact(GetController());
            SetQueuedInteraction(null);
        }
    }


    /// <summary>
    /// Deals damage the taret.
    /// </summary>
    public void Attack(IDamageReceiver target)
    {
        if(target != null && primaryDevice)
        {
            primaryDevice.Use(myAnimator);
            SetQueuedTarget(null);
        }
    }


    /// <summary>
    /// Sets the primary device by spawning it.
    /// </summary>
    public void EquipPrimary(ItemData deviceData)
    {
        primaryDevice = Instantiate(deviceData.prefab, weaponContainer, false);
        primaryDevice.SetData(deviceData);

        myAnimator.SetTrigger("draw");
    }


    /// <summary>
    /// Is the soldier in melee range from location?
    /// </summary>
    public bool IsInMeleeRange(Vector3 location)
    {
        return Vector3.Distance(transform.position, location) < MELEE_RANGE;
    }


    /// <summary>
    /// Is the soldier in melee range from location?
    /// </summary>
    public bool IsInWeaponRange(Vector3 location)
    {
        if(primaryDevice)
        {
            return Vector3.Distance(transform.position, location) < primaryDevice.Range;
        }

        return false;
    }

    /// <summary>
    /// Sets the destination of navAgent.
    /// </summary>
    public void SetDestination(Vector3 destination)
    {
        // We need to mark the soldier as moving. (needed in update).
        isMoving = true;

        forceRotation = true;

        targetRot = GetTargetRotation(destination);
        desiredRotationSpeed = GetDesiredRotationSpeed(targetRot);

        // Notify nav agent and animator of new changes.
        myAnimator.SetInteger("speed", AnimatorSpeed);
        navAgent.SetDestination(destination);
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


    /// <summary>
    /// Implementation for receive damage.
    /// </summary>
    public void ReceiveDamage(float damage)
    {
        if(damage >= MAX_HEALTH)
        {
            Die();
        }
    }
}
