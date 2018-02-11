using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodePathCluster_Start : MonoBehaviour
{
    private List<NodePath> listOfRoutes= new List<NodePath>();
    public FlightGrid connectedFlightGrid;

	// Use this for initialization
	void Start ()
	{
	    listOfRoutes = GetComponentsInChildren<NodePath>().ToList();
        DetectConnectedFlightGrid();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public NodePath SelectRandomNode()
    {
        return listOfRoutes[Random.Range(0, listOfRoutes.Count)];
    }

    public void DetectConnectedFlightGrid()
    {
        List<Collider> tempListOfOverlaps = Physics
            .OverlapBox(listOfRoutes[listOfRoutes.Count-1].listOfNodes[listOfRoutes[listOfRoutes.Count-1].listOfNodes.Count-1].transform.position, new Vector3(1, 1, 1)).ToList();
        connectedFlightGrid = tempListOfOverlaps.Find((a) => a.GetComponent<FlightGrid>()).GetComponent<FlightGrid>();


    }
}
