using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Actor
{
    [Header("Game State")]
    public float elapsedTime;


    public override void Init()
    {
        base.Init();

        
    }


    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        elapsedTime = Time.time;
    }

}
