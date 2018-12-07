using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for GameMode. It sets the rules for each game mode.
/// It uses the GameState class to eleviate some work.
/// </summary>
public class GameMode : Actor
{
    [Header("Setup")]
    public PlayerController defaultController;
    public Pawn defaultPawn;


    protected Pawn pawnInstance;

    /// <summary>
    /// Initializes the GameMode
    /// </summary>
    public override void Init()
    {
        base.Init();
        
        // TODO: GameState implementation

        // We need the controller to control the game.
        PlayerController pController = Instantiate(defaultController, Vector3.zero, Quaternion.identity, transform);
        pController.Init();

        // If there's a pawn defined, spawn it.
        if(defaultPawn)
        {
            // We need the player start for the pawn
            PlayerStart pStart = FindObjectOfType<PlayerStart>();

            if (pStart)
            {
                // Once we have the playerStart, spawn and init the pawn.
                pawnInstance = Instantiate(defaultPawn, pStart.transform.position, pStart.transform.rotation);
                pawnInstance.Init();

                // In order to control the pawn, the playerController must control it.
                pController.SetControlledPawn(pawnInstance);
            }
        }
    }
}
