using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : EquipableDevice
{
    public float Damage { get; private set; }


    /// <summary>
    /// Since the knife uses a collider/trigger to deal damage (melee attack), 
    /// we don't want the knife to damage stuff as we pass by them.
    /// Use this to keep the knife (in)active in appropriate times.
    /// </summary>
    private bool isAttacking;

    private void OnTriggerEnter(Collider other)
    {
        if(!isAttacking) { return; }

        IDamageReceiver target = other.GetComponent<IDamageReceiver>();

        if(target != null)
        {
            target.ReceiveDamage(Damage);
            isAttacking = false;
            PlayUseSound();
        }
    }


    public override void SetData(ItemData data)
    {
        base.SetData(data);

        Damage = ((WeaponData)data).damage;
    }

    public override void Use(Animator animator)
    {
        if(CanBeUsed)
        {
            animator.SetTrigger("attack");
            isAttacking = true;
        }

        base.Use(animator);
    }

    public override void Drop()
    {
        base.Drop();

        transform.SetParent(null);
        isAttacking = false;

        GetComponent<Collider>().isTrigger = false;

        Rigidbody rBody = gameObject.AddComponent<Rigidbody>();
        rBody.velocity = new Vector3(.5f, 3.0f, .5f);
        rBody.angularVelocity = new Vector3(0, 3, 0);
    }
}
