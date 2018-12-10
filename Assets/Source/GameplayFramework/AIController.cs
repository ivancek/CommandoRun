using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all Controllers
/// </summary>
public class AIController : Controller
{
    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        Debug.LogFormat("AIController possessing {0}", controlledPawn.name);
    }
}
