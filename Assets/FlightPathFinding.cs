using System;
using System.Collections;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

public class FlightPathFinding : MonoBehaviour
{
    private FlyDroneState.StateEnum myState;
    private FlyDroneState.Triggers_Movement myTriggersMovement;

    private FlightGrid myCombatAreaGrid;
    private Vector3 initialPos;
    private Vector3 targetPos;
    private event Action OnPointHit;
    private event Action OnRouteComplete;


    private NodePath currentNodePath;
    private NodePathCluster_Start myPathCluster;
    private Coroutine followingRoutine;
    private int indexCounter;

    public GameObject beacon;



	// Use this for initialization
	void Start ()
	{
	    myCombatAreaGrid = FindObjectOfType<FlightGrid>();
	   
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           //AllignToGrid();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //MoveToPoint(beacon.transform.position, 5);
            myPathCluster = FindObjectOfType<NodePathCluster_Start>();
            currentNodePath = myPathCluster.SelectRandomNode();

            StartFollowingSteps();

        }
    }

    public void AllignToGrid()
    {
       // print("WorkerRunnign");
        initialPos = transform.position;
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += DetectPoint;
        worker.RunWorkerCompleted += OnCompletDetectPoint;
        worker.RunWorkerAsync();
    }


    private void DetectPoint(object sender, DoWorkEventArgs e)
    {
        Vector3 point = new Vector3();
        float distance = 10000;
        float tempDistance = 0;
        for (int i = 0; i < myCombatAreaGrid.gridBlockSize.x; i++)
        {
            for (int j = 0; j < myCombatAreaGrid.gridBlockSize.y; j++)
            {
                for (int k = 0; k < myCombatAreaGrid.gridBlockSize.z; k++)
                {
                    if (!myCombatAreaGrid.obstacleMap[i, j, k]) continue;
                    tempDistance = Vector3.Distance(initialPos, myCombatAreaGrid.triDPos[i, j, k]);
                    if(tempDistance>distance) continue;
                    distance= tempDistance;
                    targetPos = myCombatAreaGrid.triDPos[i, j, k];
                }
            }
        }
    }
    private void OnCompletDetectPoint(object sender, RunWorkerCompletedEventArgs e)
    {
      // print("WorkerDone, Result Target: "+targetPos );
       MoveToPoint(targetPos, 1);
    }

    public void MoveToPoint(Vector3 point, float speed)
    {
        float newSpeed = 0;
        newSpeed = Vector3.Distance(transform.position, point) / speed;
        transform.DOMove(point, newSpeed).SetEase(Ease.Linear).OnComplete(EventPointHit);
    }

    public void LookAtPoint(Vector3 point, float speed)
    {
        float newSpeed = 0;
        //newSpeed = Vector3.Angle(transform.position, point);
        transform.DOLookAt(point, speed);
    }

    public void EventPointHit()
    {
        if (OnPointHit != null) OnPointHit();
    }

    public void SeekPath()
    {

    }

    public void FollowPath(NodePath path)
    {
        OnPointHit = null;
        OnPointHit += StartFollowingSteps;
        MoveToPoint(path.listOfNodes.Find((a) => a.GetComponent<Node>().thisNodeType == Node.NodeType.StartNode).transform.position,5); //add speed values

    }

    public void StartFollowingSteps()
    {
       followingRoutine=  StartCoroutine(FollowSteps(currentNodePath));
    }

    public IEnumerator FollowSteps(NodePath path)
    {
        int target = path.listOfNodes.Count;
        indexCounter = 0;
        int compare=-1;
        OnPointHit = null;
        OnPointHit += OnStepReached;

        while (true)
        {
            if (indexCounter != compare)
            {
                if (indexCounter < target)
                {
                    MoveToPoint(path.listOfNodes[indexCounter].transform.position, 10);  //add speed values
                    LookAtPoint(path.listOfNodes[indexCounter].transform.position,0.3f);  //add speed values
                    compare = indexCounter;
                }
                else
                {
                    if (OnRouteComplete != null) OnRouteComplete();
                    myCombatAreaGrid = myPathCluster.connectedFlightGrid;
                    print("PathComplete");
                    break;
                }
            }


            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void OnStepReached()
    {
        indexCounter++;
    }
    

}
