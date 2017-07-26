using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar {

    public static PriorityQueue closedList, openList;

    private static float HeristicEstimatedCost(Node curNode, Node goalNode)
    {
        Vector2 dir = goalNode.position - curNode.position;
        return dir.magnitude;
    }

    public static ArrayList FindPath(Node start, Node goal)
    {
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeristicEstimatedCost(start, goal);

        closedList = new PriorityQueue();
        Node node = null;

        while(openList.Length != 0)
        {
            node = openList.Pop();
            // check if current node is goal node
            if (node.position == goal.position)
                return CalculatePath(node);
            // Get neighbors as ArrayList
            ArrayList neighbours = new ArrayList();
            GraphManager.Instance.GetNeighbours(node, neighbours);
            // check each neighbor
            for(int i = 0; i < neighbours.Count; ++i)
            {
                Node neighbourNode = (Node)neighbours[i];
                if (!closedList.Contains(node))
                {
                    float cost = HeristicEstimatedCost(node, neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeristicEstimatedCost(neighbourNode, goal);

                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;
                    neighbourNode.parent = node;

                    if (!openList.Contains(neighbourNode))
                        openList.Push(neighbourNode);
                }
            }
            // currentNode is closed
            closedList.Push(node);
        }
        return (node.position != goal.position) ? null : CalculatePath(node);
    }

    private static ArrayList CalculatePath(Node node)
    {
        return null;
    }

}
