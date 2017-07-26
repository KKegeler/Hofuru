using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Vector2 target;
    private Transform player;
    private float speed = 15.0f;
    private float dist = 5.0f;

    private Rigidbody2D me;
    private Vector2 direction;
    private float distanceSqr;
    
    private float dt = 0.0f;

    private ArrayList path;
    private int currentNode;

    private Node prev, curr;
    
	// Use this for initialization
	void Start () {
        me = GetComponent<Rigidbody2D>();
        player = GameObjectBank.Instance.player.transform;
        target = player.position;
	}

    public void FixedUpdate()
    {
        dt += Time.deltaTime;
        if(dt >= 1.0f)
        {
            dt -= 1.0f;
            CheckPath();
        }
        Seek();
    }

    private void Seek()
    {
        if (null == path)
            target = player.position;
        direction = (Vector2)target - me.position;
        distanceSqr = direction.sqrMagnitude;
        if (distanceSqr > (dist * dist))
        {
            Vector2 velo = new Vector2(direction.normalized.x * speed, me.velocity.y);
            me.velocity = velo;
        }else
            UpdateTarget();
    }

    private void UpdateTarget()
    {
        // update Target
        if(null == path || (currentNode+1) >= path.Count)
            target = player.position;
        else
        {
            if (curr == null)
                curr = ((Node)path[currentNode]);
            else
            {
                prev = curr;
                curr = ((Node)path[++currentNode]);
                // test jump
                if ((prev.type == Node.NodeType.JUMPABLE) &&
                    ((curr.type == Node.NodeType.JUMPABLE) || (curr.type == Node.NodeType.EDGE)))
                    Jump();
            }
            target = curr.position;
        }
    }

    private void CheckPath()
    {
        if(Mathf.Abs(player.position.y - me.position.y) > dist)
        {
            MakePath();
            return;
        }
        else
        {
            Vector2 dist = (Vector2)player.position - me.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(me.position, dist, dist.magnitude);
            foreach(RaycastHit2D hit in hits)
                if (!hit.transform.IsChildOf(me.transform))
                {
                    if (hit.transform.IsChildOf(player))
                        path = null;
                    else
                        MakePath();
                    return;
                }
        }
    }

    private void MakePath()
    {
        Node start = GraphManager.Instance.GetClosestGraphNode(me.position, player.position);
        Node goal = GraphManager.Instance.GetClosestGraphNode(player.position, me.position);        
        path = ((start != null) && (goal != null)) ? AStar.FindPath(start, goal) : null;
        if (null != path)
        {
            currentNode = 0;
            curr = null;
        }
        UpdateTarget();
    }

    private void Jump()
    {
        Debug.Log("jump");
        me.AddForce(Vector2.up * 450.0f, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (null != path)
        {
            for(int i = 1; i < path.Count; ++i)
            {
                Vector3 pos1 = ((Node)path[i - 1]).position;
                Vector3 pos2 = ((Node)path[i]).position;
                Debug.DrawLine(pos1, pos2, Color.red);
            }
            Gizmos.DrawSphere(curr.position, 1.0f);
        }
    }

}
