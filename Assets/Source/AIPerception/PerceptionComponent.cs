using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Perception component. Allows AI pawns to sense the player.
/// </summary>
public class PerceptionComponent : MonoBehaviour
{
    public float range = 2;

    public System.Action<Pawn> OnPlayerSensed;
    public System.Action OnPlayerLost;

    private RaycastHit hit;
    private Collider[] result;
    private bool shouldNotifySense = true; 
    private bool shouldNotifyLost = true;

    private void Update()
    {
        int layerMask = 1 << 11;
        result = Physics.OverlapSphere(transform.position, range, layerMask, QueryTriggerInteraction.Collide);
        
        // The frame player enters, result will be 1
        if(result.Length > 0)
        {
            // Prevent multiple event calls.
            if (shouldNotifySense) 
            {
                shouldNotifySense = false;
                shouldNotifyLost = true;

                if (OnPlayerSensed != null)
                {
                    OnPlayerSensed.Invoke(result[0].GetComponent<Pawn>());
                }
                
            }
        }
        else
        {
            // Prevent multiple event calls.
            if (shouldNotifyLost) 
            {
                shouldNotifySense = true;
                shouldNotifyLost = false;

                if(OnPlayerLost != null)
                {
                    OnPlayerLost.Invoke();
                }
            }
        }
    }
}
