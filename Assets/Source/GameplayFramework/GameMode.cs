using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for GameMode. It sets the rules for each game mode.
/// It uses the GameState class to eleviate some work.
/// </summary>
public class GameMode : Actor
{
    [Header("Game Mode")]
    public GameState defaultGameState;
    public PlayerState defaultPlayerState;
    public PlayerController defaultController;
    public Pawn defaultPawn;
    public HUD defaultHUD;


    // Public properties
    public GameState GameState { get; private set; }
    public PlayerState PlayerState { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public Pawn PlayerPawn { get; private set; }
    public HUD HUD { get; private set; }



    /// <summary>
    /// Override this to add your own functionality.
    /// </summary>
    public virtual void Restart() { }



    /// <summary>
    /// Initializes the GameMode
    /// </summary>
    public override void Init()
    {
        base.Init();
        
        // Setup Game state
        GameState = Instantiate(defaultGameState, Vector3.zero, Quaternion.identity, transform);
        GameState.name = "GameState";
        
        // Setup Player State
        PlayerState = Instantiate(defaultPlayerState, Vector3.zero, Quaternion.identity, transform);
        PlayerState.name = "PlayerState";

        // Setup HUD
        HUD = Instantiate(defaultHUD, Vector3.zero, Quaternion.identity, transform);
        HUD.name = "HUD";

        // We need the controller to control the game.
        PlayerController = Instantiate(defaultController, Vector3.zero, Quaternion.identity, transform);
        PlayerController.name = "PlayerController";
        PlayerController.HUD = HUD;

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

    }
}
