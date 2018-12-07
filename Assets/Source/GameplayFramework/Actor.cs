using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all objects that will be spawned in the world. Mimics UE4 framework.
/// </summary>
public class Actor : MonoBehaviour
{
    /// <summary>
    /// Initializes the Actor
    /// </summary>
    public virtual void Init()
    {
        Debug.LogFormat("{0} initialized.", name);
    }
}
