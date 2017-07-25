using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbours {

    private ArrayList nodeIndices = new ArrayList();

    public Neighbours Add(int index)
    {
        if (!nodeIndices.Contains(index))
            nodeIndices.Add(index);
        return this;
    }

    public int[] GetNeighbourIndices()
    {
        int[] indices = new int[nodeIndices.Count];
        for (int i = 0; i < nodeIndices.Count; ++i)
            indices[i] = (int)nodeIndices[i];
        return indices;
    }

}
