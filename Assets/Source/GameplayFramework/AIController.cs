using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



/// <summary>
/// Base class for all AI Controllers
/// </summary>
public class AIController : Controller
{
    [Header("AI Controller")]
    public State currentState;
    public State remainState;
    public EnemyStats enemyStats;

    [HideInInspector] public Actor target;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector ]public float stateTime;

    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        target = GameInstance.GameMode.PlayerPawn;
        navMeshAgent = controlledPawn.GetComponent<NavMeshAgent>();
    }


    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if(controlledPawn)
        {
            if(currentState)
            {
                currentState.UpdateState(this);
            }
        }
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
        stateTime += Time.deltaTime;
        return stateTime >= duration;
    }


    private void OnExitState()
    {
        stateTime = 0;
    }
}
