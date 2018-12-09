using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableDevice : MonoBehaviour
{
    public RuntimeAnimatorController animatorController;
    public float range;

    public virtual void Use() { }
}
