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
    [HideInInspector ]public float[] actionsTimeElapsed;


    public override void NotifyPawnControlled(Pawn controlledPawn)
    {
        base.NotifyPawnControlled(controlledPawn);

        navMeshAgent = controlledPawn.GetComponent<NavMeshAgent>();
    }


    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if(controlledPawn)
        {
            currentState.UpdateState(this);
        }
    }


    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            OnExitState(currentState);

            currentState = nextState;
            actionsTimeElapsed = new float[currentState.actions.Length];
        }
    }


    public bool CheckIfCountDownElapsed(int index, float duration)
    {
        actionsTimeElapsed[index] += Time.deltaTime;
        return actionsTimeElapsed[index] >= duration;
    }


    public void ResetActionTimer(int actionIndex)
    {
        actionsTimeElapsed[actionIndex] = 0;
    }


    private void OnExitState(State oldState)
    {
        for (int i = 0; i < actionsTimeElapsed.Length; i++)
        {
            actionsTimeElapsed[i] = 0;
        } 
    }
}
