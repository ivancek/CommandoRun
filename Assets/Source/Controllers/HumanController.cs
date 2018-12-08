using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HumanController is the play class for the player. It implements specific actions a player can do while playing.
/// </summary>
public class HumanController : PlayerController
{
    private Soldier soldier;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 lastMousePosition;
    private IInteractable previousInteraction;



    /// <summary>
    /// MonoBehaviour Update
    /// </summary>
    private void Update()
    {
        RaycastIntoWorld();
    }


    /// <summary>
    /// Casts a ray into the world
    /// </summary>
    private void RaycastIntoWorld()
    {
        // Only raycast if we moved the mouse.
        if (lastMousePosition != Input.mousePosition)
        {
            // Get the ray from screen
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast
            if (Physics.Raycast(ray, out hit))
            {
                IInteractable currentInteraction = hit.transform.GetComponent<IInteractable>();

                if(currentInteraction != previousInteraction)
                {
                    // Notify previousInteraction (if any) that mouse is out.
                    if(previousInteraction != null)
                    {
                        previousInteraction.MouseOut();
                    }
                
                    // Notify current intaraction of mouse over.
                    if (currentInteraction != null)
                    {
                        currentInteraction.MouseOver();
                    }

                }

                // Set previous interaciton to whatever it is now.
                previousInteraction = currentInteraction;
            }


            // Finally, set last mouse pos to current mouse pos to prevent raycasting next frame.
            lastMousePosition = Input.mousePosition;
        }
    }


    /// <summary>
    /// Casts the controlled pawn to soldier. Need a reference for easier use later.
    /// </summary>
    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        soldier = (Soldier)GetControlledPawn();
    }

    /// <summary>
    /// Override this to add your own input bindings.
    /// </summary>
    /// <param name="component">Input component to bind to.</param>
    public override void InitializeInputComponent(InputComponent component)
    {
        base.InitializeInputComponent(component);

        component.BindAction("LeftClick", HandleLeftClick);
        component.BindAction("ToggleWalkRun", ToggleWalkRun);
    }


    /// <summary>
    /// Toggle soldier's move speed.
    /// </summary>
    private void ToggleWalkRun()
    {
        soldier.ToggleRunWalk();
    }


    /// <summary>
    /// Player controller fire method
    /// </summary>
    private void HandleLeftClick()
    {
        if (hit.transform.GetComponent<NavigationSurface>())
        {
            soldier.SetDestination(hit.point);
        }
    }
}
