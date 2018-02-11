using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    public List<Node> listOfNodes= new List<Node>();
    public bool drawPath;

	// Use this for initialization
	void Start ()
	{
	    listOfNodes = GetComponentsInChildren<Node>().ToList();
	    if (drawPath)
	    {
	        DrawPath();
	    }
	}

    private void DrawPath()
    {
        for (int i = 0; i < listOfNodes.Count; i++)
        {
            if (listOfNodes.Count > i + 1)
                Debug.DrawLine(listOfNodes[i].transform.position, listOfNodes[i+1].transform.position,Color.green, 200f);
        }
    }

    // Update is called once per frame
	void Update ()
    {
		
	}
}
