﻿using UnityEngine;
using System.Collections;
using System;

public class PatrolState : EnemyState
{

    private int currentWayPoint;
    private Transform wP;
    private Transform target;
    private Bhv_LookAt look;
    private bool up;
    
    private Bhv_Seek seek;
    private Bhv_FollowPath fpath;
    private bool pathActive;

    public PatrolState(EnemyMachine machine, Transform entity)
    {
        stateMachine = machine;
        target = entity;
        currentWayPoint = 0;
        wP = stateMachine.wayPoints[currentWayPoint];
        up = true;
        pathActive = false;
    }

    override public void UpdateState(float deltaTime)
    {
        // Transitions //
        float sqrtDist = (stateMachine.transform.position - target.position).sqrMagnitude;
        // player is alive AND player in sight range
        if ((target.GetComponent<Health>().currentHealth > 0.0f) && (sqrtDist <= stateMachine.sightRange * stateMachine.sightRange))
        {
            // Raycast //
            Vector2 dir = target.position - stateMachine.transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(stateMachine.transform.position, dir, dir.magnitude);
            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.transform.IsChildOf(stateMachine.transform)) // All colliders not attached to this gameObject
                {   // Just test first collider, after this gameObject
                    if (hit.collider.transform.IsChildOf(target))
                    { // player is in sightRange and isn't hiding
                        stateMachine.ChangeToState("WALKTOENTITY");
                    }
                    break;
                }
            }
        }
        CheckPath();
    }

    public void WayPointReachedCallback()
    {
        wP.gameObject.GetComponent<WayPointTrigger>().active = false;
        NextWayPoint();
        wP = stateMachine.wayPoints[currentWayPoint];
        WayPointTrigger wT = wP.gameObject.GetComponent<WayPointTrigger>();
        if (stateMachine)
        {
            wT.attachedEnemy = stateMachine.gameObject;
            wT.pState = this;
            wT.active = true;
            seek.target = wP;
            look.target = wP;
        }
    }

    /// <summary>
    /// sets currentWayPoint to next waypoint
    /// </summary>
    private void NextWayPoint()
    {
        if (currentWayPoint == (stateMachine.wayPoints.Length - 1)) up = false;
        if (currentWayPoint == 0) up = true;

        currentWayPoint += (up ? 1 : -1);
    }
    
    override public void EnterState()
    {
        currentWayPoint = 0;
        wP = stateMachine.wayPoints[currentWayPoint];
        WayPointTrigger wT = wP.gameObject.GetComponent<WayPointTrigger>();
        wT.attachedEnemy = stateMachine.gameObject;
        wT.pState = this;
        wT.active = true;
        up = true;

        seek = stateMachine.gameObject.AddComponent<Bhv_Seek>();
        seek.target = wP;
        seek.speed = stateMachine.patrolSpeed;
        
        fpath = stateMachine.gameObject.AddComponent<Bhv_FollowPath>();
        pathActive = false;

        look = stateMachine.gameObject.AddComponent<Bhv_LookAt>();
        look.target = wP;
        look.rightOrientated = true;

        CheckPath();
    }

    override public void ExitState()
    {
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_Seek>());
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_LookAt>());
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_FollowPath>());
    }

    public override void PauseState(bool disable)
    {
        if (stateMachine)
        {
            look.enabled = !disable;
            seek.enabled = !disable;
            fpath.enabled = !disable;
            if (!disable) CheckPath();
        }
    }

    private void BlockPathCheck()
    {
        pathActive = true;
        seek.enabled = false;
        look.enabled = false;
        fpath.enabled = true;
        if (!fpath.Init(stateMachine, (Vector2)wP.position, stateMachine.patrolSpeed))
            UnblockPathCheck();
    }

    public override void UnblockPathCheck()
    {
        pathActive = false;
        fpath.enabled = false;
        seek.enabled = true;
        look.enabled = true;
        look.target = wP;
    }

    private void CheckPath()
    {
        if (!pathActive)
        {
            if (Mathf.Abs(target.position.y - stateMachine.transform.position.y) > 4.0f)
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
                    if (!hit.transform.IsChildOf(wP))
                        BlockPathCheck();
                    return;
                }
            }
        }
    }
}
