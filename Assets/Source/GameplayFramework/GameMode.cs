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
    public GameState defaultGameState;
    public PlayerController defaultController;
    public Pawn defaultPawn;

    public Pawn PlayerPawn { get; private set; }
    public GameState GameState { get; private set; }
    public PlayerController PlayerController { get; private set; }


    /// <summary>
    /// Initializes the GameMode
    /// </summary>
    public override void Init()
    {
        base.Init();
        
        
        // We need the controller to control the game.
        PlayerController = Instantiate(defaultController, Vector3.zero, Quaternion.identity, transform);
        PlayerController.name = "PlayerController";

        // If there's a pawn defined, spawn it.
        if(defaultPawn)
        {
            // We need the player start for the pawn
            PlayerStart pStart = FindObjectOfType<PlayerStart>();

            if (pStart)
            {
                // Once we have the playerStart, spawn and init the pawn.
                PlayerPawn = Instantiate(defaultPawn, pStart.transform.position, pStart.transform.rotation);
                PlayerPawn.name = "PlayerPawn";

                // In order to control the pawn, the playerController must control it.
                PlayerController.SetControlledPawn(PlayerPawn);
            }
        }

        // Setup Game state
        GameState = Instantiate(defaultGameState, Vector3.zero, Quaternion.identity, transform);
        GameState.name = "GameState";
    }
}
