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

}
