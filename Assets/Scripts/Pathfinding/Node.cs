using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable {

    public float nodeTotalCost;
    public float estimatedCost;
    public NodeType type;
    public Node parent;
    public Vector2 position;

    public Node(Vector2 pos = default(Vector2))
    {
        estimatedCost = 0.0f;
        nodeTotalCost = 1.0f;
        type = NodeType.DEFAULT;
        parent = null;
        position = pos;
    }

    public Node Reset()
    {
        parent = null;
        return this;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node) obj;
        if (estimatedCost < node.estimatedCost) return -1;
        if (estimatedCost > node.estimatedCost) return 1;
        return 0;
    }

    public enum NodeType
    {
        DEFAULT,
        EDGE,
        JUMPABLE,
    }

}
