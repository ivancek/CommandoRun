using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageReceiver
{
    Transform Transform { get; }
    void ReceiveDamage(float damage);
}
