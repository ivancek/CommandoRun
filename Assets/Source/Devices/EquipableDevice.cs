﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableDevice : Actor
{
    [Header("Setup")]
    public RuntimeAnimatorController animatorController;


    public float Range { get; private set; }
    
    public float RateOfFire { get; private set; }

    private float lastTimeUsed;

    /// <summary>
    /// A device can be used when enough time has passed (rateOfFire) since last usage. 
    /// Important: If it returns true, it will also sets lastTimeUsed to current Time.time
    /// </summary>
    public bool CanBeUsed
    {
        get
        {
            if(lastTimeUsed + RateOfFire < Time.time)
            {
                lastTimeUsed = Time.time;
                return true;
            }

            return false;
        }
    }

    public virtual void Use(Animator animator) { }

    public virtual void SetData(ItemData data)
    {
        Range = data.range;
        RateOfFire = data.rateOfFire;
    }

}
