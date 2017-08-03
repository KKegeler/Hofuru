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
    private float jumpForce = 380.0f;

    // other components
    private EnemyMachine stateMachine;
    private EnemyGroundCheck groundCheck;
    private Animator animator;
    private Jump jump;
    private SpriteRenderer renderer;

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
        renderer = stateMachine.GetComponent<SpriteRenderer>();
        pathActive = false;
        dist = 0.2f;
    }

    public bool Init(EnemyMachine stateMachine, Vector2 dstPos, float speed)
    {
        this.speed = speed;
        this.stateMachine = stateMachine;
        InitMembers();
        // make path
        Vector2 srcPos = body.position;
        Node start = GraphManager.Instance.GetClosestGraphNode(srcPos, dstPos);
        Node goal = GraphManager.Instance.GetClosestGraphNode(dstPos, srcPos);
        path = ((null != start) && (null != goal)) ? AStar.FindPath(start, goal) : null;
        if(null != path)
        {
            currentIndex = -1;
            currentNode = null;
            UpdateTarget();
            pathActive = true;
        }
        return null != path;
    }

    public void FixedUpdate()
    {
        if (pathActive && groundCheck.grounded)
            Seek();
        if(null != animator) animator.SetFloat("speed", speed);
    }

    private void Seek()
    {
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
            renderer.flipX = target.x < body.position.x;
        }
    }

    private void CalculateJump(bool right)
    {
        float distance = (currentNode.position - previousNode.position).magnitude;
        Vector2 T = 0.707f * (right ? Vector2.right : Vector2.left) + 0.707f * Vector2.down;
        T *= distance;
        Vector2 jumpDir = 2.0f * currentNode.position - 2.0f * previousNode.position - T;
        distance = jumpDir.magnitude;
        jumpDir.Normalize();
        jump.JumpTo(jumpDir, distance * 13.8f); // please don't ask why exactly 13.8f, but it looks good.
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
