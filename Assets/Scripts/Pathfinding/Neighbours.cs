using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbors {

    private ArrayList nodeIndices = new ArrayList();

    public Neighbors Add(int index)
    {
        if (!nodeIndices.Contains(index))
            nodeIndices.Add(index);
        return this;
    }

    public int[] GetNeighborIndices()
    {
        int[] indices = new int[nodeIndices.Count];
        for (int i = 0; i < nodeIndices.Count; ++i)
            indices[i] = (int)nodeIndices[i];
        return indices;
    }

}
