using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct EnemyStats
{
    public float searchDuration;
    public float searchRange;
    public float lookRange;
}

/// <summary>
/// Base class for all AI Controllers
/// </summary>
public class AIController : Controller
{
    [Header("AI Controller")]
    public State currentState;
    public State remainState;
    public EnemyStats enemyStats;

    [HideInInspector] public Pawn target;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;


    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        navMeshAgent = controlledPawn.GetComponent<NavMeshAgent>();
    }


    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        currentState.UpdateState(this);
    }


    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }


    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }


    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
}
