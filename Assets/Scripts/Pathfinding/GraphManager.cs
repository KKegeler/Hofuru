using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {

    private static GraphManager _instance;

    private ArrayList nodes = new ArrayList();
    private ArrayList neighborhood = new ArrayList();
    private ArrayList platforms = new ArrayList();
    private ArrayList neighborsByPlatform = new ArrayList();

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
        // create neighbors Objects in neighborhoodByPlatform
        for (int i = 0; i < this.platforms.Count; ++i)
            this.neighborsByPlatform.Add(new Neighbors());
        //
        // create nodes
        CreateNodes(obstacles, traps);
        //
        // search for neighbors by platform
        for(int i = 0; i < this.platforms.Count; ++i)
        {
            // make each node at a platform adjacent to all other nodes
            // on the platform if possible (raycasting)
            Neighbors n = ((Neighbors)neighborsByPlatform[i]);
            int[] array = n.GetNeighborIndices();
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
                        CreateNode(new Vector2(leftCorner.x, hit.point.y), pIndex, Node.NodeType.JUMPABLE, -1);
                        CreateNode(new Vector2(rightCorner.x, hit.point.y), pIndex, Node.NodeType.JUMPABLE, -1);
                    }
                    else if(hit.transform.tag.Equals("obstacle") || hit.transform.tag.Equals("trap"))
                        break;
        }
    }

    private Node CreateNode(Vector2 pos, int platformIndex, Node.NodeType nType, int neighbor)
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
        // save node at nodes and at neighborsByPlatform
        nodes.Add(node);
        Neighbors n = (Neighbors)neighborsByPlatform[platformIndex];
        n.Add(nodes.Count - 1); // because node was just added to nodes
        neighborsByPlatform[platformIndex] = n;
        // make a new neighborhood for node
        neighborhood.Add(new Neighbors());
        // make nodes adjacent
        if (neighbor >= 0)
        {
            Neighbors nCurrent = (Neighbors)neighborhood[nodes.Count - 1];
            Neighbors nneighbor = (Neighbors)neighborhood[neighbor];
            nCurrent.Add(neighbor);
            nneighbor.Add(nodes.Count - 1);
            neighborhood[nodes.Count - 1] = nCurrent;
            neighborhood[neighbor] = nneighbor;
        }
        return node;
    }

    private Node CheckJumpLocation(Vector2 pos, int platformIndex, bool left, int neighbor)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, left ? leftDir : rightDir, maxJumpDistance);
        foreach (RaycastHit2D hit in hits)
            if (!hit.transform.IsChildOf(((GameObject)this.platforms[platformIndex]).transform))
                if (hit.transform.tag.Equals("obstacle") || hit.transform.tag.Equals("trap")) break;
                else if (hit.transform.tag.Equals("ground"))
                {
                    GameObject platform = hit.transform.gameObject;
                    int index = this.platforms.IndexOf(platform);
                    return CreateNode(hit.point, index, Node.NodeType.JUMPABLE, neighbor);
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
        Neighbors n2 = ((Neighbors)neighborhood[node2]);
        Neighbors n1 = ((Neighbors)neighborhood[node1]);
        n1.Add(node2);
        n2.Add(node1);
        neighborhood[node1] = n1;
        neighborhood[node2] = n2;
    }

    /// <summary>
    /// searches and returns for the closest node on the platform (from pos) 
    /// which is in the same direction as the target position.
    /// </summary>
    /// <param name="pos">Any position in world coordinated</param>
    /// <param name="target">Any target position in world coordinates</param>
    /// <returns>Closest node to pos, which is in the same direction as the target or null</returns>
    public Node GetClosestGraphNode(Vector2 pos, Vector2 target)
    {
        // get index of platform
        bool hitted = false;
        Neighbors neighbors = new Neighbors();
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.down, agentHeight);
        foreach(RaycastHit2D hit in hits)
            if (hit.transform.tag.Equals("ground"))
            {
                hitted = true;
                neighbors = ((Neighbors)neighborsByPlatform[platforms.IndexOf(hit.transform.gameObject)]);
                break;
            }
        if(!hitted) return null;
        int[] indices = neighbors.GetNeighborIndices();
        Node best = null;
        float bestCost = 0.0f;
        bool left = (target.x - pos.x) < 0.0f;
        for(int i = 0; i < indices.Length; ++i)
        {
            Node candidate = (Node)nodes[indices[i]];
            Vector2 cPos = candidate.position;
            float cCost = (cPos - pos).magnitude;
            bool cDir = (target.x - cPos.x) < 0.0f;
            if((null == best) || ((left == cDir) && (cCost < bestCost)))
            {
                bestCost = cCost;
                best = candidate;
            }
        }
        return best;
    }

    /// <summary>
    /// returns the neighbor nodes from any node
    /// </summary>
    /// <param name="node">any node on the graph</param>
    /// <param name="neighbors">this Arraylist is filled by the function with all the neighbors frome node</param>
    public void GetNeighbors(Node node, ArrayList neighbors)
    {
        Neighbors n = (Neighbors)neighborhood[nodes.IndexOf(node)];
        int[] indices = n.GetNeighborIndices();
        for (int i = 0; i < indices.Length; ++i)
            neighbors.Add((Node)nodes[indices[i]]);
    }

    private void OnDrawGizmos()
    {
        if (readyToDraw)
        {           
            for(int i = 0; i < neighborhood.Count; ++i)
            {
                Neighbors n = ((Neighbors)neighborhood[i]);
                Vector2 pos1 = ((Node)nodes[i]).position;
                Gizmos.DrawSphere(new Vector3(pos1.x, pos1.y, 0.0f), 0.5f);
                int[] neighbors = n.GetNeighborIndices();
                for(int j = 0; j < neighbors.Length; ++j)
                {
                    Vector2 pos2 = ((Node)nodes[neighbors[j]]).position;
                    Debug.DrawLine(new Vector3(pos1.x, pos1.y, 0.0f), new Vector3(pos2.x, pos2.y, 0.0f), Color.red);
                }
            }
        }
    }

}
