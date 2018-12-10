using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all objects that will be spawned in the world. Mimics UE4 framework.
/// </summary>
public class Actor : MonoBehaviour
{
    [Header("Actor")]
    // Set this to true if you are overriding Tick(), otherwise it wont tick :)
    public bool canTick = true;
    // Should this actor init itself on gameObject awake?
    public bool initOnAwake = true;


    /// <summary>
    /// MonoBehaviour awake. It runs the Init function, allowing easier Awake overriding.
    /// </summary>
    private void Start()
    {
        if(initOnAwake)
        {
            Init();
        }
    }


    /// <summary>
    /// MonoBehaviour update. It runs the Tick function, allowing easier Update overriding.
    /// Add code to Tick, not Update.
    /// </summary>
    private void Update()
    {
        if(canTick)
        {
            Tick(Time.deltaTime);
        }
    }

    /// <summary>
    /// Initializes the Actor
    /// </summary>
    public virtual void Init() { }

    /// <summary>
    /// Use this to update the actor every frame.
    /// </summary>
    public virtual void Tick(float deltaTime) { }
}
