using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoControls : MonoBehaviour
{
    private TestRobotSpawner mySpawner;
    public int ammountOfRobotsToSpawn;
    public Shooting.Mode shootingSettingLeft;
    public Shooting.Mode shootingSettingRight;

    private List<Shooting> myShooting= new List<Shooting>();

    void Start()
    {
        mySpawner = FindObjectOfType<TestRobotSpawner>();
    }

    public void SpawnRobots()
    {
        mySpawner.robotsToSpawn = ammountOfRobotsToSpawn;
        mySpawner.StartSpawning();
    }

    public void UpdateGunSetting()
    {
        print("This no longer functions");
        //myShooting.AddRange(FindObjectsOfType<Shooting>());

        //for (int i = 0; i < myShooting.Count; i++)
        //{
        //    if (myShooting[i].isLeft)
        //    {
        //        myShooting[i].fireMode = shootingSettingLeft;
        //    }
        //    else
        //    {
        //        myShooting[i].fireMode = shootingSettingRight;
        //    }
        //}
    }
}
