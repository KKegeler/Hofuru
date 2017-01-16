using UnityEngine;
using System.Collections;

public abstract class EnemyState : State
{

    protected EnemyMachine stateMachine;

    public abstract void UpdateState(float deltaTime);

    public abstract void OnTriggerEnter2D(Collider2D other);

    public abstract void EnterState();

    public abstract void ExitState();

}
