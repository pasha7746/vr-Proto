using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Vector3 spawnOfset;

   // public GameObject projectile;
    public Mode fireMode;

    public float projectileLaunchSpeed;

    private SteamVR_TrackedObject myTracker;
    private Coroutine vibrateRoutine;
    private int controllerIndex;
    private Controller myController;

    //public bool isLeft;
    public bool vibration;

    public float raycastScanDistance;

    public float shotgunShrapnelParts;
    public float burstFireShots;
    public float burstFireRate;
    public Coroutine burstFireCoroutine;

    public float ammo;
    public float maxAmmo;

    public event Action OnShoot;
    public event Action OnUpdateAmmoUI;
    public event Action OnEmptyMag;
    public event Action OnReload;
    public float reloadSlideAccuracy;
    public Vector2 reloadVector;

    private Vector2 reloadSlideCatch= new Vector2(0,0);
    private Vector2 lastFrameCatch;

    public Text debugAmmoUI;
    private Pool myPool;

    public enum Mode
    {
        Single, Shotgun, BurstFire
    }
  
    void Awake()
    {
        myTracker = GetComponentInParent<SteamVR_TrackedObject>();
        myController = GetComponentInParent<Controller>();
        

    }

    // Use this for initialization
	void Start ()
	{
	    controllerIndex = myTracker.index.GetHashCode();
	    if (myController.isLeft)
	    {
	        myController.OnTriggerButtonLeft += OnTriggerHit;

	    }
	    else
	    {
	        myController.OnTriggerButtonRight += OnTriggerHit;

	    }
	    OnUpdateAmmoUI += DebugUI;

        if (OnUpdateAmmoUI != null) OnUpdateAmmoUI();
	    myPool = FindObjectOfType<Pool>();
	}

    void Update()
    {
        if (!myController.isLeft)
        {
           //print(myController.rightTrackPad);
            if (myController.rightTrackPad != Vector2.zero)
            {
                if (reloadSlideCatch == Vector2.zero)
                {
                    reloadSlideCatch = myController.rightTrackPad;
                }

                lastFrameCatch = myController.rightTrackPad;
            }
            else if(reloadSlideCatch!= Vector2.zero)
            {
                Vector2 path = lastFrameCatch - reloadSlideCatch;
                if (Vector2.Distance(path, reloadVector) <= reloadSlideAccuracy)
                {
                    Reload();
                }
                reloadSlideCatch= Vector2.zero;
            }

            myController.rightTrackPad = reloadSlideCatch;

        }
        else
        {
            if (myController.leftTrackPad != Vector2.zero)
            {
                if (reloadSlideCatch == Vector2.zero)
                {
                    reloadSlideCatch = myController.leftTrackPad;
                }

                lastFrameCatch = myController.leftTrackPad;
            }
            else if (reloadSlideCatch != Vector2.zero)
            {
                Vector2 path = lastFrameCatch - reloadSlideCatch;
                if (Vector2.Distance(path, reloadVector) <= reloadSlideAccuracy)
                {
                    Reload();
                }
                reloadSlideCatch = Vector2.zero;
            }

            myController.leftTrackPad = reloadSlideCatch;
        }
    }

    public void DebugUI()
    {
        debugAmmoUI.text = ammo + " / " + maxAmmo;
    }

    public void Reload()
    {
        ammo = maxAmmo;
        if (OnReload != null) OnReload();
        if (OnUpdateAmmoUI != null) OnUpdateAmmoUI();
    }

    public void OnTriggerHit()
    {
        if (ammo > 0)
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

            ammo--;
            if (OnShoot != null) OnShoot();
            if (OnUpdateAmmoUI != null) OnUpdateAmmoUI();
        }
        else
        {
            if (OnEmptyMag != null) OnEmptyMag();
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

       // GameObject tempProjectile = Instantiate(projectile, transform.position + transform.TransformDirection(spawnOfset), transform.rotation);
        GameObject tempProjectile= myPool.GiveProjectile();
        tempProjectile.SetActive(true);
        tempProjectile.transform.position = transform.position + transform.TransformDirection(spawnOfset);
        tempProjectile.transform.rotation = transform.rotation;
       

        Projectile tempComponent = tempProjectile.GetComponent<Projectile>();
        
        tempComponent.hit = hit;
        tempComponent.CallStart();
        Rigidbody temRigidbody= tempProjectile.GetComponent<Rigidbody>();
        temRigidbody.isKinematic = false;
        temRigidbody.AddForce((transform.forward) * projectileLaunchSpeed);


    }

    

}
