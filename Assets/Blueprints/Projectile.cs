using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float baseDamage;
    public RaycastHit hit;

    private Rigidbody myRigidbody;
    public float raycastCollideableDistance;
    [HideInInspector]
    public Vector3 targetPoint;
    [HideInInspector] public bool shouldBounce;
    [HideInInspector] public float angle;
    public float projectileLifetime;
    [HideInInspector] public Vector3 norm;
    public float penetrationProtrusinAmmount;
    [HideInInspector]
    public GameObject impactedObject;
    private bool finalImpact;
    private bool isImpactingMovingObject;
    public int numberOfRebounds;
    public float minReboundAngle;
    private LayerMask movingMask;
    private int bounceNumber;
    public event Action<GameObject> OnDeath;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

   

    public void CallStart()
    {
        movingMask = 1 << 8;
        
        StartLifeCountDown(projectileLifetime);
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            norm = hit.normal;
            impactedObject = hit.collider.gameObject;
            targetPoint = hit.point;
            CheckIfShouldBounce();

        }




    }

    // Update is called once per frame
	void Update ()
    {
        
		ForwardCast();
	}

    public bool CheckLayerMask(int layer)
    {
        if (hit.collider.gameObject.layer == 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckIfShouldBounce()
    {
        angle = (Vector3.Angle(transform.forward, hit.normal)) / 2f;
      
        if (angle < minReboundAngle && bounceNumber<numberOfRebounds)
        {
            shouldBounce = true;
        }
        else
        {

            if (CheckLayerMask(8)) isImpactingMovingObject = true;
        }
    }

    public void CalaculateRebound()
    {
        shouldBounce = false;
        Vector3 newVelocity = Vector3.Reflect(myRigidbody.velocity, norm);
        
        Physics.Raycast(targetPoint, newVelocity, out hit, 30f);
        //Debug.DrawLine(targetPoint,hit.point,Color.blue,200f);
        if (hit.collider != null)
        {
            if (!hit.collider.isTrigger)
            {
                targetPoint = hit.point;
                impactedObject = hit.collider.gameObject;
                norm = hit.normal;
                transform.LookAt(hit.point);
                CheckIfShouldBounce();
            }

           
        }

        
        myRigidbody.velocity = newVelocity;
       
        bounceNumber++;
       // print(bounceNumber);
    }

    public void ForwardCast()
    {
        if(finalImpact) return;
        if (Vector3.Distance(targetPoint, transform.position) < raycastCollideableDistance)
        {
            
            if (shouldBounce)
            {
              
               CalaculateRebound();
                
            }
            else
            {
                myRigidbody.isKinematic = true;
                transform.position = targetPoint-(transform.forward* penetrationProtrusinAmmount);
                if (isImpactingMovingObject)
                {
                   
                    if (impactedObject != null) transform.parent = impactedObject.transform;
                }
                finalImpact = true;
              
            }

        }
        
    }

    public void StartLifeCountDown(float time)
    {
        StartCoroutine(LifeCoundown(time));
    }

    public IEnumerator LifeCoundown(float time)
    {
        yield return new WaitForSeconds(time);
        DespawnProjectile();

    }

    public void DespawnProjectile()
    {

        StopAllCoroutines();
        bounceNumber = 0;
        isImpactingMovingObject = false;
        finalImpact = false;


        if (OnDeath != null) OnDeath(gameObject);
    }

    public void OnCollisionEnter()
    {
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;
    }
}
