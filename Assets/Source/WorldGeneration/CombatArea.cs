using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Combat Area class. It is responsible for managing objectives within the combat area.
/// Combar areas are spawned by the WorldGenerator class at predefined locations in the world.
/// </summary>
public class CombatArea : Actor
{
    public BoxCollider boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(11))
        {
            Debug.LogFormat("Player entered {0}", name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(11))
        {
            Debug.LogFormat("Player exited {0}", name);
        }
    }
}
