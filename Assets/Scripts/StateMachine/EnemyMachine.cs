using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMachine : StateMachine {

    public string initState;
    public bool shooting;
    public float runSpeed;
    public float sightRange;
    public float attackDistance;
    public float patrolSpeed;
    public Transform[] wayPoints;

    new public void Start() {
        base.Start();
        Transform player = GameObjectBank.Instance.player.transform;
        states.Add("PATROL", new PatrolState(this, player));
        if (shooting) {
            states.Add("SHOOT", new ShooterState(this, player));
        } else {
            states.Add("WALKTOENTITY", new WalkToEntityState(this, player));
            states.Add("FIGHT", new FightState(this, player));
        }
        currentState = (states.ContainsKey(initState.ToUpper())) ? states[initState.ToUpper()] : states["PATROL"];
        currentState.EnterState();
    }

    new public void Update() {
        currentState.UpdateState(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ((EnemyState)currentState).OnTriggerEnter2D(other);
    }

    public void removeComponent(Component[] comp) {
        foreach (Component c in comp) {
            Destroy(c);
        }
    }

    public void DisableMachine() {
        currentState.ExitState();
        Destroy(this);
    }

    public void FreezeMachine()
    {
        currentState.PauseState(true);
    }

    public void UnfreezeMachine()
    {
        currentState.PauseState(false);
    }

}
