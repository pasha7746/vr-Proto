using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRobotSpawner : MonoBehaviour
{
    public int robotsToSpawn;
    public float spawnSpeed;
    public GameObject robot;

    private GameObject robotCache;
	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(Spawn());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnSpeed);
            robotsToSpawn--;
           robotCache= Instantiate(robot);
            print(transform.position);
            robotCache.transform.position = transform.position;
            if (robotsToSpawn <= 0)
            {
                print("all done");
                break;
            }

            yield return null;
        }



        yield return null;
    }
}
