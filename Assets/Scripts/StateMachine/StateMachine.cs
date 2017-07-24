using UnityEngine;
using System.Collections.Generic;

public abstract class StateMachine : MonoBehaviour {

    protected Dictionary<string, State> states;
    protected State currentState;
    protected float time;

    public void Start()
    {
        states = new Dictionary<string, State>();
        time = 0.0f;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        float dt = Time.deltaTime;
        time += dt;
        if(time >= 1.0f){
            time -= 1.0f;
            currentState.UpdateState(dt);
        }
    }

    public void ChangeToState(string newState)
    {
        currentState.ExitState();
        currentState = states[newState];
        currentState.EnterState();
    }

}
