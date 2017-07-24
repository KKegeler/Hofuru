using UnityEngine;
using System.Collections;

public class ShooterState : EnemyState
{

    private Transform target;

    public ShooterState(EnemyMachine machine, Transform target)
    {
        stateMachine = machine;
        this.target = target;
    }

    override public void UpdateState(float deltaTime)
    {
        float sqrtDist = (stateMachine.transform.position - target.position).sqrMagnitude;
        // player out of sight?
        if (sqrtDist > stateMachine.sightRange * stateMachine.sightRange)
        {
            stateMachine.ChangeToState("PATROL");
        }
        // player in sight | Collider check
        RaycastHit2D[] hits = Physics2D.RaycastAll(stateMachine.transform.position, (target.position - stateMachine.transform.position));
        // check collider
        foreach (RaycastHit2D hit in hits) {
            if (!hit.transform.IsChildOf(stateMachine.transform)) // get the first collider, which is not this gamebject
            {
                if (hit.collider.gameObject != target.gameObject) {   // player not visible
                    stateMachine.ChangeToState("PATROL");
                } else {
                    break;
                }
            }
        }
    }

    override public void OnTriggerEnter2D(Collider2D other)
    {
    }

    override public void EnterState()
    {
        Bhv_Flee flee = stateMachine.gameObject.AddComponent<Bhv_Flee>();
        flee.threat = target;
        flee.speed = stateMachine.runSpeed;
        flee.maxDistance = stateMachine.attackDistance;

        Bhv_LookAt look = stateMachine.gameObject.AddComponent<Bhv_LookAt>();
        look.target = target;
        look.rightOrientated = true;
    }

    override public void ExitState()
    {
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_Flee>());
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_LookAt>());
    }

    public override void PauseState(bool disable)
    {
        stateMachine.GetComponent<Bhv_Flee>().enabled = !disable;
        stateMachine.GetComponent<Bhv_LookAt>().enabled = !disable;
    }
}
