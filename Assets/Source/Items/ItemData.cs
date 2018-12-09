using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// Name to use for game objects when they are spawned (for easier debugging). E.g. Knife.
    /// </summary>
    public string shortName;
    
    /// <summary>
    /// Use this to display the name of the item in game.
    /// </summary>
    public string displayName;


    /// <summary>
    /// Short description. This is visible in the tooltip.
    /// </summary>
    public string description;


    /// <summary>
    /// Object to be used when equipping this weapon.
    /// </summary>
    public EquipableDevice prefab;


    /// <summary>
    /// Since some meshes are specifically rotated to fit the animation, 
    /// they don't all appear the same in world when rotated Vector3.zero
    /// This is why we use a special prefab (a container) for the mesh so we 
    /// can place/rotate it properly to look nice when appearing as a pickup.
    /// </summary>
    public GameObject pickupPrefab;
}
