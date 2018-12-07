using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Input component is responsible for handling all input coming from the PlayerController.
/// This is important, because some input might come from objects that implement IPointerClickHandler 
/// </summary>
public class InputComponent : MonoBehaviour
{
    public Dictionary<string, Action> actionBindings = new Dictionary<string, Action>();


    /// <summary>
    /// Monobehaviour Update
    /// </summary>
    private void Update()
    {
        // Iterate throught the dictionary and invoke inputs, if any.
        foreach(KeyValuePair<string, Action> kvp in actionBindings)
        {
            if(Input.GetButtonDown(kvp.Key))
            {
                if(kvp.Value != null)
                {
                    kvp.Value.Invoke();
                }
            }
        }
    }


    /// <summary>
    /// Binds an action to the input component.
    /// </summary>
    /// <param name="actionName">Action name to bind to</param>
    /// <param name="functionDelegate">Function to be called when action is invoked.</param>
    public void BindAction(string actionName, Action functionDelegate)
    {
        if(actionBindings.ContainsKey(actionName))
        {
            actionBindings[actionName] += functionDelegate;
        }
        else
        {
            actionBindings.Add(actionName, functionDelegate);
        }
    }
}
