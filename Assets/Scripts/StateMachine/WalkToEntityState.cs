using UnityEngine;
using System.Collections;
using System;

public class WalkToEntityState : EnemyState {

    private Transform target;
    private bool pathActive;

    private Bhv_Seek seek;
    private Bhv_FollowPath fpath;

    public WalkToEntityState(EnemyMachine machine, Transform entity) {
        stateMachine = machine;
        target = entity;
        pathActive = false;
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
        CheckPath();
    }

    private void CheckPath()
    {
        if (!pathActive)
        {
            if(Mathf.Abs(target.position.y - stateMachine.transform.position.y) > 4.0f)
            {
                BlockPathCheck();
                return;
            }
            // player in sight? collision check
            Vector2 dir = (Vector2)(target.position - stateMachine.transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(stateMachine.transform.position, dir, dir.magnitude);
            // check collider
            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.transform.IsChildOf(stateMachine.transform)) // get the first collider, which is not this gamebject
                {
                    if (!hit.transform.IsChildOf(target))
                        BlockPathCheck();
                    return;
                }
            }
        }
    }
    
    override public void EnterState() {
        seek = stateMachine.gameObject.AddComponent<Bhv_Seek>();
        seek.target = target;
        seek.speed = stateMachine.runSpeed;

        fpath = stateMachine.gameObject.AddComponent<Bhv_FollowPath>();
        pathActive = false;
        CheckPath();

        Bhv_LookAt look = stateMachine.gameObject.AddComponent<Bhv_LookAt>();
        look.target = target;
        look.rightOrientated = true;
    }

    override public void ExitState() {
        if (stateMachine != null) {
            stateMachine.removeComponent(stateMachine.GetComponents<Bhv_Seek>());
            stateMachine.removeComponent(stateMachine.GetComponents<Bhv_LookAt>());
            stateMachine.removeComponent(stateMachine.GetComponents<Bhv_FollowPath>());
        }
    }

    public override void PauseState(bool disable) {
        if (stateMachine)
        {
            stateMachine.GetComponent<Bhv_LookAt>().enabled = !disable;
            seek.enabled = !disable;
            fpath.enabled = !disable;
            if(!disable) CheckPath();
        }
    }

    private void BlockPathCheck()
    {
        pathActive = true;
        seek.enabled = false;
        fpath.enabled = true;
        if (!fpath.Init(stateMachine, (Vector2)target.position, stateMachine.runSpeed))
            UnblockPathCheck();
    }

    public override void UnblockPathCheck()
    {
        pathActive = false;
        fpath.enabled = false;
        seek.enabled = true;
    }
}
