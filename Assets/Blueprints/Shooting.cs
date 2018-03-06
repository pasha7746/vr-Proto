using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Vector3 spawnOfset;
    public GameObject projectile;
    public float projectileLaunchSpeed;
    private SteamVR_TrackedObject myTracker;
    private Coroutine vibrateRoutine;
    private int controllerIndex;
    public bool isLeft;
    public bool vibration;
    private AudioSource mySource;
    public float raycastScanDistance;
    public Mode fireMode;
    public float shotgunShrapnelParts;
    public float burstFireShots;
    public float burstFireRate;
    public Coroutine burstFireCoroutine;
    
    public enum Mode
    {
        Single, Shotgun, BurstFire
    }
  
    void Awake()
    {
        myTracker = GetComponentInParent<SteamVR_TrackedObject>();
       
        mySource = GetComponent<AudioSource>();
    }

    // Use this for initialization
	void Start ()
	{
	    controllerIndex = myTracker.index.GetHashCode();
    }
	
	// Update is called once per frame
	void Update ()
    {
	   
        if (Input.GetButtonDown("TriggerButRight")  && !isLeft)
        {
            OnTriggerHit();
        }
        if (Input.GetButtonDown("TriggerButLeft")&& isLeft)
        {
           OnTriggerHit();
        }

    }

    public void OnTriggerHit()
    {
        switch (fireMode)
        {
            case Mode.Single:
                VibrateAndShoot();
                break;
            case Mode.Shotgun:
                ShotgunShot();
                break;
            case Mode.BurstFire:
                burstFireCoroutine = StartCoroutine(BurstFireShot());

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
       
        
    }

    public IEnumerator BurstFireShot()
    {
        for (int i = 0; i < burstFireShots; i++)
        {
            yield return new WaitForSeconds(60/burstFireRate);
            VibrateAndShoot();
        }
        yield return null;
    }
    
    public void ShotgunShot()
    {
        for (int i = 0; i < shotgunShrapnelParts; i++)
        {
            VibrateAndShoot();
        }
    }

    public void VibrateAndShoot()
    {
        if (vibration)
        {
            if (vibrateRoutine != null)
            {
                StopCoroutine(vibrateRoutine);

            }
            vibrateRoutine = StartCoroutine(VibrateController(2000, 0.1f));
        }
        ShootProjectile();
    }

    public IEnumerator VibrateController(ushort stenght, float duration)
    {
        float time = 0;
        while (true)
        {
            if(time>=duration) break;
            time += Time.deltaTime;
            SteamVR_Controller.Input(controllerIndex).TriggerHapticPulse(stenght);
            yield return null;
        }
        vibrateRoutine = null;
        yield return null;
    }

    public void ShootProjectile()
    {
        
        RaycastHit hit;
        LayerMask mask = 1 << 9;
        mask = ~mask;
        Physics.Raycast(transform.position, transform.forward, out hit, raycastScanDistance,mask);
       // Debug.DrawLine(transform.position,hit.point,Color.red,200f);

        GameObject tempProjectile = Instantiate(projectile, transform.position + transform.TransformDirection(spawnOfset), transform.rotation);
        Projectile tempComponent = tempProjectile.GetComponent<Projectile>();
        
        tempComponent.hit = hit;
        tempComponent.CallStart();

        tempProjectile.GetComponent<Rigidbody>().AddForce((transform.forward) * projectileLaunchSpeed);

        //mySource.Play();
    }


}
