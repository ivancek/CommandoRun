﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Perception component. Allows AI pawns to sense the player.
/// </summary>
public class PerceptionComponent : MonoBehaviour
{
    public float range = 10;
    
    public bool IsPlayerInRange { get { return Vector3.Distance(GameInstance.GameMode.PlayerPawn.transform.position, transform.position) < range; } }

    


}
