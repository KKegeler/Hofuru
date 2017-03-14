using UnityEngine;
using System.Collections.Generic;

public abstract class StateMachine : MonoBehaviour {

    protected Dictionary<string, State> states;
    protected State currentState;

    public void Start()
    {
        states = new Dictionary<string, State>();
    }

    // Update is called once per frame
    public void Update()
    {
        currentState.UpdateState(Time.deltaTime);
    }

    public void ChangeToState(string newState)
    {
        currentState.ExitState();
        currentState = states[newState];
        currentState.EnterState();
    }

}
