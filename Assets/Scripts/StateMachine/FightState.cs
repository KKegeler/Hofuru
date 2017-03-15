using UnityEngine;
using System.Collections;

public class FightState : EnemyState
{

    private Transform target;

    private Animator animator;

    public FightState(EnemyMachine machine, Transform target)
    {
        stateMachine = machine;
        this.target = target;
        animator = stateMachine.GetComponent<Animator>();
    }

    override public void UpdateState(float deltaTime)
    {
        float sqrtDist = (stateMachine.transform.position - target.position).sqrMagnitude;
        // player out of sight? OR player dead
        if ((sqrtDist > stateMachine.sightRange * stateMachine.sightRange) || (target.GetComponent<Health>().currentHealth <= 0.0f))
        {
            stateMachine.ChangeToState("PATROL");
        }
        // player out of attack range?
        if (sqrtDist > stateMachine.attackDistance * stateMachine.attackDistance)
        {
            stateMachine.ChangeToState("WALKTOENTITY");
        }
    }

    override public void OnTriggerEnter2D(Collider2D other)
    {
    }

    override public void EnterState()
    {
        Bhv_HoldPosition hP = stateMachine.gameObject.AddComponent<Bhv_HoldPosition>();
        hP.position = stateMachine.transform.position;

        Bhv_LookAt look = stateMachine.gameObject.AddComponent<Bhv_LookAt>();
        look.target = target;
        look.rightOrientated = true;

        stateMachine.GetComponentInChildren<EnemyAttack>().enabled = true;
        stateMachine.GetComponentInChildren<EnemyAttack>().Initialize();

        animator.SetBool("doesMelee", true);
    }

    override public void ExitState()
    {
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_HoldPosition>());
        stateMachine.removeComponent(stateMachine.GetComponents<Bhv_LookAt>());

        stateMachine.GetComponentInChildren<EnemyAttack>().enabled = false;

        animator.SetBool("doesMelee", false);
    }

    public override void PauseState(bool disable)
    {
        stateMachine.GetComponent<Bhv_HoldPosition>().enabled = !disable;
        stateMachine.GetComponent<Bhv_LookAt>().enabled = !disable;
        
        stateMachine.GetComponentInChildren<EnemyAttack>().enabled = !disable;
    }
}
