using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
public class WeaponData : ItemData
{
    [Header("Weapon data")]
    public float damage;
}
