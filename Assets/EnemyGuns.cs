using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyGuns : MonoBehaviour
{
    [System.Serializable]
    public struct GunStats
    {
        public GameObject gunModel;
        public GameObject projectileModel;
        public float fireRate;
        public float projectileSpeed;
        public Vector2 shotsInBurst;
        public Transform barrelEnding;
    }

    public Players targetPlayers;
    private event Action<GunStats> OnAllGunsAimed;
    public List<GunStats> gunList;
    public Coroutine fireRoutine;

	// Use this for initialization
	void Start ()
	{
	    targetPlayers = FindObjectOfType<Players>();
	    OnAllGunsAimed += FireGuns;
        GunStats tempGunStats= new GunStats();

	    for (int i = 0; i < gunList.Count; i++)
	    {
	        tempGunStats= gunList[i];
	        tempGunStats.barrelEnding = gunList[i].gunModel.GetComponentInChildren<ProjectileSpawnPoint>().transform;
	        gunList[i] = tempGunStats;
	    }
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.A))
	    {
            AimGuns(targetPlayers.transform.position);
	    }
    }

    public void BreakFire()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
        }
    }

    public void AimGuns(Vector3 target)
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            gunList[i].gunModel.transform.DOLookAt(target, 1f).OnComplete(AimComplete(gunList[i]));

        }

        
    }

    public TweenCallback AimComplete(GunStats cGun)
    {
        if (OnAllGunsAimed != null) OnAllGunsAimed(cGun);
        return null;
    }

    public IEnumerator FireRoutine()
    {

        //loop here with waits and check conditions...fire a shot per loop.

        yield return null;
    }

    public void FireGuns(GunStats cGun)
    {
       
            //if (Cooldown())
            //{
            //    ShootProjectile(cGun);
            //}
        

        //call fire routine here...

    }


    public void ShootProjectile(GunStats cGun)   //projectile gets fired out using this method
    {
        GameObject spawnedProjectile = Instantiate(cGun.projectileModel);

        spawnedProjectile.transform.position = cGun.barrelEnding.position;
        spawnedProjectile.transform.LookAt(targetPlayers.transform.position);
        spawnedProjectile.GetComponent<Rigidbody>().AddForce(spawnedProjectile.transform.forward* cGun.projectileSpeed);
    }

    //public bool Cooldown() //all checks for fire rate and the like...
    //{

    //    return true;
    //}


}
