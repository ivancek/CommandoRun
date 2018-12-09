using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum EInputEvent
{
    Pressed = 0,
    Released = 1,
    Repeat = 2
}


/// <summary>
/// Input component is responsible for handling all input coming from the PlayerController.
/// This is important, because some input might come from objects that implement IPointerClickHandler 
/// </summary>
public class InputComponent : MonoBehaviour
{
    public Dictionary<string, Action> pressBindings = new Dictionary<string, Action>();
    public Dictionary<string, Action> releaseBindings = new Dictionary<string, Action>();
    public Dictionary<string, Action> repeatBindings = new Dictionary<string, Action>();


    /// <summary>
    /// Monobehaviour Update
    /// </summary>
    private void Update()
    {
        /* Run each bindings set. Order is on purpose.
         * As soon as one binding is hit, we break the chain.
         * This is to reduce possible input bugs.
         * Press has priority, then repeat, then release.
         * 
         * Example: Player controller binds to LeftClick press and repeat.
         *          First instance of click will be handled by the press event,
         *          all following frames will process repeat bindings,
         *          and finally end with release.
         */


        if (RunPressBindings()) { return; }

        if (RunRepeatBindings()) { return; }

        if (RunReleaseBindings()) { return; }
    }


    /// <summary>
    /// Iterates through pressBindings and invokes any methods on the delegate.
    /// </summary>
    private bool RunPressBindings()
    {
        foreach (KeyValuePair<string, Action> kvp in pressBindings)
        {
            if (Input.GetButtonDown(kvp.Key))
            {
                if (kvp.Value != null)
                {
                    kvp.Value.Invoke();
                    return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Iterates through pressBindings and invokes any methods on the delegate.
    /// </summary>
    private bool RunRepeatBindings()
    {
        foreach (KeyValuePair<string, Action> kvp in repeatBindings)
        {
            if (Input.GetButton(kvp.Key))
            {
                if (kvp.Value != null)
                {
                    kvp.Value.Invoke();
                    return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Iterates through pressBindings and invokes any methods on the delegate.
    /// </summary>
    private bool RunReleaseBindings()
    {
        foreach (KeyValuePair<string, Action> kvp in releaseBindings)
        {
            if (Input.GetButtonUp(kvp.Key))
            {
                if (kvp.Value != null)
                {
                    kvp.Value.Invoke();
                    return true;
                }
            }
        }

        return false;
    }
    /// <summary>
    /// Binds a delegate to pressBindings under action name
    /// </summary>
    private void BindPress(string actionName, Action functionDelegate)
    {
        if (pressBindings.ContainsKey(actionName))
        {
            pressBindings[actionName] += functionDelegate;
        }
        else
        {
            pressBindings.Add(actionName, functionDelegate);
        }
    }


    /// <summary>
    /// Binds a delegate to releaseBindings under action name
    /// </summary>
    private void BindRelease(string actionName, Action functionDelegate)
    {
        if (releaseBindings.ContainsKey(actionName))
        {
            releaseBindings[actionName] += functionDelegate;
        }
        else
        {
            releaseBindings.Add(actionName, functionDelegate);
        }
    }


    /// <summary>
    /// Binds a delegate to repeat bindings under action name
    /// </summary>
    private void BindRepeat(string actionName, Action functionDelegate)
    {
        if (repeatBindings.ContainsKey(actionName))
        {
            repeatBindings[actionName] += functionDelegate;
        }
        else
        {
            repeatBindings.Add(actionName, functionDelegate);
        }
    }
    
    
    /// <summary>
    /// Binds an action to the input component.
    /// </summary>
    /// <param name="actionName">Action name to bind to</param>
    /// <param name="functionDelegate">Function to be called when action is invoked.</param>
    public void BindAction(string actionName, EInputEvent keyEvent, Action functionDelegate)
    {
        switch(keyEvent)
        {
            case EInputEvent.Pressed:
                BindPress(actionName, functionDelegate);
                break;
            case EInputEvent.Released:
                BindRelease(actionName, functionDelegate);
                break;
            case EInputEvent.Repeat:
                BindRepeat(actionName, functionDelegate);
                break;
        }
    }
}
