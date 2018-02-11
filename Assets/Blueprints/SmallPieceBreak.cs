using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPieceBreak : MonoBehaviour
{
    private Rigidbody myRigidbody;
    private Collider myCollider;


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

    public void OnTriggerEnter(Collider other)
    {
        if(!other.GetComponent<Projectile>()) return;
        myRigidbody.isKinematic = false;
        myCollider.isTrigger = false;
    }

}
