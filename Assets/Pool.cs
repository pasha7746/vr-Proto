using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject projectilePrefab;
    //public struct ProjectileComponentCache
    //{


    //}
    public List<GameObject> listOfBolts= new List<GameObject>();    //public for debug


    // Use this for initialization
    void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    public GameObject GiveProjectile()
    {
        
        if (SetObjectAlive()!=null)
        {
            return SetObjectAlive();
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<Projectile>().OnDeath += PutObjectToSleep;
            return projectile;
        }
    }

    public GameObject SetObjectAlive()
    {
        
        return listOfBolts.Find((a) => !a.activeSelf);


    }

    public void PutObjectToSleep(GameObject projectile)
    {
        Projectile projectileReset  = projectilePrefab.GetComponent<Projectile>();
        Projectile proejctileClone = projectile.GetComponent<Projectile>();
        

        proejctileClone.angle = projectileReset.angle;
        proejctileClone.hit = projectileReset.hit;
        proejctileClone.impactedObject = projectileReset.impactedObject;
        proejctileClone.norm = projectileReset.norm;
        proejctileClone.shouldBounce = projectileReset.shouldBounce;
        proejctileClone.targetPoint = projectileReset.targetPoint;

        projectile.SetActive(false);

        projectile.GetComponent<Rigidbody>().velocity= Vector3.zero;
        projectile.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        if (!listOfBolts.Contains(projectile))
        {
            listOfBolts.Add(projectile);

        }

    }

}
