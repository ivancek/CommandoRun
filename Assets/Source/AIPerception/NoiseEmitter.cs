using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    public bool IsEmitingNoise { get; private set; }

    public void SetEmittingNoise(bool isEmitting)
    {
        IsEmitingNoise = isEmitting;
    }
}
