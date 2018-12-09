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

    /// <summary>
    /// MonoBehaviour update. It runs the Tick function, allowing easier Update overriding 
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
    public virtual void Init()
    {
        Debug.LogFormat("{0} initialized.", name);
    }

    public virtual void Tick(float deltaTime) { }
}
