using UnityEngine;
using System.Collections;

public abstract class EnemyState : State
{

    protected EnemyMachine stateMachine;

    public abstract void UpdateState(float deltaTime);
    
    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void PauseState(bool disable);

    public abstract void UnblockPathCheck();

}
