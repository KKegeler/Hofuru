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

    // other components
    private EnemyMachine stateMachine;
    private EnemyGroundCheck groundCheck;
    private Jump jump;

    // path
    private ArrayList path;
    private int currentIndex;
    private Node previousNode, currentNode;
    private bool pathActive;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<EnemyGroundCheck>();
        jump = GetComponentInChildren<Jump>();
        pathActive = false;
        dist = 1.0f;
    }

    public void Init(EnemyMachine stateMachine, Vector2 dstPos, float speed)
    {
        this.speed = speed;
        this.stateMachine = stateMachine;
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
        else
        {
            stateMachine.UnblockPathCheck();
        }
    }

    public void FixedUpdate()
    {
        if (pathActive && groundCheck.grounded)
            Seek();
    }

    private void Seek()
    {
        direction = target - body.position;
        direction.y = 0.0f;
        if (direction.sqrMagnitude > (dist * dist))
        {
            Vector2 v = new Vector2(direction.normalized.x * speed, body.velocity.y);
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
            if ((previousNode.type == Node.NodeType.JUMPABLE) &&
                ((currentNode.type == Node.NodeType.JUMPABLE) || (currentNode.type == Node.NodeType.EDGE)))
                if (currentNode.position.x < body.position.x)
                    jump.JumpLeft();
                else if (currentNode.position.x > body.position.x)
                    jump.JumpRight();
            target = currentNode.position;
        }
    }

}
