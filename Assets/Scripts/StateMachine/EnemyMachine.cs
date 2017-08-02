﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMachine : StateMachine {

    public string initState;
    public float runSpeed;
    public float sightRange;
    public float attackDistance;
    public float patrolSpeed;
    public GameObject wayPointPrototype;
    public float wPDistance;
    public Transform[] wayPoints;

    new public void Start() {
        base.Start();
        if (wayPoints.Length == 0)
        {
            InitializeWayPoints();
        }
        Transform player = GameObjectBank.Instance.player.transform;
        states.Add("PATROL", new PatrolState(this, player));
        states.Add("WALKTOENTITY", new WalkToEntityState(this, player));
        states.Add("FIGHT", new FightState(this, player));
        currentState = (states.ContainsKey(initState.ToUpper())) ? states[initState.ToUpper()] : states["PATROL"];
        currentState.EnterState();
    }

    private void InitializeWayPoints()
    {
        wayPoints = new Transform[2];
        GameObject wP = Instantiate(wayPointPrototype, GameObjectBank.Instance.levelElements.transform, true);
        wP.transform.position = transform.position + wPDistance * Vector3.left;
        wayPoints[0] = wP.transform;
        wP = Instantiate(wayPointPrototype, GameObjectBank.Instance.levelElements.transform, true);
        wP.transform.position = transform.position - wPDistance * Vector3.left;
        wayPoints[1] = wP.transform;
    }

    new public void FixedUpdate() {
        float dt = Time.deltaTime;
        time += dt;
        if(time >= 1.0f){
            time -= 1.0f;
            currentState.UpdateState(Time.deltaTime);
        }
    }
    
    public void removeComponent(Component[] comp) {
        foreach (Component c in comp) {
            Destroy(c);
        }
    }

    public void DisableMachine() {
        currentState.ExitState();
        states.Clear();
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

    public void UnblockPathCheck()
    {
        ((EnemyState)currentState).UnblockPathCheck();
    }

}
