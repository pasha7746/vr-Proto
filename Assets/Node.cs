using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{


    public enum NodeType
    {
        none,
        StepNode,
        LinkNode,
        StartNode,
        EndNode
    }

    public NodeType thisNodeType;

   

}
