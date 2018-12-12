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
    [Header("Soldier")]
    public float MELEE_RANGE = 1.2f;
    public float RUN_SPEED = 4.8f;
    public float WALK_SPEED = 1.5f;
    public float MAX_HEALTH = 100.0f;

    // Set in Inspector
    public NavMeshAgent navAgent;
    public CapsuleCollider capsCollider;
    public NoiseEmitter noiseEmitter;
    public Animator myAnimator;
    public Transform weaponContainer;
    public SpeechComponent speechComponent;
    public AudioSource audioSource;
    public AudioClip[] hurtSounds;

    public bool canMove;

    // Events
    public System.Action OnDeath;


    // public Properties
    public EquipableDevice PrimaryDevice { get; protected set; } 
    public Transform Transform { get { return transform; } }
    public IDamageReceiver QueuedTarget { get; private set; }
    

    // Private Properties for easier code reading.
    private int AnimatorSpeed { get { return isRunning ? 2 : 1; } }
    private float NavAgentSpeed { get { return isRunning ? RUN_SPEED : WALK_SPEED; } }
    private bool DestinationReached { get { return Vector3.Distance(transform.position, navAgent.destination) <= navAgent.stoppingDistance; } }


    // Private fields
    private IInteractable queuedInteraction;
    private bool isRunning;
    
    
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

    
        if (!navAgent.isStopped)
        {
            SetTargetRotation(navAgent.desiredVelocity);

            if (DestinationReached)
            {
                StopMoving();
                Attack(QueuedTarget);
                InteractWith(queuedInteraction);
            }
        }

        RotateTowardsTargetRotation(targetRot);
    }


    /// <summary>
    /// All that is needed when the Soldier stops moving.
    /// </summary>
    private void StopMoving()
    {
        myAnimator.SetInteger("speed", 0);
        navAgent.isStopped = true;
        noiseEmitter.SetEmittingNoise(false);
    }


    /// <summary>
    /// Quickly rotates towards target rotation.
    /// Sets forceRotation to false, once angle is lower than 5.
    /// Helper function to keep code cleaner.
    /// </summary>
    private void RotateTowardsTargetRotation(Quaternion newRot)
    {
        if(Quaternion.Angle(transform.rotation, newRot) > 0.1f)
        {
            var step = desiredRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, step);
        }
    }


    /// <summary>
    /// Gets target rotation and desired rotation speed.
    /// This in turn uses Tick to quickly rotate, i.e. override navAgent's auto rotation.
    /// </summary>
    public void LookAtTweened(Vector3 position)
    {
        targetRot = GetTargetRotation(position);
        desiredRotationSpeed = GetDesiredRotationSpeed(targetRot);
    }


    /// <summary>
    /// Gets target rotation and desired rotation speed.
    /// This in turn uses Tick to quickly rotate, i.e. override navAgent's auto rotation.
    /// </summary>
    public void LookAtInstant(Vector3 position)
    {
        transform.rotation = GetTargetRotation(position);
        targetRot = transform.rotation;
    }


    

    
    /// <summary>
    /// Resets the rotation back to the rotation Soldier had on BeginPlay
    /// </summary>
    public void ResetRotation()
    {
        StopMoving();
        SetTargetRotation(defaultRot);
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
        enabled = false;

        if(PrimaryDevice)
        {
            PrimaryDevice.Drop();
        }

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
        QueuedTarget = target;
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
            canMove = false; // this will get set to true once we get out of attack animator state.
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
        StopMoving();

        canMove = false; // this will get set to true once we get out of draw weapon animator state.

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
        if(!canMove) { return; }
        
        // Let the nav agent move again.
        navAgent.isStopped = false;

        myAnimator.SetInteger("speed", AnimatorSpeed);

        navAgent.stoppingDistance = stoppingDistance;
        navAgent.SetDestination(destination);

        noiseEmitter.SetEmittingNoise(isRunning);
    }


    /// <summary>
    /// Toggles between running and walking.
    /// </summary>
    public void ToggleRunWalk()
    {
        // Toggle the running state.
        isRunning = !isRunning;

        navAgent.speed = NavAgentSpeed;

        // When toggling speed while moving, we need to update the animator speed parameter as well.
        // Otherwise, we would change moving speed, but animation would stay the same.
        if(!navAgent.isStopped)
        {
            noiseEmitter.SetEmittingNoise(isRunning);
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
