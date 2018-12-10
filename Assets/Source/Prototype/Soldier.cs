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
    public AudioSource audioSource;
    public AudioClip[] hurtSounds;


    // Events
    public System.Action OnDeath;


    // public Properties
    public EquipableDevice PrimaryDevice { get; protected set; } 
    public Transform Transform { get { return transform; } }


    // Private Properties for easier code reading.
    private int AnimatorSpeed { get { return isRunning ? 2 : 1; } }
    private float NavAgentSpeed { get { return isRunning ? RUN_SPEED : WALK_SPEED; } }
    private bool DestinationReached { get { return Vector3.Distance(transform.position, navAgent.destination) <= navAgent.stoppingDistance; } }


    // Private fields
    private IInteractable queuedInteraction;
    private IDamageReceiver queuedTarget;
    private Quaternion targetRot;
    private Quaternion defaultRot;
    private bool isRunning;
    private bool isMoving;
    private bool forceRotation;
    private float desiredRotationSpeed;

    
    
    /// <summary>
    /// MonoBehaviour Awake
    /// </summary>
    public override void Init()
    {
        base.Init();

        navAgent.speed = WALK_SPEED;
        defaultRot = transform.rotation;
    }


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
    /// Sets force rotation to true, gets target rotation and desired rotation speed.
    /// This in turn uses Tick to quickly rotate, i.e. override navAgent's auto rotation.
    /// </summary>
    public void LookAtTweened(Vector3 position)
    {
        forceRotation = true;

        targetRot = GetTargetRotation(position);
        desiredRotationSpeed = GetDesiredRotationSpeed(targetRot);
    }


    /// <summary>
    /// Sets force rotation to true, gets target rotation and desired rotation speed.
    /// This in turn uses Tick to quickly rotate, i.e. override navAgent's auto rotation.
    /// </summary>
    public void LookAtInstant(Vector3 position)
    {
        transform.rotation = GetTargetRotation(position);
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
    /// Resets the rotation back to the rotation Soldier had on BeginPlay
    /// </summary>
    public void ResetRotation()
    {
        SetTargetRotation(defaultRot);
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


        // After everything is done for death, we must invoke death event.
        if(OnDeath != null)
        {
            OnDeath.Invoke();
        }
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
        if(target != null && PrimaryDevice)
        {
            navAgent.isStopped = true;
            myAnimator.SetInteger("speed", 0);

            LookAtInstant(target.Transform.position);
            PrimaryDevice.Use(myAnimator);
            SetQueuedTarget(null);
        }
    }


    /// <summary>
    /// Sets the primary device by spawning it.
    /// </summary>
    public void EquipPrimary(ItemData deviceData)
    {
        PrimaryDevice = Instantiate(deviceData.prefab, weaponContainer, false);
        PrimaryDevice.SetData(deviceData);
        PrimaryDevice.gameObject.SetActive(false);

        

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
        if(PrimaryDevice)
        {
            float distance = Vector3.Distance(transform.position, location);
            return distance < PrimaryDevice.Range;
        }

        return false;
    }

    /// <summary>
    /// Sets the destination of navAgent.
    /// </summary>
    public void SetDestination(Vector3 destination, float stoppingDistance)
    {
        // Let the nav agent move again.
        navAgent.isStopped = false;

        // We need to mark the soldier as moving. (needed in update).
        isMoving = true;

        // Request quick rotation. 
        LookAtTweened(destination);

        // Notify nav agent and animator of new changes.
        myAnimator.SetInteger("speed", AnimatorSpeed);

        navAgent.stoppingDistance = stoppingDistance;
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
        if(!audioSource.isPlaying)
        {
            int randomInt = UnityEngine.Random.Range(0, hurtSounds.Length);
            audioSource.clip = hurtSounds[randomInt];
            audioSource.Play();
        }

        if(damage >= MAX_HEALTH)
        {
            Die();
        }
    }
}
