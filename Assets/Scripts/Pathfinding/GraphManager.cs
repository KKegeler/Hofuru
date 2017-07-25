using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {

    private static GraphManager _instance;

    private ArrayList nodes = new ArrayList();
    private ArrayList neighbourhood = new ArrayList();
    private ArrayList platforms = new ArrayList();
    private ArrayList neighboursByPlatform = new ArrayList();

    public float agentHeight = 4.127f; // height of ninjaGirls boxCollider
    public float agentMaxAngle = 50.0f; // angle in degrees
    public float maxJumpHeight = 12.0f; //for platforms with two blocks
    private float maxJumpDistance = 0.0f;
    private Vector2 leftDir;
    private Vector2 rightDir;

    public static GraphManager Instance { get { return _instance; } }

    public void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
            float a = agentMaxAngle * Mathf.Deg2Rad;
            float b = (180.0f - agentMaxAngle) * Mathf.Deg2Rad;
            maxJumpDistance = (maxJumpHeight / Mathf.Sin(a));
            leftDir = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * (-1.0f);
            rightDir = new Vector2(Mathf.Cos(b), Mathf.Sin(a)) * (-1.0f);
            MakeGraph();
        }
        else if (this != _instance)
            Destroy(this);
    }
    
    private void MakeGraph()
    {
        //
        // get level elements such as platforms, obstacles and traps
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("ground");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacles");
        GameObject[] traps = GameObject.FindGameObjectsWithTag("trap");
        //
        // save platform indices
        for (int i = 0; i < platforms.Length; ++i)
            this.platforms.Add(platforms[i]);
        // save obstacles as platforms (to jump on or over the obstacle)
        for (int i = 0; i < obstacles.Length; ++i)
            this.platforms.Add(obstacles[i]);
        //
        // create Neighbours Objects in neighbourhoodByPlatform
        for (int i = 0; i < this.platforms.Count; ++i)
            this.neighboursByPlatform.Add(new Neighbours());
        //
        // create nodes
        CreateNodes(obstacles, traps);
        //
        // search for neighbours by platform
        for(int i = 0; i < this.platforms.Count; ++i)
        {
            // make each node at a platform adjacent to all other nodes
            // on the platform if possible (raycasting)
            Neighbours n = ((Neighbours)neighboursByPlatform[i]);
            int[] array = n.GetNeighbourIndices();
            for (int j = 0; j < array.Length; ++j)
                for (int k = j; k < array.Length; ++k)
                    makeAdjacent(array[j], array[k]);
        }
    }

    private void CreateNodes(GameObject[] obstacles, GameObject[] traps)
    {
        // create platform nodes (edges)
        for (int i = 0; i < this.platforms.Count; ++i)
        {
            // calculate the upper left and upper right corner
            GameObject p = (GameObject)this.platforms[i];
            BoxCollider2D c = p.GetComponent<BoxCollider2D>();
            Vector2 leftCorner = new Vector2(p.transform.position.x, p.transform.position.y) + (Vector2.up * c.size.y * 0.5f) + (Vector2.left * c.size.x * 0.5f);
            Vector2 rightCorner = leftCorner + (Vector2.right * c.size.x);
            // create edgeNodes by platforms
            Node leftNode = CreateNode(leftCorner, i, Node.NodeType.EDGE, -1);
            Node rightNode = CreateNode(rightCorner, i, Node.NodeType.EDGE, -1);
            // for each edgeNode create a node on an other platform if possible
            if (null != leftNode) CheckJumpLocation(leftCorner, i, true, nodes.IndexOf(leftNode));
            if (null != rightNode) CheckJumpLocation(rightCorner, i, false, nodes.IndexOf(rightNode));
        }
        // create nodes at bottom of obstacles
        for (int i = 0; i < obstacles.Length; ++i)
        {
            // calculate lower left and lower right corner
            BoxCollider2D c = obstacles[i].GetComponent<BoxCollider2D>();
            Vector2 leftCorner = new Vector2(c.transform.position.x, c.transform.position.y) + (Vector2.down * c.size.y * 0.5f) + (Vector2.left * c.size.x * 0.5f);
            Vector2 rightCorner = leftCorner + (Vector2.right * c.size.x);
            // get platform under obstacle
            RaycastHit2D[] hits = Physics2D.RaycastAll(leftCorner, Vector2.down, 0.5f);
            foreach (RaycastHit2D hit in hits)
                if (!hit.transform.IsChildOf(c.transform))
                    if (hit.transform.tag.Equals("ground"))
                    {
                        int pIndex = this.platforms.IndexOf(hit.transform.gameObject);
                        // create Nodes
                        CreateNode(leftCorner + Vector2.left * c.size.x, pIndex, Node.NodeType.DEFAULT, -1);
                        CreateNode(rightCorner + Vector2.right * c.size.x, pIndex, Node.NodeType.DEFAULT, -1);
                    }
                    else
                        break;
        }
        // create nodes next to traps (to jump over)
        for (int i = 0; i < traps.Length; ++i)
        {
            // calculate lower left and lower right corner
            GameObject t = traps[i];
            Vector2 leftCorner = new Vector2(t.transform.position.x, t.transform.position.y) + (Vector2.left * 4.0f);
            Vector2 rightCorner = leftCorner + (Vector2.right * 8.0f);
            // get platform under trap
            RaycastHit2D[] hits = Physics2D.RaycastAll(leftCorner, Vector2.down, 4.0f);
            foreach (RaycastHit2D hit in hits)
                if (!hit.transform.IsChildOf(t.transform))
                    if (hit.transform.tag.Equals("ground"))
                    {
                        int pIndex = this.platforms.IndexOf(hit.transform.gameObject);
                        // create Nodes
                        CreateNode(leftCorner, pIndex, Node.NodeType.JUMPABLE, -1);
                        CreateNode(rightCorner, pIndex, Node.NodeType.JUMPABLE, -1);
                    }
                    else
                        break;
        }
    }

    private Node CreateNode(Vector2 pos, int platformIndex, Node.NodeType nType, int neighbour)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.up, agentHeight);
        foreach(RaycastHit2D hit in hits)
            if (!hit.transform.IsChildOf(((GameObject)this.platforms[platformIndex]).transform))
                return null;
        Node node = new Node(pos);
        node.type = nType;
        // save node at nodes and at neighboursByPlatform
        nodes.Add(node);
        Neighbours n = (Neighbours)neighboursByPlatform[platformIndex];
        n.Add(nodes.Count - 1); // because node was just added to nodes
        neighboursByPlatform[platformIndex] = n;
        // make nodes adjacent
        if (neighbour >= 0)
        {
            neighbourhood.Add(new Neighbours().Add(neighbour));
            Neighbours nNeighbour = (Neighbours)neighbourhood[neighbour];
            nNeighbour.Add(nodes.Count - 1);
            neighbourhood[neighbour] = nNeighbour;
        }
        return node;
    }

    private Node CheckJumpLocation(Vector2 pos, int platformIndex, bool left, int neighbour)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, left ? leftDir : rightDir, maxJumpDistance);
        foreach (RaycastHit2D hit in hits)
            if (!hit.transform.IsChildOf(((GameObject)this.platforms[platformIndex]).transform))
                if (hit.transform.tag.Equals("ground"))
                {
                    GameObject platform = hit.transform.gameObject;
                    int index = this.platforms.IndexOf(platform);
                    return CreateNode(hit.point, index, Node.NodeType.JUMPABLE, neighbour);
                }
        return null;
    }

    private void makeAdjacent(int node1, int node2)
    {
        Vector2 pos1 = ((Node)nodes[node1]).position + Vector2.up * agentHeight;
        Vector2 dir = (((Node)nodes[node2]).position + Vector2.up * agentHeight) - pos1;
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos1, dir, dir.magnitude);
        foreach (RaycastHit2D hit in hits)
            if (hit.transform.tag.Equals("obstacle"))
                return;
        Neighbours n1 = ((Neighbours)neighbourhood[node1]);
        Neighbours n2 = ((Neighbours)neighbourhood[node2]);
        n1.Add(node2);
        n2.Add(node1);
        neighbourhood[node1] = n1;
        neighbourhood[node2] = n2;
    }

}
