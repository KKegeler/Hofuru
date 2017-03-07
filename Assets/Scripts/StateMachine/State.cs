using UnityEngine;
using System.Collections;

public interface State
{

    void UpdateState(float deltaTime);

    void EnterState();

    void ExitState();

    void PauseState(bool disable);

}
