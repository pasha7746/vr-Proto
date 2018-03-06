using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class FlightGrid : MonoBehaviour
{
    [HideInInspector]
    public Vector3[,,] triDPos;
    [HideInInspector]
    public bool[,,] obstacleMap;
    private Vector3 scaledRes;
    public Vector3 bounds;
    public bool showGrid;
    public bool showBlockPath;
    public Vector3 gridBlockSize;
    public GameObject target;
    private BackgroundWorker worker;
    private Object workweLock;
    private Vector3 start;

    public Vector3 postest;
    public GameObject marker;

	// Use this for initialization
	void Start ()
	{
	    bounds = GetComponent<BoxCollider>().size;
	    start = transform.position - (bounds / 2);

        //BuildGrid(gridBlockSize, true);
        worker = new BackgroundWorker();
        worker.DoWork += Worker_DoWork;
        worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        worker.RunWorkerAsync();

	}

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        BuildObstacleMap();
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        
       BuildGrid(gridBlockSize,true);
       
    }

    // Update is called once per frame
    void Update ()
    {
		//print(triDPos[(int)postest.x,(int)postest.y,(int)postest.z]);
       // marker.transform.position = triDPos[(int) postest.x, (int) postest.y, (int) postest.z];

    }


    public void BuildGrid(Vector3 res, bool buildObsMap)
    {
       // lock (workweLock)
        {
            scaledRes = new Vector3();
            scaledRes.x = bounds.x / res.x;
            scaledRes.z = bounds.z / res.z;
            scaledRes.y = bounds.y / res.y;

            triDPos = new Vector3[(int)res.x, (int)res.y, (int)res.z];
            obstacleMap = new bool[(int)res.x, (int)res.y, (int)res.z];

            for (int i = 0; i < res.y; i++)
            {
                for (int j = 0; j < res.z; j++)
                {
                    for (int k = 0; k < res.x; k++)
                    {

                        triDPos[k, i, j] = start + new Vector3(scaledRes.x * k, scaledRes.y * i, scaledRes.z * j);
                    }
                }
            }
            //if (buildObsMap)
            //{
            //    BuildObstacleMap();
            //}
        }
        
       // StartCoroutine(CheckGrid());
    }

    public void BuildObstacleMap()
    {
        List<Collider> overlaps= new List<Collider>();
        Vector3 halfExtends= new Vector3(scaledRes.x, scaledRes.y, scaledRes.z);

        for (int i = 0; i < gridBlockSize.x; i++)
        {
            for (int j = 0; j < gridBlockSize.y; j++)
            {
                for (int k = 0; k < gridBlockSize.z; k++)
                {
                    overlaps = Physics.OverlapBox(triDPos[i, j, k], halfExtends).ToList();
                    overlaps.RemoveAll((a) => a.isTrigger);
                    overlaps.RemoveAll((a) => !a.gameObject.isStatic);

                    obstacleMap[i, j, k] = (overlaps.Count ==0 ? true : false);
                }
            }
        }
       
        if (showGrid)
        {
            DrawGrid();
        }
        if (showBlockPath)
        {
            DrawBlockPath();
        }
    }

    public void DrawBlockPath() //visual debug
    {
        GameObject mark;
        for (int i = 0; i < gridBlockSize.x; i++)
        {
            for (int j = 0; j < gridBlockSize.y; j++)
            {
                for (int k = 0; k < gridBlockSize.z; k++)
                {
                    if (!obstacleMap[i, j, k])
                    {
                        mark = Instantiate(marker, triDPos[i, j, k], Quaternion.Euler(new Vector3()));
                        mark.GetComponent<Renderer>().material.color = Color.red;

                    }

                   

                }
            }
        }
    }


    public void DrawGrid()  //visual debug
    {
        for (int i = 0; i < gridBlockSize.x; i++)
        {
            if (i == (int)gridBlockSize.x - 1) continue;
            for (int j = 0; j < gridBlockSize.y; j++)
            {
                for (int k = 0; k < gridBlockSize.z; k++)
                {

                        Debug.DrawLine(triDPos[i, j, k], triDPos[i + 1, j, k], Color.red, 200f);

                    
                }
            }
        }
        for (int i = 0; i < gridBlockSize.x; i++)
        {
            for (int j = 0; j < gridBlockSize.y; j++)
            {
                if (j == (int)gridBlockSize.y - 1) continue;

                for (int k = 0; k < gridBlockSize.z; k++)
                {

                    Debug.DrawLine(triDPos[i, j, k], triDPos[i , j+1, k], Color.green, 200f);


                }
            }
        }
        for (int i = 0; i < gridBlockSize.x; i++)
        {
            for (int j = 0; j < gridBlockSize.y; j++)
            {
                for (int k = 0; k < gridBlockSize.z; k++)
                {
                    if (k == (int)gridBlockSize.z - 1) continue;

                    Debug.DrawLine(triDPos[i, j, k], triDPos[i , j, k+1], Color.blue, 200f);


                }
            }
        }
    }



    public IEnumerator CheckGrid()  //visual debug
    {
        foreach (var cPos in triDPos)
        {
            GameObject thing = Instantiate(target);
            thing.transform.position = cPos;
            yield return new WaitForSeconds(0.01f);
        }


        yield return null;
    }
}

//for (int i = 0; i<gridBlockSize.x; i++)
//{
           
//if (  i== (int)gridBlockSize.x-1) continue;
//for (int j = 0; j<gridBlockSize.y; j++)
//{

//for (int k = 0; k<gridBlockSize.z; k++)
//{
//if (!Physics.Raycast(triDPos[i, j, k], triDPos[i + 1, j, k], scaledRes.x, ~20,
//QueryTriggerInteraction.Ignore))
//{
//obstacleMap[i, j, k] = true;
//obstacleMap[i + 1, j, k] = true;
//}
//else
//{
//obstacleMap[i, j, k] = false;
//obstacleMap[i + 1, j, k] = false;
//}
                   
//}
                
//}
               
//}
//for (int i = 0; i<gridBlockSize.x; i++)
//{

           
//for (int j = 0; j<gridBlockSize.y; j++)
//{
//if (j == (int)gridBlockSize.y - 1) continue;
//for (int k = 0; k<gridBlockSize.z; k++)
//{

//if (!Physics.Raycast(triDPos[i, j, k], triDPos[i, j + 1, k], scaledRes.x, ~20,
//QueryTriggerInteraction.Ignore))
//{
//obstacleMap[i, j, k] = true;
//obstacleMap[i, j + 1, k] = true;
//}
//else
//{
//obstacleMap[i, j, k] = false;
//obstacleMap[i, j + 1, k] = false;
//}
                   
//}

//}

//}
//for (int i = 0; i<gridBlockSize.x; i++)
//{


//for (int j = 0; j<gridBlockSize.y; j++)
//{
               
//for (int k = 0; k<gridBlockSize.z; k++)
//{
//if (k == (int)gridBlockSize.z - 1) continue;
//if (!Physics.Raycast(triDPos[i, j, k], triDPos[i, j, k + 1], scaledRes.x, ~20,
//QueryTriggerInteraction.Ignore))
//{
//obstacleMap[i, j, k] = true;
//obstacleMap[i, j, k + 1] = true;
//}
//else
//{
//obstacleMap[i, j, k] = false;
//obstacleMap[i, j, k + 1] = false;
//}
                   
//}

//}

//}
