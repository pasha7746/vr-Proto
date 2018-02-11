using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    public bool itemNeedsDoubleTapToDrop;
    public event Action OnPickup;

	// Use this for initialization
	void Start ()
	{
	    OnPickup += PickupAction;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void CallPickUp()
    {
        if (OnPickup != null) OnPickup();
    }

    public void PickupAction()
    {
        TestPlayThrowEnemy();
    }

    public void TestPlayThrowEnemy()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        if (nav && myRigidbody)
        {
            nav.enabled = false;
            myRigidbody.isKinematic = false;
            StartCoroutine(Counter());
        }

    }

    public void ReturnToNormal()
    {
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        if (nav && myRigidbody)
        {
            myRigidbody.isKinematic = true;
            nav.enabled = true;
        }
    }


    public IEnumerator Counter()
    {

        yield return new WaitForSeconds(10);
        ReturnToNormal();
        yield return null;
    }
}
