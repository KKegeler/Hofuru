using UnityEngine;
using System.Collections;
using System;

public class WalkToEntityState : EnemyState {

    private Transform target;

    public WalkToEntityState(EnemyMachine machine, Transform entity) {
        stateMachine = machine;
        target = entity;
    }

    override public void UpdateState(float deltaTime) {
        float sqrtDist = (stateMachine.transform.position - target.position).sqrMagnitude;
        // player out of sight?
        if (sqrtDist > stateMachine.sightRange * stateMachine.sightRange) {
            stateMachine.ChangeToState("PATROL");
        }
        // player reached?
        if (sqrtDist <= stateMachine.attackDistance * stateMachine.attackDistance) {
            stateMachine.ChangeToState("FIGHT");
        }
    }

    IEnumerator RayCastCoRoutine() {
        while (true) {
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
            yield return new WaitForSeconds(0.5f);
        }
    }

    override public void OnTriggerEnter2D(Collider2D other) {
    }

    override public void EnterState() {
        Bhv_Seek seek = stateMachine.gameObject.AddComponent<Bhv_Seek>();
        seek.target = target;
        seek.speed = stateMachine.runSpeed;

        Bhv_LookAt look = stateMachine.gameObject.AddComponent<Bhv_LookAt>();
        look.target = target;
        look.rightOrientated = true;

        stateMachine.StartCoroutine(RayCastCoRoutine());
    }

    override public void ExitState() {
        if (stateMachine != null) {
            stateMachine.removeComponent(stateMachine.GetComponents<Bhv_Seek>());
            stateMachine.removeComponent(stateMachine.GetComponents<Bhv_LookAt>());

            stateMachine.StopAllCoroutines();
        }
    }

    public override void PauseState(bool disable) {
        stateMachine.GetComponent<Bhv_Seek>().enabled = !disable;
        stateMachine.GetComponent<Bhv_LookAt>().enabled = !disable;

        if (disable) {
            stateMachine.StopAllCoroutines();
        } else {
            stateMachine.StartCoroutine(RayCastCoRoutine());
        }
    }
}
