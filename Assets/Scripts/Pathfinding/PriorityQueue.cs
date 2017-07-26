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
        nodes.Add(node);
        nodes.Sort();
        return this;
    }

    public PriorityQueue Remove(Node node)
    {
        nodes.Remove(node);
        return this;
    }

    public Node Pop()
    {
        nodes.Sort();
        Node node = (nodes.Count > 0) ? (Node) nodes[0] : null;
        if (null != node)
            nodes.RemoveAt(0);
        return node;
    }

    public bool Contains(object node)
    {
        return nodes.Contains(node);
    }

    public void FinalCleanUp()
    {
        for (int i = 0; i < nodes.Count; ++i)
            ((Node)nodes[i]).Reset();
    }

}
