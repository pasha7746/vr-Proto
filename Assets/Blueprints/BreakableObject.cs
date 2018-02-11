using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject wholeModel;
    public GameObject brokenModel;
    public event Action OnObjectHit;
    private Rigidbody myRigidbody;
    private Collider myCollider;
    public float despawnTimer;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }

    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void  OnTriggerEnter(Collider other)
    {
        
        if(!other.GetComponent<Projectile>()) return;
        Destroy(wholeModel);
        brokenModel.SetActive(true);
        if (myRigidbody)
        {
            myRigidbody.isKinematic = true;

        }
        if (OnObjectHit != null) OnObjectHit();
        myCollider.enabled = false;
        StartCoroutine(DespawnTimer());
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTimer);
        Destroy(gameObject);

        yield return null;
    }



}
