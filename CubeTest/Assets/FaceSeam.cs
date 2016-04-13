using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class FaceSeam
{
    Vector3 crossVector;
    TilePatchFace inFace;
    TilePatchFace outFace;
    GameObject[] inNodes, outNodes;
    bool reverse;

    Dictionary<GameObject, GameObject> edges = new Dictionary<GameObject, GameObject>();

    public FaceSeam(GameObject[] inNodes, GameObject[] outNodes, bool reverse = false)
    {
        this.inNodes = outNodes;
        this.outNodes = outNodes;
        this.reverse = reverse;
        GenerateSeam();
    }

    public void GenerateSeam()
    {
        if (inNodes.Length != outNodes.Length)
            return;
        for (int i = 0; i < inNodes.Length; ++i)
        {
            edges[inNodes[0]] = outNodes[reverse ? outNodes.Length - 1 - i : i];
        }
    }



}
