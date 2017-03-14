using UnityEngine;
using System.Collections;
using System;

public class PatrolState : EnemyState
{

    private int currentWayPoint;
    private Transform wP;
    private Transform target;
    private Bhv_Seek seek;
    private Bhv_LookAt look;
    private bool up;

    public PatrolState(EnemyMachine machine, Transform entity)
    {
        stateMachine = machine;
        target = entity;
        currentWayPoint = 0;
        wP = stateMachine.wayPoints[currentWayPoint];
        up = true;
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
                        stateMachine.ChangeToState(stateMachine.shooting ? "SHOOT" : "WALKTOENTITY");
                    }
                    break;
                }
            }
        }
        //// Behaviour //
        //float distWP = (stateMachine.transform.position - wP.position).sqrMagnitude;
        //// wayPoint reached?
        //if (distWP <= 2.0f)
        //{
        //    NextWayPoint();
        //    wP = stateMachine.wayPoints[currentWayPoint];
        //    seek.target = wP;
        //    look.target = wP;
        //}
    }

    public void WayPointReachedCallback()
    {
        wP.gameObject.GetComponent<WayPointTrigger>().active = false;
        NextWayPoint();
        wP = stateMachine.wayPoints[currentWayPoint];
        WayPointTrigger wT = wP.gameObject.GetComponent<WayPointTrigger>();
        wT.attachedEnemy = stateMachine.gameObject;
        wT.pState = this;
        wT.active = true;
        seek.target = wP;
        look.target = wP;
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

    override public void OnTriggerEnter2D(Collider2D other)
    {
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

        look = stateMachine.gameObject.AddComponent<Bhv_LookAt>();
        look.target = wP;
        look.rightOrientated = true;
    }

    override public void ExitState()
    {
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_Seek>());
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_LookAt>());
    }

    public override void PauseState(bool disable)
    {
        stateMachine.GetComponent<Bhv_Seek>().enabled = !disable;
        stateMachine.GetComponent<Bhv_LookAt>().enabled = !disable;
    }

}
