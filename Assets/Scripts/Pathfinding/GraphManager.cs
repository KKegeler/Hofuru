using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {

    private static GraphManager _instance;

    private ArrayList nodes = new ArrayList();
    private ArrayList neighbourhood = new ArrayList();
    private ArrayList platforms = new ArrayList();
    private ArrayList neighboursByPlatform = new ArrayList();
    private PositionComparer comparer;

    public float agentHeight = 4.127f; // height of ninjaGirls boxCollider
    public float agentMaxAngle = 50.0f; // angle in degrees
    public float maxJumpHeight = 10.0f; //for platforms with two blocks
    private float maxJumpDistance = 0.0f;
    private Vector2 leftDir;
    private Vector2 rightDir;

    public static GraphManager Instance { get { return _instance; } }

    public bool drawGraph;
    private bool readyToDraw = false;

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
            comparer = new PositionComparer();
            MakeGraph();
            readyToDraw = drawGraph;
        }
        else if (this != _instance)
            Destroy(this);
    }
    
    private void MakeGraph()
    {
        //
        // get level elements such as platforms, obstacles and traps
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("ground");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        GameObject[] traps = GameObject.FindGameObjectsWithTag("trap");
        //
        // save platform indices
        for (int i = 0; i < platforms.Length; ++i)
            this.platforms.Add(platforms[i]);
        // save obstacles as platforms (to jump on or over the obstacle)
        for (int i = 0; i < obstacles.Length; ++i)
            this.platforms.Add(obstacles[i]);
        //
        // create neighbours Objects in neighbourhoodByPlatform
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
            int[] array = SortedIndices(n.GetNeighbourIndices());
            for (int j = 1; j < array.Length; ++j)
                makeAdjacent(array[j - 1], array[j]);
        }        
        comparer = null;
    }

    private void CreateNodes(GameObject[] obstacles, GameObject[] traps)
    {
        // create platform nodes (edges)
        for (int i = 0; i < this.platforms.Count; ++i)
        {
            // calculate the upper left and upper right corner
            GameObject p = (GameObject)this.platforms[i];
            BoxCollider2D c = p.GetComponent<BoxCollider2D>();
            Vector2 leftCorner = new Vector2(p.transform.position.x, p.transform.position.y) + 
                                 (Vector2.up * c.size.y * 0.5f * p.transform.lossyScale.y) + 
                                 (Vector2.left * c.size.x * 0.5f * p.transform.lossyScale.x) + 
                                 (Vector2.up * c.offset.y + Vector2.right * c.offset.x);
            Vector2 rightCorner = leftCorner + (Vector2.right * c.size.x * p.transform.lossyScale.x);
            // create edgeNodes by platforms
            Node leftNode = CreateNode(leftCorner, i, Node.NodeType.EDGE, -1);
            Node rightNode = CreateNode(rightCorner, i, Node.NodeType.EDGE, -1);
            // for each edgeNode create a node on an other platform if possible
            if (null != leftNode) CheckJumpLocation(leftCorner, i, true, nodes.IndexOf(leftNode));
            if (null != rightNode) CheckJumpLocation(rightCorner, i, false, nodes.IndexOf(rightNode));
            // create nodes by platform at a certain distance
            float stepSize = 10.0f;
            Vector2 currentPos = leftCorner + Vector2.right * stepSize;
            while(currentPos.x < rightCorner.x)
            {
                CreateNode(currentPos, i, Node.NodeType.DEFAULT, -1);
                currentPos += Vector2.right * stepSize;
            }
        }
        // create nodes at bottom of obstacles
        for (int i = 0; i < obstacles.Length; ++i)
        {
            // calculate lower left and lower right corner
            BoxCollider2D c = obstacles[i].GetComponent<BoxCollider2D>();
            Vector2 leftCorner = new Vector2(c.transform.position.x, c.transform.position.y) + 
                                 (Vector2.down * c.size.y * 0.5f * c.transform.lossyScale.y) + 
                                 (Vector2.left * c.size.x * 0.5f * c.transform.lossyScale.x) + 
                                 (Vector2.up * c.offset.y + Vector2.right * c.offset.x);
            Vector2 rightCorner = leftCorner + (Vector2.right * c.size.x * c.transform.lossyScale.x);
            // get platform under obstacle
            RaycastHit2D[] hits = Physics2D.RaycastAll(leftCorner, Vector2.down, (c.size.y * 0.5f)+4.0f);
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
    }

    private Node CreateNode(Vector2 pos, int platformIndex, Node.NodeType nType, int neighbour)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.up, agentHeight);
        foreach(RaycastHit2D hit in hits)
            if (!hit.transform.IsChildOf(((GameObject)this.platforms[platformIndex]).transform))
            {
                string tag = hit.transform.tag;
                if(tag.Equals("ground") || tag.Equals("obstacle") || tag.Equals("trap"))
                    return null;
            }
        Node node = new Node(pos);
        node.type = nType;
        // save node at nodes and at neighboursByPlatform
        nodes.Add(node);
        Neighbours n = (Neighbours)neighboursByPlatform[platformIndex];
        n.Add(nodes.Count - 1); // because node was just added to nodes
        neighboursByPlatform[platformIndex] = n;
        // make a new neighbourhood for node
        neighbourhood.Add(new Neighbours());
        // make nodes adjacent
        if (neighbour >= 0)
        {
            Neighbours nCurrent = (Neighbours)neighbourhood[nodes.Count - 1];
            Neighbours nneighbour = (Neighbours)neighbourhood[neighbour];
            nCurrent.Add(neighbour);
            nneighbour.Add(nodes.Count - 1);
            neighbourhood[nodes.Count - 1] = nCurrent;
            neighbourhood[neighbour] = nneighbour;
        }
        return node;
    }

    private Node CheckJumpLocation(Vector2 pos, int platformIndex, bool left, int neighbour)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, left ? leftDir : rightDir, maxJumpDistance);
        foreach (RaycastHit2D hit in hits)
            if (!hit.transform.IsChildOf(((GameObject)this.platforms[platformIndex]).transform))
                if (hit.transform.tag.Equals("obstacle") || hit.transform.tag.Equals("trap")) break;
                else if (hit.transform.tag.Equals("ground"))
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
            if (hit.transform.tag.Equals("obstacle") || hit.transform.tag.Equals("ground"))
                return;
        Neighbours n1 = ((Neighbours)neighbourhood[node1]);
        Neighbours n2 = ((Neighbours)neighbourhood[node2]);
        n1.Add(node2);
        n2.Add(node1);
        neighbourhood[node1] = n1;
        neighbourhood[node2] = n2;
    }

    private int[] SortedIndices(int[] indices)
    {
        ArrayList n = new ArrayList();
        foreach (int i in indices)
            n.Add(nodes[i]);
        n.Sort(comparer);
        int[] result = new int[indices.Length];
        for (int i = 0; i < indices.Length; ++i)
            result[i] = nodes.IndexOf(n[i]);
        return result;
    }

    /// <summary>
    /// searches and returns the closest node on the platform (from pos) 
    /// which is in the same direction as the target position.
    /// </summary>
    /// <param name="pos">Any position in world coordinated</param>
    /// <param name="target">Any target position in world coordinates</param>
    /// <returns>Closest node to pos, which is in the same direction as the target or null</returns>
    public Node GetClosestGraphNode(Vector2 pos, Vector2 target)
    {
        // get index of platform
        bool hitted = false;
        Neighbours neighbours = new Neighbours();
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.down, 4.0f);
        foreach(RaycastHit2D hit in hits)
            if (hit.transform.tag.Equals("ground"))
            {
                hitted = true;
                neighbours = ((Neighbours)neighboursByPlatform[platforms.IndexOf(hit.transform.gameObject)]);
                break;
            }
        if(!hitted) return null;
        int[] indices = neighbours.GetNeighbourIndices();
        Node best = null;
        float bestCost = 0.0f;
        for(int i = 0; i < indices.Length; ++i)
        {
            Node candidate = (Node)nodes[indices[i]];
            Vector2 cPos = candidate.position;
            float cCost = (cPos - pos).sqrMagnitude;
            if((null == best) || (cCost < bestCost))
            {
                bestCost = cCost;
                best = candidate;
            }
        }
        return best;
    }

    /// <summary>
    /// returns the neighbour nodes from any node
    /// </summary>
    /// <param name="node">any node on the graph</param>
    /// <param name="neighbours">this Arraylist is filled by the function with all the neighbours frome node</param>
    public void GetNeighbours(Node node, ArrayList neighbours)
    {
        Neighbours n = (Neighbours)neighbourhood[nodes.IndexOf(node)];
        int[] indices = n.GetNeighbourIndices();
        for (int i = 0; i < indices.Length; ++i)
        {
            Node nodeToAdd = ((Node)nodes[indices[i]]);
            neighbours.Add(nodeToAdd);
        }
    }

    private void OnDrawGizmos()
    {
        if (readyToDraw)
        {           
            for(int i = 0; i < neighbourhood.Count; ++i)
            {
                Neighbours n = ((Neighbours)neighbourhood[i]);
                Vector2 pos1 = ((Node)nodes[i]).position;
                Gizmos.DrawSphere(new Vector3(pos1.x, pos1.y, 0.0f), 0.5f);
                int[] neighbours = n.GetNeighbourIndices();
                for(int j = 0; j < neighbours.Length; ++j)
                {
                    Vector2 pos2 = ((Node)nodes[neighbours[j]]).position;
                    Debug.DrawLine(new Vector3(pos1.x, pos1.y, 0.0f), new Vector3(pos2.x, pos2.y, 0.0f), Color.red);
                }
            }
        }
    }

}
