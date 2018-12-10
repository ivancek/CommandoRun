using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Perception component. Allows AI pawns to sense the player.
/// </summary>
public class PerceptionComponent : MonoBehaviour
{
    public float range = 10;
    
    /// <summary>
    /// Triggered when player is in range. This is coming from an update, so treat is as such.
    /// </summary>
    public System.Action<bool> OnPlayerSensed;

    private bool IsPlayerInRange { get { return Vector3.Distance(GameInstance.GameMode.PlayerPawn.transform.position, transform.position) < range; } }

    private void Update()
    {
        if(OnPlayerSensed != null)
        {
            OnPlayerSensed(IsPlayerInRange);
        }
    }


}
