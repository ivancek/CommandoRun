using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the PlayerController. Inherit this to make your own player controllers.
/// </summary>
public class PlayerController : Controller
{
    public HUD HUD { get; set; }

    /// <summary>
    /// Override this to add your own input bindings.
    /// </summary>
    /// <param name="component">Input component to bind to.</param>
    public virtual void InitializeInputComponent(InputComponent component) { }
    
    
    /// <summary>
    /// Initializes the PlayerController
    /// </summary>
    public override void Init()
    {
        base.Init();

        InitializeInputComponent(gameObject.AddComponent<InputComponent>());
    }
}
