using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue {

    private ArrayList nodes;

    public PriorityQueue()
    {
        nodes = new ArrayList();
    }

    public int Length { get { return nodes.Count; } }

    public Node First()
    {
        return (nodes.Count > 0) ? (Node)nodes[0] : null;
    }

    public PriorityQueue Push(Node node)
    {
        if(nodes.Count == 0)
            nodes.Add(node);
        else
        {
            int i = nodes.Count;
            while ((i > 0) && (((Node)nodes[i - 1]).CompareTo(node) == 1))
                --i;
            nodes.Insert(i, node);
        }
        return this;
    }

    public PriorityQueue Remove(Node node)
    {
        nodes.Remove(node);
        return this;
    }

    public Node Pop()
    {
        Node node = (nodes.Count > 0) ? (Node) nodes[0] : null;
        if (null != node)
            nodes.RemoveAt(0);
        return node;
    }

}
