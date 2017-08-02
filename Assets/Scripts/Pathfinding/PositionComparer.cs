using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionComparer : IComparer
{

    // Compares Nodes! by x position
    public int Compare(object x, object y)
    {
        float x1 = ((Node)x).position.x;
        float x2 = ((Node)y).position.x;

        return x1 < x2 ? -1 : x1 > x2 ? 1 : 0;
    }
}
