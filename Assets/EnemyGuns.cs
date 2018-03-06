using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGuns : MonoBehaviour
{
    [System.Serializable]
    public struct GunStats
    {
        public GameObject gunModel;
        public GameObject projectileModel;
        public Vector2 fireRateRange;
        public float projectileSpeed;
        public Vector2 shotsInBurst;
        public Transform barrelEnding;
        [Range(0,1)]
        public float accuracy;
    }

    public SteamVR_Camera targetPlayers;
    private event Action<GunStats, int> OnAllGunsAimed;
    public List<GunStats> gunList;
    public List<Coroutine> fireRoutineList= new List<Coroutine>();

	// Use this for initialization
	void Start ()
	{
	    targetPlayers = FindObjectOfType<SteamVR_Camera>();
	    OnAllGunsAimed += FireGuns;
	    GetComponent<RobotCenterHealth>().OnDeath += OnDeathCutOfFireRoutines;

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
	    //if (Input.GetKeyDown(KeyCode.A))
	    //{
     //       AimGuns(targetPlayers.transform.position);
	    //}
    }

    public void OnDeathCutOfFireRoutines()
    {
       OnAllGunsAimed = null;
       StopAllCoroutines();
      

    }
    
    public void AimGuns(Vector3 target)
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            
            gunList[i].gunModel.transform.DOLookAt(target, 1f).OnComplete(AimComplete(gunList[i], i));

        }

        
    }

    public TweenCallback AimComplete(GunStats cGun, int index)
    {
        if (OnAllGunsAimed != null) OnAllGunsAimed(cGun, index);
        return null;
    }

    public IEnumerator FireRoutine(GunStats cGun)
    {

        int ammountOfShots = (int)(Random.Range(cGun.shotsInBurst.x, cGun.shotsInBurst.y+1));

        for (int i = 0; i < ammountOfShots; i++)
        {
           // Debug.Break();

            yield return new WaitForSeconds(Random.Range(cGun.fireRateRange.x, cGun.fireRateRange.y));
            ShootProjectile(cGun);
        }

        yield return null;
    }

    public void FireGuns(GunStats cGun, int coroutineIndex)
    {
        if (fireRoutineList.Count < gunList.Count)
        {
            fireRoutineList.Add(StartCoroutine(FireRoutine(cGun)));
        }
        else
        {
           
                fireRoutineList[coroutineIndex] = StartCoroutine(FireRoutine(cGun));
        }


    }


    public void ShootProjectile(GunStats cGun)   //projectile gets fired out using this method
    {
        GameObject spawnedProjectile = Instantiate(cGun.projectileModel);

        spawnedProjectile.transform.position = cGun.barrelEnding.position;
        spawnedProjectile.transform.LookAt(targetPlayers.transform.position);

        float distance = Vector3.Distance(cGun.barrelEnding.position, targetPlayers.transform.position);
        distance *= cGun.accuracy;
      //  Vector3 pos = cGun.barrelEnding.position+ 
        Vector3 accuracyOffest = (spawnedProjectile.transform.forward* distance) + new Vector3(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) ;
        accuracyOffest= Vector3.Normalize(accuracyOffest);
        spawnedProjectile.GetComponent<Rigidbody>().AddForce((spawnedProjectile.transform.forward+ accuracyOffest) * cGun.projectileSpeed);
    }

   


}
