using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_FollowPath : MonoBehaviour {

    // standalone seek behaviour for path following
    private Rigidbody2D body;
    private Vector2 target;
    private float speed;
    private float dist;
    private Vector2 direction;
    private Vector2 dstPos;

    // other components
    private EnemyMachine stateMachine;
    private EnemyGroundCheck groundCheck;
    private Animator animator;
    private Jump jump;
    private SpriteRenderer sRenderer;
    private GameObject trapEvasion;

    // path
    private ArrayList path;
    private int currentIndex;
    private Node previousNode, currentNode;
    private bool pathActive;
    
    private void InitMembers()
    {
        body = stateMachine.GetComponent<Rigidbody2D>();
        groundCheck = stateMachine.GetComponentInChildren<EnemyGroundCheck>();
        jump = stateMachine.GetComponentInChildren<Jump>();
        animator = stateMachine.GetComponent<Animator>();
        sRenderer = stateMachine.GetComponent<SpriteRenderer>();
        trapEvasion = stateMachine.transform.GetChild(2).gameObject;
        pathActive = false;
        dist = 0.1f;
        if ((null == body) || (null == groundCheck) || (null == jump) || (null == animator) || (null == sRenderer) || (null == trapEvasion))
            MakePath();
    }

    private void MakePath()
    {
        // make path
        Vector2 srcPos = body.position;
        Node start = GraphManager.Instance.GetClosestGraphNode(srcPos, dstPos);
        Node goal = GraphManager.Instance.GetClosestGraphNode(dstPos, srcPos);
        path = ((null != start) && (null != goal)) ? AStar.FindPath(start, goal) : null;
        if (null != path)
        {
            currentIndex = -1;
            currentNode = null;
            UpdateTarget();
            pathActive = true;
        }
    }
    
    public bool Init(EnemyMachine stateMachine, Vector2 dstPos, float speed)
    {
        this.speed = speed;
        this.stateMachine = stateMachine;
        this.dstPos = dstPos;
        InitMembers();
        MakePath();
        return null != path;
    }

    public void FixedUpdate()
    {
        if (pathActive && groundCheck.grounded)
            Seek();
        else if (pathActive && !groundCheck.grounded)
            WhileJumping();
        if(null != animator) animator.SetFloat("speed", speed);
    }

    private void Seek()
    {
        trapEvasion.SetActive(true);
        direction = target - body.position;
        direction.y = 0.0f;
        if (direction.sqrMagnitude > (dist * dist))
        {
            Vector2 v = new Vector2(direction.normalized.x * speed, (body.velocity.y != float.NaN)? body.velocity.y : 0.0f);
            body.velocity = v;
        }
        else
            UpdateTarget();
    }

    private void WhileJumping()
    {
        trapEvasion.SetActive(false);
        direction = target - body.position;
        direction.y = 0.0f;
        if (direction.sqrMagnitude <= (dist * dist))
            UpdateTarget();
    }

    private void UpdateTarget()
    {
        // test if the goalNode is reached
        if (++currentIndex >= path.Count)
            stateMachine.UnblockPathCheck();
        else
        {
            previousNode = currentNode;
            currentNode = ((Node)path[currentIndex]);
            // test if a jump is necessary
            if ((null != previousNode) && (previousNode.type == Node.NodeType.JUMPABLE) &&
                ((currentNode.type == Node.NodeType.JUMPABLE) || (currentNode.type == Node.NodeType.EDGE)))
                CalculateJump(currentNode.position.x > body.position.x);
            target = currentNode.position;
            sRenderer.flipX = target.x < body.position.x;
        }
    }

    private void CalculateJump(bool right)
    {
        if (null != jump)
        {
            trapEvasion.SetActive(false);
            float distance = (currentNode.position - previousNode.position).magnitude;
            float angle = 70.0f * Mathf.Deg2Rad;
            Vector2 dir = new Vector2((right ? 1.0f : -1.0f) * Mathf.Cos(angle), Mathf.Sin(angle));
            if (distance >= 12.0f)
                jump.JumpTo(dir, 350.0f);
            else if (distance >= 6.0f)
                jump.JumpTo(dir, 340.0f);
            else
                jump.JumpTo(dir, 250.0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (pathActive)
        {
            for(int i = currentIndex+1; i < path.Count;++i)
            {
                Vector3 pos1 = ((Node)path[i - 1]).position;
                Vector3 pos2 = ((Node)path[i]).position;
                Debug.DrawLine(pos1, pos2, Color.blue);
            }
            Gizmos.DrawSphere(currentNode.position, 1.0f);
        }
    }

}
